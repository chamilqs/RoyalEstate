using RoyalState.Infrastructure.Shared;
using RoyalState.Infrastructure.Identity;
using RoyalState.Infrastructure.Persistence;
using RoyalState.Core.Application;
using RoyalState.Presentation.WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddSession();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructureForWeb(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddScoped<LoginAuthorize>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();

var app = builder.Build();

// Create default roles and users
await app.Services.AddIdentitySeeds();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
