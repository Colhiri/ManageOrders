using ManageOrders.Models;
using ManageOrders.Utility;
using System;
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

        public bool ActionComplete 
        { 
            get; 
            protected set; 
        } = false;

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

        public Action ClosingWindow;

        public BaseOrderVM(OrderModel order)
        {
            NewOrder = order;
            Initialize();
        }

        private void Initialize()
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
