using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BornOtomasyonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    {
        // Bu endpoint'e sadece JWT token ile erişilebilir
        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecretData()
        {
            return Ok("Bu veri sadece token ile erişilebilir!");
        }
    }
}
