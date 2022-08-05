using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string result = $"[订单服务]-{DateTime.Now:yyyy-MM-dd HH:mm:ss}-{Request.HttpContext.Connection.LocalIpAddress}:{Request.HttpContext.Connection.LocalPort}";
            return Ok(result);
        }
    }
}
