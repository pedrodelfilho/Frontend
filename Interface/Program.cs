using Interface.Services;
using Interface.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();


// Configura��o de autentica��o com cookies
builder.Services.AddAuthentication("Interface")
    .AddCookie("Interface", options =>
    {
        options.LoginPath = "/Autenticacao/Login";
        options.AccessDeniedPath = "/Autenticacao/AccessDenied";
        options.LogoutPath = "/Autenticacao/Logout";
        options.ReturnUrlParameter = "a";
    });

// Registrando os servi�os
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IExameService, ExameService>();
builder.Services.AddScoped<IAdministracaoService, AdministracaoService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();


var app = builder.Build();

// Configura��o do pipeline de requisi��o HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseHsts(); 
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Certifique-se de que a sess�o seja utilizada corretamente

// Configura��o de roteamento
app.UseRouting();

// Autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento das rotas padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autenticacao}/{action=Login}/{id?}");

// Inicia o aplicativo
app.Run();
