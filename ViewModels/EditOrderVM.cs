using ManageOrders.Utility;

namespace ManageOrders.ViewModels
{
    public class EditOrderVM : BaseOrderVM
    {
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
            bool isRightStatus = NewOrder?.Status == "Новая";

            enabledNameClient = isRightStatus;
            enabledNameExecutor = isRightStatus;
            enabledPickupAddress = isRightStatus;
            enabledDeliveryAddress = isRightStatus;
            enabledPickupTime = isRightStatus;
            enabledStatus = true;
            enabledCancelReason = isRightStatus;

            visibleCancelReason = false;
        }

        public override void ActionOrder()
        {
            // Отослать на сервер
            ServiceDB serviceDB = new ServiceDB(Config.pathToDB);
            serviceDB.EditOrder(NewOrder);
        }

        public override bool CheckRun()
        {
            return NewOrder != null;
        }

        private void OnStatusChange()
        {
            visibleCancelReason = NewOrder.Status == "Отменена";
        }

        protected override bool CheckRunAction(out string msg) 
        {
            msg = string.Empty;    
            return true; 
        }
    }
}
