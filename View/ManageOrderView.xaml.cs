using System.Windows;

namespace ManageOrders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManageOrderView : Window
    {
        public ManageOrderView()
        {
            InitializeComponent();

            DataContext = new ViewModels.ManageOrderVM();
        }
    }
}
