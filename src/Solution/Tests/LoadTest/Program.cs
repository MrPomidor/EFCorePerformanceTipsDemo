using System.Collections.Concurrent;
using System.Text;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using Reusables.Storage;
using Reusables.Utils;

// SUT configuration
const int pageSize = 50;
const string baseUrl = "https://localhost:44327/api/products";

// NBomber confituration
const int paralellClients = 30;
TimeSpan warmupTime = TimeSpan.FromSeconds(10);
TimeSpan timeToRun = TimeSpan.FromSeconds(30);


// get by id scenario
var getByIdScenario = () => GetByIdScenario(isFull: false);
// get by id full scenario
var getByIdFullScenario = () => GetByIdScenario(isFull: true);
// get pages scenario
var getPageScenario = () => GetPagesScenario(isFull: false);
// get full pages scenario
var getFullPageScenario = () => GetPagesScenario(isFull: true);
// edit product scenario
var editProductScenario = () => EditProductScenario();
// create product scenario
var createProductScenario = () => CreateProductScenario();

var scenariousToRun = new[]
{
    (nameof(getByIdScenario), getByIdScenario),
    (nameof(getByIdFullScenario), getByIdFullScenario),
    (nameof(getPageScenario), getPageScenario),
    (nameof(getFullPageScenario), getFullPageScenario),
    (nameof(editProductScenario), editProductScenario),
    (nameof(createProductScenario), createProductScenario),
};

foreach (var (scenarioName, scenarioTask) in scenariousToRun)
{
    Console.WriteLine($"LOAD_TEST: Creating {scenarioName} ...");

    var scenario = await scenarioTask();

    Console.WriteLine($"LOAD_TEST: Running {scenarioName} ...");

    NBomberRunner
        .RegisterScenarios(scenario)
        .WithReportFolder(scenarioName)
        .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
        .Run();
}

Console.WriteLine("LOAD_TEST: Press any key ...");
Console.ReadKey();



async Task<Scenario> GetByIdScenario(bool isFull)
{
    // get ids
    var productIds = await GetProductIds();

    // get product ids feed
    var productIdsFeed = Feed.CreateCircular("productIds", productIds);

    // get client factory
    var clientFactory = CreateClientFactory();

    // create step
    var stepName = isFull ? "getProductByIdFullStep" : "getProductByIdStep";
    var step = Step.Create(stepName, clientFactory, productIdsFeed, async (context) =>
    {
        var resourseUrl = isFull ?
            $"{baseUrl}/{context.FeedItem}/full" :
            $"{baseUrl}/{context.FeedItem}";
        var response = await context.Client.GetAsync(resourseUrl);
        if (response.IsSuccessStatusCode)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return Response.Ok(payload: bytes, statusCode: (int)response.StatusCode);
        }
        else
        {
            if (context.Logger != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                context.Logger.Error(responseContent);
            }
            return Response.Fail(statusCode: (int)response.StatusCode, error: response.ReasonPhrase);
        }
    });

    // create scenario
    var scenarioName = isFull ? "Product by Id Full" : "Product by Id";
    var scenario = ScenarioBuilder.CreateScenario(scenarioName, step)
        .WithWarmUpDuration(warmupTime)
        .WithLoadSimulations(
            LoadSimulation.NewKeepConstant(_copies: paralellClients, _during: timeToRun)
        );

    return scenario;
}

async Task<Scenario> GetPagesScenario(bool isFull)
{
    // get pages count
    var pagesCount = await GetPagesCount();

    // get pages feed
    var productPageNumbersFeed = Feed.CreateCircular("pageNumbers", Enumerable.Range(1, pagesCount));

    // get client factory
    var clientFactory = CreateClientFactory();

    // create step
    var stepName = isFull ? "getProductsPageFullStep" : "getProductsPageStep";
    var step = Step.Create(stepName, clientFactory, productPageNumbersFeed, async (context) =>
    {
        var resourseUrl = isFull ?
            $"{baseUrl}/list/full?page={context.FeedItem}&pageSize={pageSize}" :
            $"{baseUrl}/list?page={context.FeedItem}&pageSize={pageSize}";
        var response = await context.Client.GetAsync(resourseUrl);
        if (response.IsSuccessStatusCode)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return Response.Ok(payload: bytes, statusCode: (int)response.StatusCode);
        }
        else
        {
            if (context.Logger != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                context.Logger.Error(responseContent);
            }
            return Response.Fail(statusCode: (int)response.StatusCode, error: response.ReasonPhrase);
        }
    });

    // create scenario
    var scenarioName = isFull ? "Products Page Full" : "Products Page";
    var scenario = ScenarioBuilder.CreateScenario(scenarioName, step)
        .WithWarmUpDuration(warmupTime)
        .WithLoadSimulations(
            LoadSimulation.NewKeepConstant(_copies: paralellClients, _during: timeToRun)
        );

    return scenario;
}

