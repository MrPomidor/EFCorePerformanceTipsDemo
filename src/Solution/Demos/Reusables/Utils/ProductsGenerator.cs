using System;
using Bogus;
using Reusables.Models;

namespace Reusables.Utils
{
    public class ProductsGenerator
    {
        public static ProductsGenerator Instance = new ProductsGenerator();

        private Bogus.DataSets.Commerce _commerceDataset;
        private Faker<AddProductModel> _faker;

        private ProductsGenerator()
        {
            _commerceDataset = new Bogus.DataSets.Commerce();
            _faker = new Faker<AddProductModel>()
                .StrictMode(false)
                .RuleFor(x => x.Name, x => GenerateProductName())
                .RuleFor(x => x.ProductNumber, x => Guid.NewGuid().ToString().Substring(25))
                .RuleFor(x => x.SafetyStockLevel, x => x.Random.Short(1, 10_000))
                .RuleFor(x => x.ReorderPoint, (x, i) => x.Random.Short(1, Math.Max((short)(i.SafetyStockLevel / 2), (short)1)))
                .RuleFor(x => x.StandartCost, x => x.Random.Decimal(1, 100_000))
                .RuleFor(x => x.ListPrice, x => x.Random.Decimal(1, 100_000))
                .RuleFor(x => x.DaysToManifacture, x => x.Random.Int(0, 30))
                .RuleFor(x => x.Class, x => x.PickRandom("L", "M", "H", null))
                .RuleFor(x => x.Style, x => x.PickRandom("W", "M", "U"))
                .RuleFor(x => x.SellStartDate, x => x.Date.Future(2));
        }

        public string GenerateProductName() =>
            $"{_commerceDataset.ProductName().PadLeft(35, ' ').Substring(0, 35)}-{Guid.NewGuid().ToString().Substring(0, 10)}";

        public AddProductModel GenerateProduct() =>
            _faker.Generate();
    }
}
