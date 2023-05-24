using Acl;
using Marten;
using MessageContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DataSubscribers>();

// Add services to the container.
var dataConnectionString = builder.Configuration.GetConnectionString("data") ?? throw new ArgumentNullException("Need a connection string");
var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka") ?? throw new ArgumentNullException("Need a kafka broker");
builder.Services.AddMarten(options =>
{
    options.Connection(dataConnectionString);
    options.Schema.For<ApplicantCreated>().Identity(a => a.UserId);
    // talk more about this later.
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;
    }
});



builder.Services.AddCap(options =>
{
    options.UseKafka(kafkaConnectionString); // message broker
    options.UsePostgreSql(dataConnectionString); // it uses an "outbox" pattern.
    options.UseDashboard(); // just for class, but I think it's cool. 
});
var app = builder.Build();
app.MapGet("/", () => "This is not an API");



app.Run();