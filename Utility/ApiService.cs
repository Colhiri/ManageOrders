using ManageOrders.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManageOrders.Utility
{
    public class ApiService
    {
        private readonly HttpClient httpClient;

        public ApiService() 
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            httpClient = new HttpClient(handler)
             {
                 BaseAddress = new Uri("https://localhost:7124"),

             };
        }

        public async Task<List<OrderModel>> GetOrders()
        {
            HttpResponseMessage response = await httpClient.GetAsync("api/orders");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OrderModel>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        // public async Task CreateOrder(OrderModel order)
        // {
        //     
        // }
        // 
        // public Task<OrderModel> EditOrder(OrderModel order)
        // {
        //     
        // }
        // 
        // public Task DeleteOrder(string idOrder)
        // {
        // 
        // }



    }
}