async Task<Scenario> EditProductScenario()
{
    // get product Ids
    var productIds = await GetProductIds();

    // get productIds with fake names feed
    var productIdNewNameFeed = Feed.CreateCircular(
        "pageNumbers",
        productIds.Select(x => (productId: x, newName: ProductsGenerator.Instance.GenerateProductName()))
        );

    // get client factory
    var clientFactory = CreateClientFactory();

    // create step
    var step = Step.Create("editProduct", clientFactory, productIdNewNameFeed, async (context) =>
    {
        var putBody = new StringContent("\""+context.FeedItem.newName+"\"", Encoding.UTF8, "application/json");

        var response = await context.Client.PutAsync($"{baseUrl}/{context.FeedItem.productId}", putBody);
        if (response.IsSuccessStatusCode)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return Response.Ok(payload: bytes, statusCode: (int)response.StatusCode);
        }
        else
        {
            if (context.Logger != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                context.Logger.Error(responseContent);
            }
            return Response.Fail(statusCode: (int)response.StatusCode, error: response.ReasonPhrase);
        }
    });

    // create scenario
    var scenario = ScenarioBuilder.CreateScenario("Edit Product", step)
        .WithWarmUpDuration(warmupTime)
        .WithLoadSimulations(
            LoadSimulation.NewKeepConstant(_copies: paralellClients, _during: timeToRun)
        );

    return scenario;
}

Task<Scenario> CreateProductScenario()
{
    // get products to be created
    var newProductsFeed = Feed.CreateCircular(
        "products",
        Enumerable.Range(0, 500_000).Select(i => ProductsGenerator.Instance.GenerateProduct())
        );

    // get client factory
    var clientFactory = CreateClientFactory();

    // create step
    var step = Step.Create("createProduct", clientFactory, newProductsFeed, async (context) =>
    {
        var postBody = new StringContent(JsonConvert.SerializeObject(context.FeedItem), Encoding.UTF8, "application/json");

        var response = await context.Client.PostAsync($"{baseUrl}", postBody);
        if (response.IsSuccessStatusCode)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync();
            return Response.Ok(payload: bytes, statusCode: (int)response.StatusCode);
        }
        else
        {
            if (context.Logger != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                context.Logger.Error(responseContent);
            }
            return Response.Fail(statusCode: (int)response.StatusCode, error: response.ReasonPhrase);
        }
    });

    // create scenario
    var scenario = ScenarioBuilder.CreateScenario("Create Product", step)
        .WithWarmUpDuration(warmupTime)
        .WithLoadSimulations(
            LoadSimulation.NewKeepConstant(_copies: paralellClients, _during: timeToRun)
        );

    return Task.FromResult(scenario);
}

IClientFactory<HttpClient> CreateClientFactory() => ClientFactory.Create(
        name: "http_factory",
        clientCount: paralellClients,
        initClient: (number, context) => Task.FromResult(new HttpClient()));

async Task<int[]> GetProductIds()
{
    using var client = new HttpClient();

    var pagesCount = await _GetPagesCount(client);

    var idsBag = new ConcurrentBag<int[]>();

    await Parallel.ForEachAsync(Enumerable.Range(1, pagesCount), async (pageNumber, ct) =>
    {
        var getPageResponse = await client.GetAsync($"{baseUrl}/list?page={pageNumber}&pageSize={pageSize}", ct);
        if (!getPageResponse.IsSuccessStatusCode)
            throw new ApplicationException("Unable to fetch products page");

        var productsList = JsonConvert.DeserializeObject<List<Product>>(await getPageResponse.Content.ReadAsStringAsync())
            ?? new List<Product>();
        idsBag.Add(productsList.Select(x => x.ProductId).ToArray());
    });

    return idsBag.SelectMany(x => x.Select(y => y)).ToArray();
}

async Task<int> GetPagesCount()
{
    using var client = new HttpClient();

    return await _GetPagesCount(client);
}

async Task<int> _GetPagesCount(HttpClient client)
{
    var totalCountResponse = await client.GetAsync($"{baseUrl}/list/total");
    if (!totalCountResponse.IsSuccessStatusCode)
        throw new ApplicationException("Failed to fetched total amount of products");

    var totalCount = JsonConvert.DeserializeObject<int>(await totalCountResponse.Content.ReadAsStringAsync());

    var pagesCount = (totalCount / pageSize) + 1;
    return pagesCount;
}
