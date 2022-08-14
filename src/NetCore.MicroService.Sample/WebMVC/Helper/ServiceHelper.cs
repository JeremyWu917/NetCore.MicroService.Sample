using Consul;
using RestSharp;
using System.Collections.Concurrent;

namespace WebMVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ConsulClient _consulClient;
        private ConcurrentBag<string> _orderServiceUrls;
        private ConcurrentBag<string> _productServiceUrls;

        public ServiceHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            });
        }

        public async Task<string> GetOrder(string accessToken)
        {
            //订单服务的地址，可以放在配置文件或者数据库等等...
            //string serviceUrl = "http://localhost:5001";

            //var Client = new RestClient(serviceUrl);


            //服务集群
            //string[] serviceUrls = { "http://localhost:5001", "http://localhost:5002", "http://localhost:5003", "http://localhost:5004", "http://localhost:5005" };
            //每次随机访问一个服务实例
            //var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);

            //var request = new RestRequest("/orders", Method.Get);

            //var consulClient = new ConsulClient(c =>
            //{
            //    //consul地址
            //    c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            //});
            ////健康的服务
            //var services = consulClient.Health.Service("OrderService", null, true, null).Result.Response;
            ////订单服务地址列表
            //string[] serviceUrls = services.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

            //if (!serviceUrls.Any())
            //{
            //    return await Task.FromResult("【订单服务】服务列表为空");
            //}

            //每次随机访问一个服务实例
            //var Client = new RestClient(serviceUrls[new Random().Next(0, serviceUrls.Length)]);

            if (_orderServiceUrls == null)
                return await Task.FromResult("【订单服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(_orderServiceUrls.ElementAt(new Random().Next(0, _orderServiceUrls.Count())));

            var request = new RestRequest("/orders", Method.Get);

            var response = await Client.ExecuteAsync(request);

            return response.Content;
        }

        public async Task<string> GetProduct(string accessToken)
        {
            //产品服务的地址，可以放在配置文件或者数据库等等...
            //string serviceUrl = "http://localhost:5000";

            //var Client = new RestClient(serviceUrl);

            //服务集群
            //string[] serviceUrls = { "http://localhost:5006", "http://localhost:5007", "http://localhost:5008", "http://localhost:5009", "http://localhost:5010" };
            //每次随机访问一个服务实例
            //var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);



            //var consulClient = new ConsulClient(c =>
            //{
            //    //consul地址
            //    c.Address = new Uri(_configuration["ConsulSetting:ConsulAddress"]);
            //});
            ////健康的服务
            //var services = consulClient.Health.Service("ProductService", null, true, null).Result.Response;
            ////产品服务地址列表
            //string[] serviceUrls = services.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

            //if (!serviceUrls.Any())
            //{
            //    return await Task.FromResult("【产品服务】服务列表为空");
            //}

            ////每次随机访问一个服务实例
            //var Client = new RestClient(serviceUrls[new Random().Next(0, serviceUrls.Length)]);


            if (_productServiceUrls == null)
                return await Task.FromResult("【产品服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(_productServiceUrls.ElementAt(new Random().Next(0, _productServiceUrls.Count())));

            var request = new RestRequest("/products", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        /// <summary>
        /// 获取微服务列表
        /// </summary>
        public void GetServices()
        {
            var serviceNames = new string[] { "OrderService", "ProductService" };
            Array.ForEach(serviceNames, p =>
            {
                Task.Run(() =>
                {
                    //WaitTime默认为5分钟
                    var queryOptions = new QueryOptions { WaitTime = TimeSpan.FromMinutes(10) };
                    while (true)
                    {
                        GetServices(queryOptions, p);
                    }
                });
            });
        }

        /// <summary>
        /// 版本号变化时获取服务列表
        /// </summary>
        /// <param name="queryOptions"></param>
        /// <param name="serviceName">服务名称</param>
        private void GetServices(QueryOptions queryOptions, string serviceName)
        {
            var res = _consulClient.Health.Service(serviceName, null, true, queryOptions).Result;

            //控制台打印一下获取服务列表的响应时间等信息
            Console.WriteLine($"{DateTime.Now}获取{serviceName}：queryOptions.WaitIndex：{queryOptions.WaitIndex}  LastIndex：{res.LastIndex}");

            //版本号不一致 说明服务列表发生了变化
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                queryOptions.WaitIndex = res.LastIndex;

                //服务地址列表
                var serviceUrls = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();

                if (serviceName == "OrderService")
                    _orderServiceUrls = new ConcurrentBag<string>(serviceUrls);
                else if (serviceName == "ProductService")
                    _productServiceUrls = new ConcurrentBag<string>(serviceUrls);
            }
        }
    }

}
