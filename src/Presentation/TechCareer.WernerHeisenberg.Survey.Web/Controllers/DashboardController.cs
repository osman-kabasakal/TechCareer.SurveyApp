using Microsoft.AspNetCore.Mvc;

namespace TechCareer.WernerHeisenberg.Survey.Web.Areas.Management.Controllers;

public class DashboardController : Controller
{
    public DashboardController()
    {
        
    }
    public IActionResult Index()
    {
        return View();
    }
}