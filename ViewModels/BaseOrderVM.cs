using ManageOrders.Models;
using ManageOrders.Utility;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ManageOrders.ViewModels
{
    public abstract class BaseOrderVM
    {
        public bool enabledNameClient { get; set; } = true;
        public bool enabledNameExecutor { get; set; } = true;
        public bool enabledPickupAddress { get; set; } = true;
        public bool enabledDeliveryAddress { get; set; } = true;
        public bool enabledPickupTime { get; set; } = true;
        public bool enabledStatus { get; set; } = true;
        public bool enabledCancelReason { get; set; }  = true;

        public bool visibleNameClient { get; set; } = true;
        public bool visibleNameExecutor { get; set; } = true;
        public bool visiblePickupAddress { get; set; } = true;
        public bool visibleDeliveryAddress { get; set; } = true;
        public bool visiblePickupTime { get; set; } = true;
        public bool visibleStatus { get; set; } = true;
        public bool visibleCancelReason { get; set; }  = true;

        private OrderModel _newOrder;
        public OrderModel NewOrder
        {
            get { return _newOrder; }
            set
            {
                _newOrder = value;
                SetParameters();
            }
        }
        public ReadOnlyObservableCollection<string> StatusOrder { get; set; }
        public string Title { get; set; }
        public ICommand ActionOrderCommand { get; set; }

        public BaseOrderVM() 
        {
            StatusOrder = new ReadOnlyObservableCollection<string>(Utility.Utility.statusOrder);
            ActionOrderCommand = new RelayCommand(ActionOrder);
            SetParameters();
            ManageControlsOrder();
        }

        public abstract void SetParameters();
        public abstract void ManageControlsOrder();
        public abstract void ActionOrder();
        public abstract bool CheckRun();
        protected abstract bool CheckRunAction(out string msg);
    }
}
