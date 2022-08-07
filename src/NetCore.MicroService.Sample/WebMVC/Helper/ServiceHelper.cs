using Consul;
using RestSharp;

namespace WebMVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetOrder()
        {
            //订单服务的地址，可以放在配置文件或者数据库等等...
            //string serviceUrl = "http://localhost:5001";

            //var Client = new RestClient(serviceUrl);


            //服务集群
            //string[] serviceUrls = { "http://localhost:5001", "http://localhost:5002", "http://localhost:5003", "http://localhost:5004", "http://localhost:5005" };
            //每次随机访问一个服务实例
            //var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);

            //var request = new RestRequest("/orders", Method.Get);

            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });
            //健康的服务
            var services = consulClient.Health.Service("OrderService", null, true, null).Result.Response;
            //订单服务地址列表
            string[] serviceUrls = services.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

            if (!serviceUrls.Any())
            {
                return await Task.FromResult("【订单服务】服务列表为空");
            }

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, serviceUrls.Length)]);
            var request = new RestRequest("/orders", Method.Get);

            var response = await Client.ExecuteAsync(request);

            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            //产品服务的地址，可以放在配置文件或者数据库等等...
            //string serviceUrl = "http://localhost:5000";

            //var Client = new RestClient(serviceUrl);

            //服务集群
            //string[] serviceUrls = { "http://localhost:5006", "http://localhost:5007", "http://localhost:5008", "http://localhost:5009", "http://localhost:5010" };
            //每次随机访问一个服务实例
            //var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);



            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });
            //健康的服务
            var services = consulClient.Health.Service("ProductService", null, true, null).Result.Response;
            //产品服务地址列表
            string[] serviceUrls = services.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

            if (!serviceUrls.Any())
            {
                return await Task.FromResult("【产品服务】服务列表为空");
            }

            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, serviceUrls.Length)]);

            var request = new RestRequest("/products", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }

}
