using Microsoft.AspNetCore.Mvc;

namespace WebInterface.Controllers
{
    public class LogController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "logs",
                "execution_web.log");
            string[] logs = System.IO.File.ReadAllLines(path);
            Array.Reverse(logs);

            return View(logs);
        }
    }
}
