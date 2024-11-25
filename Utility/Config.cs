using System.Configuration;

namespace ManageOrders.Utility
{
    /// <summary>
    /// Конфиг для программы
    /// </summary>
    public static class Config
    {
        public static readonly string apiUrl;

        static Config()
        {
            apiUrl = ConfigurationManager.ConnectionStrings["BaseUrl"].ConnectionString;
        }
    }
}
