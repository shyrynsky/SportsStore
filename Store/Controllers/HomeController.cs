using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}