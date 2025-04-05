using Interface.Services;
using Interface.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

// Configura��o de autentica��o com cookies
builder.Services.AddAuthentication("Interface")
    .AddCookie("Interface", options =>
    {
        // Redireciona para a p�gina de login
        options.LoginPath = "/Autenticacao/Login";
        // Redireciona em caso de acesso n�o autorizado
        options.AccessDeniedPath = "/Autenticacao/AccessDenied";
        // Redireciona ap�s logout
        options.LogoutPath = "/Autenticacao/Logout";
        // Par�metro de URL para retorno ap�s login
        options.ReturnUrlParameter = "a";
    });

// Registrando os servi�os
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IExameService, ExameService>();

var app = builder.Build();

// Configura��o do pipeline de requisi��o HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS para ambientes de produ��o
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia o aplicativo
app.Run();
