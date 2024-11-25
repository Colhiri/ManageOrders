using ManageOrders.Utility;
using System.Text;
using System.Windows;
using ManageOrders.Models;
using System;

namespace ManageOrders.ViewModels
{
    /// <summary>
    /// Создание заявки
    /// </summary>
    public class CreateNewOrderVM : BaseOrderVM
    {
        public CreateNewOrderVM(OrderModel order, ApiService apiService) : base(order, apiService)
        {
        }

        public override void SetParameters() => Title = "Создать заявку";

        public override void ManageControlsOrder()
        {
            enabledStatus = false;
            enabledCancelReason = false;
        }

        public override async void ActionOrder()
        {
            if (!CheckRunAction(out string msg))
            {
                MessageBox.Show(msg);
                return;
            }

            try
            {
                OrderModel serverModel = await apiService.CreateOrderAsync(CurrentOrder);
                CurrentOrder.IdOrder = serverModel.IdOrder;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сервера. Создание заявки: {ex.Message}");
                return;
            }

            ActionComplete = true;
            ClosingWindow?.Invoke();
        }

        public override bool CheckRun()
        {
            return CurrentOrder != null;
        }

        protected override bool CheckRunAction(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            bool check = true;
            if (!CurrentOrder.CheckNameClient())
            {
                check = false;
                sb.AppendLine("Поле <Имя клиента> должно быть заполнено!");
            }
            if (!CurrentOrder.CheckNameExecutor())
            {
                check = false;
                sb.AppendLine("Поле <Имя исполнителя> должно быть заполнено!");
            }
            if (!CurrentOrder.CheckPickupAddress())
            {
                check = false;
                sb.AppendLine("Поле <Адрес клиента> должно быть заполнено!");
            }
            if (!CurrentOrder.CheckDeliveryAddress())
            {
                check = false;
                sb.AppendLine("Поле <Адрес доставки> должно быть заполнено!");
            }
            if (!CurrentOrder.CheckPickupTime())
            {
                check = false;
                sb.AppendLine("Поле <Время передачи посылки> не может быть заполнено задним числом!");
            }
            msg = sb.ToString();
            return check;
        }
    }
}
