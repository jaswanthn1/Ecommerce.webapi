using Ecommerce.EFCore;
using Ecommerce.Repositories.Repositories.Authenticetion;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddUtilityConfiguration(builder.Configuration);

builder.Services.AddDbContext<EcommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceDBContext"), s => s.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

//Dependency Injection
builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();

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
