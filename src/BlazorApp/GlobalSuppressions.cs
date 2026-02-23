using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "SonarAnalyzer CSharp",
    "S1939:Remove redundant interface implementation",
    Justification = "Required when deriving from EF Identity base classes with custom key types (ObjectId)",
    Scope = "file",
    Target = "~F:Devpro.TodoList.BlazorApp.Identity.RoleStore.cs")]

[assembly: SuppressMessage(
    "SonarAnalyzer CSharp",
    "S2436:Reduce the number of generic parameters",
    Justification = "Unavoidable due to ASP.NET Core Identity EF Core base class design",
    Scope = "file",
    Target = "~F:Devpro.TodoList.BlazorApp.Identity.RoleStore.cs")]

[assembly: SuppressMessage(
    "SonarAnalyzer CSharp",
    "S2436:Reduce the number of generic parameters",
    Justification = "Unavoidable due to ASP.NET Core Identity EF Core base class design",
    Scope = "file",
    Target = "~F:Devpro.TodoList.BlazorApp.Identity.UserStore.cs")]
