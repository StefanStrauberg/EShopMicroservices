var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var Assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssembly(Assembly);
  config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddCarter();
builder.Services.AddMarten(opts =>
{
  opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
builder.Services.AddValidatorsFromAssembly(Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(options =>
{

});
app.MapCarter();

app.Run();
