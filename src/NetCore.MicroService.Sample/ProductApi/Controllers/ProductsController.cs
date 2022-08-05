using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string result = $"[产品服务]-{DateTime.Now:yyyy-MM-dd HH:mm:ss}-{Request.HttpContext.Connection.LocalIpAddress}:{Request.HttpContext.Connection.LocalPort}";
            return Ok(result);
        }
    }
}
