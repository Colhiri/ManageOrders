using ManageOrders.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
                BaseAddress = new Uri(Config.apiUrl),
            };
        }

        /// <summary>
        /// Получить все заявки
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderModel>> GetOrdersAsync()
        {
            return await httpClient.GetFromJsonAsync<List<OrderModel>>("api/orders");
        }

        /// <summary>
        /// Создать заявку
        /// </summary>
        /// <param name="order">Новая заявка</param>
        /// <returns></returns>
        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            var response = await httpClient.PostAsJsonAsync("api/orders", order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderModel>();
        }

        /// <summary>
        /// Обновить статус заявки на "Передано на исполнение"
        /// </summary>
        /// <param name="order">Заявка</param>
        /// <returns></returns>
        public async Task UpdateStateOrderAsync(OrderModel order)
        {
            var response = await httpClient.PutAsJsonAsync($"api/orders/{order.IdOrder}/status", order);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Редактировать заявку
        /// </summary>
        /// <param name="order">Заявка</param>
        /// <returns></returns>
        public async Task EditOrderAsync(OrderModel order)
        {
            var response = await httpClient.PutAsJsonAsync($"api/orders/{order.IdOrder}", order);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Удалить заявку
        /// </summary>
        /// <param name="idOrder">Идентификатор заявки</param>
        /// <returns></returns>
        public async Task DeleteOrderAsync(int idOrder)
        {
            var response = await httpClient.DeleteAsync($"api/orders/{idOrder}");
            response.EnsureSuccessStatusCode();
        }
    }
}
