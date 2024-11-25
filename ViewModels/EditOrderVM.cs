using ManageOrders.Models;
using ManageOrders.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;

namespace ManageOrders.ViewModels
{
    /// <summary>
    /// Редактирование заявки
    /// </summary>
    public class EditOrderVM : BaseOrderVM
    {
        public EditOrderVM(OrderModel order, ApiService apiService) : base(order, apiService)
        {
        }

        public override void SetParameters()
        {
            Title = "Редактировать заявку";
            if (CurrentOrder != null)
            {
                CurrentOrder.statusAction += OnStatusChange;
            }
        }

        public override void ManageControlsOrder()
        {
            if (CurrentOrder == null) { return; }

            bool isRightStatus = CurrentOrder.Status == "Новая";

            enabledNameClient = isRightStatus;
            enabledNameExecutor = isRightStatus;
            enabledPickupAddress = isRightStatus;
            enabledDeliveryAddress = isRightStatus;
            enabledPickupTime = isRightStatus;
            enabledStatus = true;
            enabledCancelReason = CurrentOrder.Status == "Отменена";

            List<string> temp = new List<string>();
            temp.AddRange(Utility.Utility.statusOrder);
            switch (CurrentOrder.Status)
            {
                case "Новая":
                    break;
                case "Передано на выполнение":
                    temp.Remove("Новая");
                    break;
                case "Выполнено":
                    temp.Remove("Новая");
                    temp.Remove("Передано на выполнение");
                    temp.Remove("Отменена");
                    break;
                case "Отменена":
                    temp.Remove("Новая");
                    temp.Remove("Передано на выполнение");
                    temp.Remove("Выполнено");
                    break;
                default:
                    throw new ArgumentException("Неизвестный тип заявки.");
            }

            StatusOrder = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(temp));
        }

        public override async void ActionOrder()
        {
            try
            {
                await apiService.EditOrderAsync(CurrentOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сервера. Редактирование заявки: {ex.Message}");
                return;
            }

            ActionComplete = true;
            ClosingWindow?.Invoke();
        }

        public override bool CheckRun()
        {
            return CurrentOrder != null;
        }

        private void OnStatusChange()
        {
            enabledCancelReason = CurrentOrder.Status == "Отменена";
            if (!enabledCancelReason)
            {
                CurrentOrder.CancelReason = String.Empty;
                OnPropertyChanged(nameof(CurrentOrder));
            }
            OnPropertyChanged(nameof(enabledCancelReason));
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
            if (!CurrentOrder.CheckStatus())
            {
                check = false;
                sb.AppendLine("Поле <Статус> содержит неизвестный статус.");
            }
            if (enabledCancelReason && !CurrentOrder.CheckCancelReason())
            {
                check = false;
                sb.AppendLine("При установке статуса <Отменена> нужно заполнить поле <Причина отмены>.");
            }
            msg = sb.ToString();
            return check;
        }
    }
}
