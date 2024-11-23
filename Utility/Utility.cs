using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ManageOrders.Utility
{
    public static class Utility
    {
        public static ObservableCollection<string> statusOrder = new ObservableCollection<string>()
        {
            "Новая",
            "Передано на выполнение",
            "Выполнено",
            "Отменена",
        };

        /// <summary>
        /// Получаем статус для записи в базу данных
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static string GetStatusToDb(StatusOrder status)
        {
            return status switch
            {
                StatusOrder.New => "Новая",
                StatusOrder.Execute => "Передано на выполнение",
                StatusOrder.Complete => "Выполнено",
                StatusOrder.Cancel => "Отменена",
                _ => throw new System.Exception("Неизвестный тип статуса!"),
            };
        }

        /// <summary>
        /// Получаем расшифровку для пользователя
        /// </summary>
        /// <param name="statusDB"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static StatusOrder GetStatusFromDb(string statusDB)
        {
            return statusDB switch
            {
                "Новая" => StatusOrder.New,
                "Передано на выполнение" => StatusOrder.Execute,
                "Выполнено" => StatusOrder.Complete,
                "Отменена" => StatusOrder.Cancel,
                _ => throw new System.Exception("Неизвестный тип статуса!"),
            };
        }
    }
}
