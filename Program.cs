
var builder = WebApplication.CreateBuilder(args);

// CORS for cross resource sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");  
app.UseAuthorization();
app.MapControllers();
app.Run();
