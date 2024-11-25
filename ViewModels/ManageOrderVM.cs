using ManageOrders.Models;
using ManageOrders.Utility;
using ManageOrders.View;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ManageOrders.ViewModels
{
    public class ManageOrderVM : BaseVM
    {
        private ApiService ApiService;
        public ObservableCollection<OrderModel> Orders { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand FilterOrderCommand { get; set; }
        public ICommand ExecuteOrderCommand { get; set; }
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        public ICommand ResetFilterOrderCommand { get; set; }

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
            ApiService = new ApiService();

            Orders = new ObservableCollection<OrderModel>();
            LoadOrders();

            CreateOrderCommand = new RelayCommand(CreateOrder);
            FilterOrderCommand = new RelayCommand(FilterOrders);
            ResetFilterOrderCommand = new RelayCommand(ResetFilterOrders);
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

            try
            {
                List<OrderModel> orders = await ApiService.GetOrdersAsync();

                foreach (OrderModel order in orders)
                {
                    Orders.Add(order);
                }
                OnPropertyChanged(nameof(Orders));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}");
            }
        }

        /// <summary>
        /// Создать заявку
        /// </summary>
        private void CreateOrder()
        {
            OrderModel newOrder = new OrderModel()
            {
                Status = "Новая",
                PickupTime = DateTime.Now,
            };
            CreateNewOrderVM viewModel = new CreateNewOrderVM(newOrder, ApiService);
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
                // Заменяем на ID с базы
                Orders.Add(viewModel.CurrentOrder);
                OnPropertyChanged(nameof(Orders));
            }
        }

        /// <summary>
        /// Отфильтровать заявки
        /// </summary>
        private void FilterOrders()
        {
            string input = Interaction.InputBox(
                "Введите текст для фильтра", // Текст над полем ввода 
                "Фильтрация заявок" // Название окна
                ).ToLower();

            if (string.IsNullOrEmpty(input)) { return; }

            Orders = new ObservableCollection<OrderModel>(Orders.Where(order => order.StrRepresent().ToLower().Contains(input)));
            OnPropertyChanged(nameof(Orders));
        }

        /// <summary>
        /// Сбросить фильтр
        /// </summary>
        private void ResetFilterOrders()
        {
            LoadOrders();
        }

        /// <summary>
        /// Поставить заявку на исполнение
        /// </summary>
        private void ExecuteOrder()
        {
            ExecuteStateOrderVM viewModel = new ExecuteStateOrderVM((OrderModel)SelectedOrder?.Clone(), ApiService);
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
                Orders[Orders.IndexOf(SelectedOrder)] = viewModel.CurrentOrder;
                OnPropertyChanged(nameof(Orders));
            }
        }

        /// <summary>
        /// Редактировать заявку
        /// </summary>
        private void EditOrder()
        {
            BaseOrderVM viewModel = new EditOrderVM((OrderModel)SelectedOrder?.Clone(), ApiService);
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
                Orders[Orders.IndexOf(SelectedOrder)] = viewModel.CurrentOrder;
                OnPropertyChanged(nameof(Orders));
            }
        }

        /// <summary>
        /// Удалить заявку
        /// </summary>
        private async void DeleteOrder()
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

            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    await ApiService.DeleteOrderAsync(SelectedOrder.IdOrder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заявок: {ex.Message}");
                return;
            }

            Orders.Remove(SelectedOrder);
            SelectedOrder = null;
            OnPropertyChanged(nameof(Orders));
        }
    }
}
