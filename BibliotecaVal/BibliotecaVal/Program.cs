using BibliotecaAPI.Data;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<LivroRepository>();
builder.Services.AddScoped<EmprestimoRepository>();

builder.Services.AddScoped<LivroService>();
builder.Services.AddScoped<EmprestimoService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();