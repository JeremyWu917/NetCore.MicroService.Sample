namespace WebMVC.Helper
{
    public interface IServiceHelper
    {
        /// <summary>
        /// 获取产品数据
        /// </summary>
        /// <returns></returns>
        Task<string> GetProduct();

        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <returns></returns>
        Task<string> GetOrder();
    }

}
