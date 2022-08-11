using RestSharp;

namespace WebMVC.Helper
{
    /// <summary>
    /// 通过gateway调用服务
    /// </summary>
    public class GatewayServiceHelper : IServiceHelper
    {
        public async Task<string> GetOrder()
        {
            var Client = new RestClient("http://localhost:5000");
            var request = new RestRequest("/orders", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetProduct()
        {
            var Client = new RestClient("http://localhost:5000");
            var request = new RestRequest("/products", Method.Get);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }

}
