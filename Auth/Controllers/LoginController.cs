using KiteConnect;
using Microsoft.AspNetCore.Mvc;

namespace TokenGenerator.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        [Route("")]
        public IActionResult Login()
        {
            Kite kc = new Kite("z5xy8uzid74zcvu5");
            ViewData["LoginURL"] = kc.GetLoginURL();

            return View();
        }

        [Route("success")]
        public string RequestToken([FromQuery] string request_token)
        {
            return request_token;
        }
    }
}