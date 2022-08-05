using RestSharp;

namespace WebMVC.Helper
{
    public class ServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            //订单服务的地址，可以放在配置文件或者数据库等等...
            string serviceUrl = "http://172.29.12.155:5001";

            var Client = new RestClient(serviceUrl);
            var request = new RestRequest("/orders", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            //产品服务的地址，可以放在配置文件或者数据库等等...
            string serviceUrl = "http://172.29.12.155:5000";

            var Client = new RestClient(serviceUrl);
            var request = new RestRequest("/products", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }
    }

}
