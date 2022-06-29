# To bypass execution policy restrictions use next command:
# Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

. dotnet tool install --global dotnet-ef

. dotnet ef dbcontext scaffold `
    "data source=localhost;initial catalog=AdventureWorks;persist security info=True;user id=sa;password=Hello_world_777;MultipleActiveResultSets=True;App=EntityFramework" `
    Microsoft.EntityFrameworkCore.SqlServer `
    --context-dir Storage --output-dir Storage\Models `
    --namespace Reusables.Storage --context-namespace Reusables.Storage.Models