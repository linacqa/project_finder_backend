# project_finder_backend
Bachelor final thesis project backend.

```bash
dotnet ef migrations add --project DAL.EF --startup-project WebApp --context AppDbContext InitialCreate

dotnet ef migrations --project DAL.EF --startup-project WebApp remove

dotnet ef database --project DAL.EF --startup-project WebApp update
dotnet ef database --project DAL.EF --startup-project WebApp drop
```

Api controllers
```bash
cd WebApp

dotnet aspnet-codegenerator controller -name ApplicationsController -m Domain.Application -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ProjectsController -m Domain.Project -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
```
