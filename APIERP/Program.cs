using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using APIERP;
using APIERP.Endpoints;
using APIERP.Entidades;
using APIERP.Repositorios;
using APIERP.Servicios;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenespermitidos")!;
// Inicio de área de los servicios

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", configuracion =>
    {
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepoCategories, RepoCategories>();
builder.Services.AddScoped<IRepoStores, RepoStores>();
builder.Services.AddScoped<IRepoProducts, RepoProducts>();
builder.Services.AddScoped<IRepoOrders, RepoOrders>();
builder.Services.AddScoped<IRepoOrderDetails, RepoOrderDetails>();
builder.Services.AddScoped<IRepoRainchecks, RepoRainchecks>();

builder.Services.AddScoped(typeof(IRepo<>), typeof(Repo<>)); // Generics T
builder.Services.AddScoped<IRepoErrores, RepoErrores>();

builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

// Fin de área de los servicios

var app = builder.Build();
// Inicio de área de los middleware

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    var exceptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
    var excepcion = exceptionHandleFeature?.Error!;

    var error = new Error
    {
        Fecha = DateTime.UtcNow,
        MensajeDeError = excepcion.Message,
        StackTrace = excepcion.StackTrace
    };

    var repositorio = context.RequestServices.GetRequiredService<IRepoErrores>();
    await repositorio.Crear(error);

    await TypedResults.BadRequest(
        new { tipo = "Error", mensaje = "Ha ocurrido un error inesperado", estado = 500 })
    .ExecuteAsync(context);
}));

app.UseStatusCodePages();

app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.MapGet("/", [EnableCors(policyName: "libre")] () => "¡ERP API Funciona!");
app.MapGet("/error", () =>
{
    throw new InvalidOperationException("Error de prueba. Probar el guardado de errores en API ERP");
});

app.MapGroup("/generate-data").AddDataToDB();

app.MapGroup("/categories").MapCategories();
app.MapGroup("/stores").MapStores();
app.MapGroup("/products").MapProducts();
app.MapGroup("/orders").MapOrders();
app.MapGroup("/orderdetails").MapOrderDetails();
app.MapGroup("/rainchecks").MapRainChecks();

// Fin de área de los middleware
app.Run();
