# Todo Blazor (Web Application)

Todo list web application written in Blazor / C# / .NET.

## Backlog

- [x] Auth with users in MongoDB
- [ ] CI/CD
- [ ] Dockerfile
- [ ] Docker compose
- [ ] Helm chart
- [ ] Unit tests (.NET)
- [ ] Integration/e2e tests (Playwright)
- [ ] Sonar
- [ ] GitGuardian
- [ ] Code indentation review (in particular js/css files)

## Limitations

NuGet packages:

- Keep version 9 of ASP.NET EF (Entity Framework) for now, as version 10 introduces breaking changes for MongoDB EF Provider 9

## References

.NET documentation:

- [Introduction to Identity on ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [ASP.NET Core Blazor authentication and authorization](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/)

Thanks to Miiro Ashirafu for his example on Blazor user identity with MongoDB: [ashirafumiiro/MongoDBEFCoreWeatherApp](https://github.com/ashirafumiiro/MongoDBEFCoreWeatherApp).

Blazor samples:

```bash
git clone https://github.com/dotnet/blazor-samples.git
cd blazor-samples\10.0\BlazorWebAppMovies
dotnet tool restore
dotnet ef database update
dotnet run
```
