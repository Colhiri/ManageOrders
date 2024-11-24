using ManageOrders.Models;
using ManageOrders.Utility;
using System.Text;

namespace ManageOrders.ViewModels
{
    public class ExecuteOrderVM : BaseOrderVM
    {
        public ExecuteOrderVM(OrderModel order) : base(order)
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

        public override void ActionOrder()
        {
            // Отослать на сервер
            ServiceDB serviceDB = new ServiceDB(Config.pathToDB);
            NewOrder.Status = "Передано на выполнение";
            serviceDB.EditOrder(NewOrder);
            ActionComplete = true;
            ClosingWindow?.Invoke();
        }

        public override bool CheckRun()
        {
            return NewOrder?.Status == "Новая";
        }

        protected override bool CheckRunAction(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            bool check = true;
            
            if (!NewOrder.CheckStatus())
            {
                check = false;
                sb.AppendLine("Поле <Статус> содержит неизвестный статус.");
            }
            
            msg = sb.ToString();
            return check;
        }
    }
}
