using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers;

public class HomeController : Controller
{
    private static readonly List<Employee> __Employees = new()
    {
        new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 23 },
        new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 27 },
        new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
    };

    private readonly IConfiguration _Configuration;

    public HomeController(IConfiguration Configuration) { _Configuration = Configuration; }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ContentString(string Id = "-id-")
    {
        return Content($"content: {Id}");
    }

    public IActionResult ConfigStr()
    {
        return Content($"config: {_Configuration["ServerGreetings"]}");
    }

    public IActionResult Employees()
    {
        return View(__Employees);
    }
}