using Microsoft.AspNetCore.Mvc;

namespace api
{
    [Route("/")]
    public class Controller1 : Controller
    {
        // GET
        public IActionResult Get()
        {
            return Ok("Hello from 1");
        }
    }
}