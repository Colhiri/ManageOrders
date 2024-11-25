using ManageOrders.Models;
using ManageOrders.Utility;
using System;
using System.Text;
using System.Windows;

namespace ManageOrders.ViewModels
{
    /// <summary>
    /// Передать на выполнение
    /// </summary>
    public class ExecuteStateOrderVM : BaseOrderVM
    {
        public ExecuteStateOrderVM(OrderModel order, ApiService apiService) : base(order, apiService)
        {
        }

        public override void SetParameters() => Title = "Передать заявку на выполнение";

        public override void ManageControlsOrder()
        {
            enabledNameClient = false;
            enabledPickupAddress = false;
            enabledDeliveryAddress = false;
            enabledPickupTime = false;
            enabledStatus = false;
            enabledCancelReason = false;
        }

        public override async void ActionOrder()
        {
            CurrentOrder.Status = "Передано на выполнение";

            if (!CheckRunAction(out string msg))
            {
                MessageBox.Show(msg);
                return;
            }

            try
            {
                await apiService.UpdateStateOrderAsync(CurrentOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сервера. Обновление заявок: {ex.Message}");
                return;
            }

            ActionComplete = true;
            ClosingWindow?.Invoke();
        }

        public override bool CheckRun()
        {
            return CurrentOrder?.Status == "Новая";
        }

        protected override bool CheckRunAction(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            bool check = true;
            
            if (!CurrentOrder.CheckStatus())
            {
                check = false;
                sb.AppendLine("Поле <Статус> содержит неизвестный статус.");
            }
            
            msg = sb.ToString();
            return check;
        }
    }
}
