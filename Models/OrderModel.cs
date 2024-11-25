﻿using System;

namespace ManageOrders.Models
{
    public class OrderModel : ICloneable
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
        public bool CheckCancelReason() => !string.IsNullOrEmpty(_cancelReason);
        #endregion

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
                _nameClient = value;
            }
        }
        public string NameExecutor
        {
            get { return _nameExecutor; }
            set
            {
                _nameExecutor = value;
            }
        }
        public string PickupAddress
        {
            get { return _pickupAddress; }
            set
            {
                _pickupAddress = value;
            }
        }
        public string DeliveryAddress
        {
            get { return _deliveryAddress; }
            set
            {
                _deliveryAddress = value;
            }
        }
        public DateTime PickupTime
        {
            get { return _pickupTime; }
            set
            {
                _pickupTime = value;
            }
        }
        public string Status
        {
            get { return _status; }
            set 
            {
                _status = value;

                statusAction?.Invoke();
            }
        }
        public string CancelReason
        {
            get { return _cancelReason; }
            set
            {
                _cancelReason = value;
            }
        }

        /// <summary>
        /// Клонирование для создание поверхностной копии заявки
        /// </summary>
        /// <returns>Копия заявки</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Строковое представление класса
        /// </summary>
        /// <returns>Строка со всеми полями</returns>
        public string StrRepresent()
        {
            return $"{IdOrder} {NameClient ?? ""} {NameExecutor ?? ""} {PickupAddress ?? ""} {PickupTime} {DeliveryAddress ?? ""} {PickupTime} {Status ?? ""} {CancelReason ?? ""}";
        }
    }
}
