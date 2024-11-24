using ManageOrders.Utility;
using System.Text;
using System.Windows;
using ManageOrders.Models;

namespace ManageOrders.ViewModels
{
    public class CreateNewOrderVM : BaseOrderVM
    {
        public CreateNewOrderVM(OrderModel order) : base(order)
        {
        }

        public override void SetParameters() => Title = "Создать заявку";

        public override void ManageControlsOrder()
        {
            enabledStatus = false;
            enabledCancelReason = false;
        }

        public override void ActionOrder()
        {
            if (!CheckRunAction(out string msg))
            {
                MessageBox.Show(msg);
                return;
            }
            // Отослать на сервер
            ServiceDB serviceDB = new ServiceDB(Config.pathToDB);
            serviceDB.CreateOrder(NewOrder);

            ActionComplete = true;

            ClosingWindow?.Invoke();
        }

        public override bool CheckRun()
        {
            return NewOrder != null;
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
            // if (!NewOrder.CheckStatus())
            // {
            //     check = false;
            //     sb.AppendLine("Поле <Статус> содержит неизвестный статус.");
            // }
            // if (!NewOrder.CheckCancelReason())
            // {
            //     check = false;
            //     sb.AppendLine("");
            // }
            msg = sb.ToString();
            return check;
        }
    }
}
