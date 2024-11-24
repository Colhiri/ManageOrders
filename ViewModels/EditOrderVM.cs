using ManageOrders.Models;
using ManageOrders.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace ManageOrders.ViewModels
{
    public class EditOrderVM : BaseOrderVM
    {
        public EditOrderVM(OrderModel order) : base(order)
        {
        }

        public override void SetParameters()
        {
            Title = "Редактировать заявку";
            if (NewOrder != null)
            {
                NewOrder.statusAction += OnStatusChange;
            }
        }

        public override void ManageControlsOrder()
        {
            if (NewOrder  == null) { return; }

            bool isRightStatus = NewOrder.Status == "Новая";

            enabledNameClient = isRightStatus;
            enabledNameExecutor = isRightStatus;
            enabledPickupAddress = isRightStatus;
            enabledDeliveryAddress = isRightStatus;
            enabledPickupTime = isRightStatus;
            enabledStatus = true;
            enabledCancelReason = isRightStatus;

            List<string> temp = new List<string>();
            temp.AddRange(Utility.Utility.statusOrder);
            switch (NewOrder.Status)
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

        public override void ActionOrder()
        {
            // Отослать на сервер
            ServiceDB serviceDB = new ServiceDB(Config.pathToDB);
            serviceDB.EditOrder(NewOrder);
            ActionComplete = true;
            ClosingWindow?.Invoke();
        }

        public override bool CheckRun()
        {
            return NewOrder != null;
        }

        private void OnStatusChange()
        {
            enabledCancelReason = NewOrder.Status == "Отменена";
        }

        protected override bool CheckRunAction(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            bool check = true;
            if (!NewOrder.CheckNameClient())
            {
                check = false;
                sb.AppendLine("Поле <Имя клиента> должно быть заполнено!");
            }
            if (!NewOrder.CheckNameExecutor())
            {
                check = false;
                sb.AppendLine("Поле <Имя исполнителя> должно быть заполнено!");
            }
            if (!NewOrder.CheckPickupAddress())
            {
                check = false;
                sb.AppendLine("Поле <Адрес клиента> должно быть заполнено!");
            }
            if (!NewOrder.CheckDeliveryAddress())
            {
                check = false;
                sb.AppendLine("Поле <Адрес доставки> должно быть заполнено!");
            }
            if (!NewOrder.CheckPickupTime())
            {
                check = false;
                sb.AppendLine("Поле <Время передачи посылки> не может быть заполнено задним числом!");
            }
            if (!NewOrder.CheckStatus())
            {
                check = false;
                sb.AppendLine("Поле <Статус> содержит неизвестный статус.");
            }
            if (enabledCancelReason || !NewOrder.CheckCancelReason())
            {
                check = false;
                sb.AppendLine("При установке статуса <Отменена> нужно заполнить поле <Причина отмены>.");
            }
            msg = sb.ToString();
            return check;
        }
    }
}
