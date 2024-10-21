using Microsoft.AspNetCore.Mvc;

namespace EasyShopping.Core.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Home : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get() =>
            Ok("Welcom to EasyShopping");
    }
}
