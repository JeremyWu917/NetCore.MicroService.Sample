using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.MessageDto;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICapPublisher _capBus;
        private readonly ProductContext _context;

        public ProductsController(ILogger<ProductsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string result = $"【产品服务】{DateTime.Now:yyyy-MM-dd HH:mm:ss}——" +
                $"{Request.HttpContext.Connection.LocalIpAddress}:{_configuration["ConsulSetting:ServicePort"]}";

            return Ok(result);
        }

        /// <summary>
        /// 减库存 订阅下单事件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe("order.services.createorder")]
        public async Task ReduceStock(CreateOrderMessageDto message)
        {
            //业务代码
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == message.ProductID);
            product.Stock -= message.Count;

            await _context.SaveChangesAsync();
        }

    }
}
