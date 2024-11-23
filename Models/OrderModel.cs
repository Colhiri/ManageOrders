using System;
using System.Text;

namespace ManageOrders.Models
{
    public class OrderModel
    {
        private int _idOrder;
        private string _nameClient;
        private string _nameExecutor;
        private string _pickupAddress;
        private string _deliveryAddress;
        private DateTime _pickupTime;
        private string _status;
        private string _cancelReason;

        public Action statusAction;

        #region Проверка существующих значений
        public bool CheckNameClient() => !string.IsNullOrEmpty(_nameClient);
        public bool CheckNameExecutor() => !string.IsNullOrEmpty(_nameExecutor);
        public bool CheckPickupAddress() => !string.IsNullOrEmpty(_pickupAddress);
        public bool CheckDeliveryAddress() => !string.IsNullOrEmpty(_deliveryAddress);
        public bool CheckPickupTime() => _pickupTime > DateTime.Now;
        public bool CheckStatus() => Utility.Utility.statusOrder.Contains(_status);
        public bool CheckCancelReason() => true;
        #endregion

        /// <summary>
        /// Ид заказа
        /// </summary>
        public int IdOrder
        {
            get { return _idOrder; }
            set { _idOrder = value; }
        }
        public string NameClient 
        {
            get { return _nameClient; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Поле <Имя клиента> должно быть заполнено!");
                }
                _nameClient = value;
            }
        }
        public string NameExecutor
        {
            get { return _nameExecutor; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Поле <Имя исполнителя> должно быть заполнено!");
                }
                _nameExecutor = value;
            }
        }
        public string PickupAddress
        {
            get { return _pickupAddress; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Поле <Адрес клиента> должно быть заполнено!");
                }
                _pickupAddress = value;
            }
        }
        public string DeliveryAddress
        {
            get { return _deliveryAddress; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Поле <Адрес доставки> должно быть заполнено!");
                }
                _deliveryAddress = value;
            }
        }
        public DateTime PickupTime
        {
            get { return _pickupTime; }
            set
            {
                if (value < DateTime.Now)
                {
                    throw new ArgumentNullException("Поле <Время передачи посылки> не может быть заполнено задним числом!");
                }
                _pickupTime = value;
            }
        }
        public string Status
        {
            get { return _status; }
            set 
            {
                if (!Utility.Utility.statusOrder.Contains(value))
                {
                    throw new ArgumentException("Поле <Статус> содержит неизвестный статус.");
                }
                _status = value;

                statusAction?.Invoke();
            }
        }
        public string CancelReason
        {
            get { return _cancelReason; }
            set
            {
                // if (string.IsNullOrEmpty(value))
                // {
                //     throw new ArgumentNullException("Поле <Причина отмены> при статусе 'Отменена' должно быть заполнено!");
                // }
                _cancelReason = value;
            }
        }
    }
}
