using BookComparerAPI.Scraping;
using BookComparerAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Scraper.GetAmazonBook(); //WARNING: May not be the best way to do this; TODO: Async task
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<BookDataContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString(""))
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
