using BibliotecaAPI.Data;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte aos Controllers da API
builder.Services.AddControllers();

// Configura a conexão com o banco de dados usando o SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Registra os Repositories para poderem ser usados pelos Services
builder.Services.AddScoped<LivroRepository>();
builder.Services.AddScoped<EmprestimoRepository>();

// Registra os Services que possuem as regras da aplicação
builder.Services.AddScoped<LivroService>();
builder.Services.AddScoped<EmprestimoService>();

var app = builder.Build();

// Faz a API usar HTTPS
app.UseHttpsRedirection();

// Libera as rotas dos Controllers
app.MapControllers();

// Inicia a aplicação
app.Run();
