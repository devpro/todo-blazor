using Devpro.TodoList.BlazorApp.Components;
using Devpro.TodoList.BlazorApp.Components.Account;
using Devpro.TodoList.BlazorApp.Configuration;
using Devpro.TodoList.BlazorApp.Identity;
using Devpro.TodoList.BlazorApp.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var databaseSettings = new DatabaseSettings
{
    ConnectionString = builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                       ?? throw new InvalidOperationException("Connection string must be defined in configuration"),
    DatabaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName")
                   ?? throw new InvalidOperationException("Database name must be defined in configuration")
};

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options
        .UseMongoDB(databaseSettings.ConnectionString, databaseSettings.DatabaseName)
        .EnableSensitiveDataLogging());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
builder.Services.AddTransient<IRoleStore<IdentityRole<ObjectId>>, RoleStore<IdentityRole<ObjectId>>>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var pack = new ConventionPack
    {
        new CamelCaseElementNameConvention(),
        new EnumRepresentationConvention(BsonType.String),
        new IgnoreExtraElementsConvention(true),
        new IgnoreIfNullConvention(true)
    };
    ConventionRegistry.Register("Conventions", pack, t => true);
    return new MongoClient(databaseSettings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp
    => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseSettings.DatabaseName));

builder.Services.AddScoped<TodoItemRepository>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(
        name: "mongodb-ef",
        customTestQuery: (context, token) => context.Database.CanConnectAsync(token),
        tags: ["db", "mongodb"]
    );

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
if (builder.Configuration.GetValue<bool>("Features:IsHttpsRedirectionEnabled"))
{
    app.UseHttpsRedirection();
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
