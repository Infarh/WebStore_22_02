using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Areas.Admin.Controllers;

//[Area("Admin")]
[Authorize(Roles = Role.Adinistrators)]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
