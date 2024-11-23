using ManageOrders.Utility;

namespace ManageOrders.ViewModels
{
    public class ExecuteOrderVM : BaseOrderVM
    {
        public override void SetParameters() => Title = "Передать заявку на выполнение";

        public override void ManageControlsOrder()
        {
            enabledNameClient = false;
            enabledPickupAddress = false;
            enabledDeliveryAddress = false;
            enabledPickupTime = false;
            enabledStatus = false;
            enabledCancelReason = false;

            visibleStatus = false;
            visibleCancelReason = false;
        }

        public override void ActionOrder()
        {
            // Отослать на сервер
            ServiceDB serviceDB = new ServiceDB(Config.pathToDB);
            NewOrder.Status = "Передано на исполнение";
            serviceDB.EditOrder(NewOrder);
        }

        public override bool CheckRun()
        {
            return NewOrder?.Status == "Новая";
        }

        protected override bool CheckRunAction(out string msg)
        {
            msg = string.Empty;
            return true;
        }
    }
}
