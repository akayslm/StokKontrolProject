using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StokKontrolProject.Repositories.Abstract;
using StokKontrolProject.Repositories.Concrete;
using StokKontrolProject.Repositories.Context;
using StokKontrolProject.Service.Abstract;
using StokKontrolProject.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(
    option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StokKontrolContext>(option =>
{
    option.UseSqlServer("Server=DESKTOP-P0H9JIH; Database=HS6StokKontrolDB; uid=sa; pwd=orti1903");
});

builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericManager<>));
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
