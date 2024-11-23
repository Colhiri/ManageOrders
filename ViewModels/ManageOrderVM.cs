using ManageOrders.Models;
using ManageOrders.Utility;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ManageOrders.ViewModels
{
    public class ManageOrderVM : INotifyPropertyChanged
    {
        public ObservableCollection<OrderModel> Orders { get; set; }
        public ICommand CreateOrderCommand { get; set; }
        public ICommand FilterOrderCommand { get; set; }
        public ICommand ExecuteOrderCommand { get; set; }
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

        public ManageOrderVM()
        {
            Orders = new ObservableCollection<OrderModel>();
            CreateOrderCommand = new RelayCommand(CreateOrder);
            FilterOrderCommand = new RelayCommand(FilterOrders);
            ExecuteOrderCommand = new RelayCommand(ExecuteOrder);
            EditOrderCommand = new RelayCommand(EditOrder);
            DeleteOrderCommand = new RelayCommand(DeleteOrder);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Создать заявку
        /// </summary>
        private void CreateOrder()
        {

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

        }

        /// <summary>
        /// Редактировать заявку
        /// </summary>
        private void EditOrder()
        {

        }

        /// <summary>
        /// Удалить заявку
        /// </summary>
        private void DeleteOrder()
        {

        }

    }
}
