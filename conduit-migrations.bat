rmdir .\src\Conduit.Infrastructure\Migrations\ /S /Q

dotnet ef database drop --force --project .\src\Conduit.Infrastructure\ --startup-project .\src\Conduit.Api\ --context ApplicationDbContext

dotnet ef migrations add CreateDatabase --project .\src\Conduit.Infrastructure\ --startup-project .\src\Conduit.Api\ --context ApplicationDbContext

dotnet ef database update --project .\src\Conduit.Infrastructure\ --startup-project .\src\Conduit.Api\ --context ApplicationDbContext
