using ManageOrders.Models;
using ManageOrders.Utility;
using ManageOrders.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ManageOrders.ViewModels
{
    public class ManageOrderVM : BaseVM
    {
        private ServiceDB ServiceDB;
        public ObservableCollection<OrderModel> Orders { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand FilterOrderCommand { get; set; }
        public ICommand ExecuteOrderCommand { get; set; }
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

        private OrderModel _selectedOrder;
        public OrderModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
            }
        }

        public ManageOrderVM()
        {
            ServiceDB = new ServiceDB(Config.pathToDB);

            Orders = new ObservableCollection<OrderModel>();
            LoadOrders();

            CreateOrderCommand = new RelayCommand(CreateOrder);
            FilterOrderCommand = new RelayCommand(FilterOrders);
            ExecuteOrderCommand = new RelayCommand(ExecuteOrder);
            EditOrderCommand = new RelayCommand(EditOrder);
            DeleteOrderCommand = new RelayCommand(DeleteOrder);
        }

        /// <summary>
        /// Загрузить все заявки в ListView
        /// </summary>
        private void LoadOrders()
        {
            foreach (OrderModel order in ServiceDB.GetOrders())
            {
                Orders.Add(order);
            }
        }

        /// <summary>
        /// Создать заявку
        /// </summary>
        private void CreateOrder()
        {
            CreateNewOrderVM viewModel = new CreateNewOrderVM();
            viewModel.NewOrder = new OrderModel();
            if (!viewModel.CheckRun())
            {
                return;
            }
            WorkOrderView view = new WorkOrderView();
            view.DataContext = viewModel;
            view.Show();
        }

        /// <summary>
        /// Отфильтровать заявки
        /// </summary>
        private void FilterOrders()
        {

        }

        /// <summary>
        /// Поставить заявку на исполнение
        /// </summary>
        private void ExecuteOrder()
        {
            ExecuteOrderVM viewModel = new ExecuteOrderVM();
            viewModel.NewOrder = SelectedOrder;
            if (!viewModel.CheckRun())
            {
                MessageBox.Show("Выберите заявку со статусом <Новая>!");
                return;
            }
            WorkOrderView view = new WorkOrderView();
            view.DataContext = viewModel;
            view.Show();
        }

        /// <summary>
        /// Редактировать заявку
        /// </summary>
        private void EditOrder()
        {
            EditOrderVM viewModel = new EditOrderVM();
            viewModel.NewOrder = SelectedOrder;
            if (!viewModel.CheckRun())
            {
                MessageBox.Show("Выберите заявку!");
                return;
            }
            WorkOrderView view = new WorkOrderView();
            view.DataContext = viewModel;
            view.Show();
        }

        /// <summary>
        /// Удалить заявку
        /// </summary>
        private void DeleteOrder()
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Выберите заявку!");
                return;
            }

            MessageBoxResult result = MessageBox.Show(
            "Удалить заявку?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ServiceDB.DeleteOrder(SelectedOrder.IdOrder);
            }
        }
    }
}
