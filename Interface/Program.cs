using Interface.Services;
using Interface.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();


// Configuração de autenticação com cookies
builder.Services.AddAuthentication("Interface")
    .AddCookie("Interface", options =>
    {
        options.LoginPath = "/Autenticacao/Login";
        options.AccessDeniedPath = "/Autenticacao/AccessDenied";
        options.LogoutPath = "/Autenticacao/Logout";
        options.ReturnUrlParameter = "a";
    });

// Registrando os serviços
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IExameService, ExameService>();
builder.Services.AddScoped<IAdministracaoService, AdministracaoService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();


var app = builder.Build();

// Configuração do pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseHsts(); 
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
    pattern: "{controller=Autenticacao}/{action=Login}/{id?}");

// Inicia o aplicativo
app.Run();
