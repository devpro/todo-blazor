# Todo Blazor (Web Application)

A simple todo list that runs as a web application (written with Blazor / C# / .NET).

For development details, see [CONTRIBUTING.md](CONTRIBUTING.md) - contributions are more than welcome 🙂

## Backlog

- [x] Auth with users in MongoDB
- [x] Todo list page
- [x] Dockerfile
- [x] Docker compose
- [x] Helm chart
- [ ] CI/CD
- [ ] Secret check (GitGuardian)
- [ ] Unit tests (.NET)
- [ ] Integration/e2e tests (Playwright)
- [ ] Health check (with db check)
- [ ] Sonar
- [ ] Code indentation review (in particular js/css files)

## Limitations

NuGet packages:

- Keep version 9 of ASP.NET EF (Entity Framework) for now, as version 10 introduces breaking changes for MongoDB EF Provider 9

## References

.NET documentation:

- [Introduction to Identity on ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [ASP.NET Core Blazor authentication and authorization](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/)

Thanks to Miiro Ashirafu for his example on Blazor user identity with MongoDB: [ashirafumiiro/MongoDBEFCoreWeatherApp](https://github.com/ashirafumiiro/MongoDBEFCoreWeatherApp).

Blazor samples ([Blazor movie database app](https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/movie-database-app/)):

```bash
git clone https://github.com/dotnet/blazor-samples.git
cd blazor-samples\10.0\BlazorWebAppMovies
dotnet tool restore
dotnet ef database update
dotnet run
```
