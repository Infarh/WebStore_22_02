var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
