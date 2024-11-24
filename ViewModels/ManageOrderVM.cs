using ManageOrders.Models;
using ManageOrders.Utility;
using ManageOrders.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ManageOrders.ViewModels
{
    public class ManageOrderVM : BaseVM
    {
        private ServiceDB ServiceDB;
        private ApiService ApiService;
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
            ApiService = new ApiService();

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
        private async void LoadOrders()
        {
            Orders.Clear();

            List<OrderModel> orders = await ApiService.GetOrders();

            foreach (OrderModel order in orders)// ServiceDB.GetOrders())
            {
                Orders.Add(order);
            }
            OnPropertyChanged();
        }

        /// <summary>
        /// Создать заявку
        /// </summary>
        private void CreateOrder()
        {
            CreateNewOrderVM viewModel = new CreateNewOrderVM(new OrderModel() 
            { 
                Status = "Новая" 
            });
            if (!viewModel.CheckRun())
            {
                return;
            }
            WorkOrderView view = new WorkOrderView();
            viewModel.ClosingWindow += view.Close;
            view.DataContext = viewModel;
            view.ShowDialog();
            if (viewModel.ActionComplete)
            {
                Orders.Add(viewModel.NewOrder);
                OnPropertyChanged();
            }
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
            ExecuteOrderVM viewModel = new ExecuteOrderVM((OrderModel)SelectedOrder?.Clone());
            if (!viewModel.CheckRun())
            {
                MessageBox.Show("Выберите заявку со статусом <Новая>!");
                return;
            }
            WorkOrderView view = new WorkOrderView();
            viewModel.ClosingWindow += view.Close;
            view.DataContext = viewModel;
            view.ShowDialog();

            if (viewModel.ActionComplete)
            {
                Orders[Orders.IndexOf(SelectedOrder)] = viewModel.NewOrder;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Редактировать заявку
        /// </summary>
        private void EditOrder()
        {
            BaseOrderVM viewModel = new EditOrderVM((OrderModel)SelectedOrder?.Clone());
            if (!viewModel.CheckRun())
            {
                MessageBox.Show("Выберите заявку!");
                return;
            }
            WorkOrderView view = new WorkOrderView();
            viewModel.ClosingWindow += view.Close;
            view.DataContext = viewModel;
            view.ShowDialog();

            if (viewModel.ActionComplete)
            {
                Orders[Orders.IndexOf(SelectedOrder)] = viewModel.NewOrder;
                OnPropertyChanged();
            }
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

                Orders.Remove(SelectedOrder);

                SelectedOrder = null;

                OnPropertyChanged();
            }
        }
    }
}
