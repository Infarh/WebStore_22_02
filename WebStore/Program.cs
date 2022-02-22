var builder = WebApplication.CreateBuilder(args);


// Регистрация сервисов

var app = builder.Build();

app.MapGet("/", () => app.Configuration["ServerGreetings"]);

app.Run();
