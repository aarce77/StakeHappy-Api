using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace StakHappy.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    public class TestController : Controller
    {
        [HttpGet("GetMessage")]
        public IActionResult GetMessage()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "user_id");
            return Json(new
            {
                userId = claim.Value
            });
        }
    }
}