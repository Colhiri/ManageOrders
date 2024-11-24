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
    }
}
