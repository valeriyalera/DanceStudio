// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;


// namespace DanceStudio.Infrastructure.Controllers;

// [Authorize(Roles = "Admin")]
// public class AdminController : Controller
// {
//     public IActionResult Index()
//     {
//         return View();
//     }
// }


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudio.Infrastructure.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return Content("Ви увійшли як АДМІНІСТРАТОР! Доступні всі функції.");
    }
}
