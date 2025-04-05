using Interface.Services;
using Interface.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

// Configuração de autenticação com cookies
builder.Services.AddAuthentication("Interface")
    .AddCookie("Interface", options =>
    {
        // Redireciona para a página de login
        options.LoginPath = "/Autenticacao/Login";
        // Redireciona em caso de acesso não autorizado
        options.AccessDeniedPath = "/Autenticacao/AccessDenied";
        // Redireciona após logout
        options.LogoutPath = "/Autenticacao/Logout";
        // Parâmetro de URL para retorno após login
        options.ReturnUrlParameter = "a";
    });

// Registrando os serviços
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IExameService, ExameService>();

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS para ambientes de produção
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Certifique-se de que a sessão seja utilizada corretamente

// Configuração de roteamento
app.UseRouting();

// Autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento das rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia o aplicativo
app.Run();
