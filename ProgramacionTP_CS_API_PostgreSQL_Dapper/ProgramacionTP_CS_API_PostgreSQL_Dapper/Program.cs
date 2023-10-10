using ProgramacionTP_CS_API_PostgreSQL_Dapper.DbContexts;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Interfaces;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Repositories;
using ProgramacionTB_CS_API_PostgreSQL_Dapper.Services;
using ProgramacionTP_CS_API_PostgreSQL_Dapper.Services;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

//El DBContext a utilizar
builder.Services.AddSingleton<PgsqlDbContext>();

//Los repositorios
builder.Services.AddScoped<IAutobusRepository, AutobusRepository>();
builder.Services.AddScoped<ICargadorRepository, CargadorRepository>();
builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
builder.Services.AddScoped<IOperacionAutobusRepository, OperacionAutobusRepository>();
builder.Services.AddScoped<IUtilizacionCargadorRepository, UtilizacionCargadorRepository>();
builder.Services.AddScoped<IInformeRepository, InformeRepository>();

//Aqui agregamos los servicios asociados para cada EndPoint
builder.Services.AddScoped<AutobusService>();
builder.Services.AddScoped<CargadorService>();
builder.Services.AddScoped<HorarioService>();
builder.Services.AddScoped<OperacionAutobusService>();
builder.Services.AddScoped<UtilizacionCargadorService>();
builder.Services.AddScoped<InformeService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
