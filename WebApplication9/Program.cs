using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using WebApplication9;
using WebApplication9.DbContexts;
using WebApplication9.Models;
using WebApplication9.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//.Logging.AddConsole();

builder.Host.UseSerilog();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Add services to the container.

builder.Services.AddControllers(options =>
{
   
    options.ReturnHttpNotAcceptable=true;   
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
	var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

	setupAction.IncludeXmlComments(xmlCommentsFullPath);
	setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme()
	{
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		Description = "Input a valid token to access this API"
	});

	setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference {
					Type = ReferenceType.SecurityScheme,
					Id = "CityInfoApiBearerAuth" }
			}, new List<string>() }
	});
});

	builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
	builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

	builder.Services.AddSingleton<CitiesDataStore>();

	builder.Services.AddDbContext<WebApplication9Context>(DbContextOptions => DbContextOptions.UseSqlite(
		builder.Configuration["ConnectionStrings:WebApplication9DBConnectionString"]));

	builder.Services.AddScoped<IWebApplication9Repository, WebApplication9Repository>();

	builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

	builder.Services.AddAuthentication("Bearer")
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new()
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = builder.Configuration["Authentication:Issuer"],
				ValidAudience = builder.Configuration["Authentication:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(
					   Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
			};
		}
		);
	builder.Services.AddAuthorization(options =>
	{
		options.AddPolicy("MustBeFromAntwerp", policy =>
		{
			policy.RequireAuthenticatedUser();
			policy.RequireClaim("city", "Antwerp");
		});
	});

	builder.Services.AddApiVersioning(setupAction =>
	{
		setupAction.AssumeDefaultVersionWhenUnspecified = true;
		setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
		setupAction.ReportApiVersions = true;
	});

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseRouting();

	app.UseAuthentication();

	app.UseAuthorization();

	app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllers();
	});


	app.Run();

