using ManageOrders.Models;
using ManageOrders.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ManageOrders.ViewModels
{
    /// <summary>
    /// Базовый класс для форм работы с заявкой
    /// </summary>
    public abstract class BaseOrderVM : INotifyPropertyChanged
    {
        #region Параметры для блокировки контролов на форме
        public bool enabledNameClient { get; set; } = true;
        public bool enabledNameExecutor { get; set; } = true;
        public bool enabledPickupAddress { get; set; } = true;
        public bool enabledDeliveryAddress { get; set; } = true;
        public bool enabledPickupTime { get; set; } = true;
        public bool enabledStatus { get; set; } = true;
        public bool enabledCancelReason { get; set; }  = true;
        #endregion

        /// <summary>
        /// Завершилось ли действие успешно или нет
        /// </summary>
        public bool ActionComplete 
        { 
            get; 
            protected set; 
        } = false;

        
        private OrderModel _currentOrder;
        /// <summary>
        /// Текущая заявка
        /// </summary>
        public OrderModel CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                _currentOrder = value;
                SetParameters();
            }
        }

        public ApiService apiService { get; private set; }

        /// <summary>
        /// Коллекция статусов заявки
        /// </summary>
        public ReadOnlyObservableCollection<string> StatusOrder { get; set; }
        /// <summary>
        /// Имя формы
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Действие выполняемое при сохранении данных на форме
        /// </summary>
        public ICommand ActionOrderCommand { get; set; }
        /// <summary>
        /// Событие для закрытия окна, чтобы закрывать его при сохранении данных на форме
        /// </summary>
        public Action ClosingWindow;

        public BaseOrderVM(OrderModel order, ApiService apiService)
        {
            CurrentOrder = order;
            this.apiService = apiService;
            Initialize();
        }

        /// <summary>
        /// Инициализация основных параметров
        /// </summary>
        private void Initialize()
        {
            StatusOrder = new ReadOnlyObservableCollection<string>(Utility.Utility.statusOrder);
            ActionOrderCommand = new RelayCommand(ActionOrder);
            SetParameters();
            ManageControlsOrder();
        }

        /// <summary>
        /// Установить параметры 
        /// </summary>
        public abstract void SetParameters();
        /// <summary>
        /// Установить доступность контролов
        /// </summary>
        public abstract void ManageControlsOrder();
        /// <summary>
        /// Действие при сохранении формы
        /// </summary>
        public abstract void ActionOrder();
        /// <summary>
        /// Проверка возможности инициализации формы
        /// </summary>
        /// <returns>Возможность запуска формы</returns>
        public abstract bool CheckRun();
        /// <summary>
        /// Проверка возможности завершения сохранения формы
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Возможность завершения</returns>
        protected abstract bool CheckRunAction(out string msg);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
