# Run benchmarks. Results are stored in 'BenchmarkDotNet.Artifacts/results' folder
# To choose specific benchmark to run, use --filter appropriately, for example
# dotnet run -c Release --filter *Read*         to run all benchmarks with "Read" in full method path
# dotnet run -c Release --filter *Sequential*         to run all benchmarks with "Sequential" in full method path

# NOTE: if script is not running, please use next command in current Powershell session
# Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

dotnet run -c Release `
    --framework net6.0 `
    --filter * 