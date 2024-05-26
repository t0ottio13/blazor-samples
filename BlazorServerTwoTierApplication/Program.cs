using BlazorServerTwoTierApplication.Components;
using BlazorServerTwoTierApplication.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<PubsDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options = options.EnableSensitiveDataLogging().EnableDetailedErrors();
    }
    options.UseSqlServer(builder.Configuration.GetConnectionString("PubsDBContext"),
        providerOption =>
        {
            providerOption.EnableRetryOnFailure();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
