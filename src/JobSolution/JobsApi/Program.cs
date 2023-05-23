using JobsApi.Controllers;
using Marten;
using SlugGenerators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICheckForUniqueValues, UniqueIdChecker>();
builder.Services.AddScoped<SlugGenerator>();
builder.Services.AddScoped<JobManager>();

var dataConnectionString = builder.Configuration.GetConnectionString("data") ?? throw new ArgumentNullException("AHHHHHHHHHHH");
var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka") ?? throw new ArgumentNullException("AHHHHHHHHHHH");

builder.Services.AddMarten(options => // this gives us the IDocumentSession
{
    options.Connection(dataConnectionString);
    if(builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
    }
});

builder.Services.AddCap(options =>
{
    options.UseKafka(kafkaConnectionString);
    options.UsePostgreSql(dataConnectionString); // "outbox" pattern
    options.UseDashboard();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
