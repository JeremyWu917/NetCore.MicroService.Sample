using RestSharp;

namespace WebMVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            //订单服务的地址，可以放在配置文件或者数据库等等...
            //string serviceUrl = "http://172.29.12.155:5001";

            //var Client = new RestClient(serviceUrl);


            //服务集群
            string[] serviceUrls = { "http://172.29.12.155:5101", "http://172.29.12.155:5102", "http://172.29.12.155:5103" };
            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);

            var request = new RestRequest("/orders", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            //产品服务的地址，可以放在配置文件或者数据库等等...
            //string serviceUrl = "http://172.29.12.155:5000";

            //var Client = new RestClient(serviceUrl);

            //服务集群
            string[] serviceUrls = { "http://172.29.12.155:5001", "http://172.29.12.155:5002", "http://172.29.12.155:5003" };
            //每次随机访问一个服务实例
            var Client = new RestClient(serviceUrls[new Random().Next(0, 3)]);

            var request = new RestRequest("/products", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }

}
