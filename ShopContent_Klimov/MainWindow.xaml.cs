using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopContent_Klimov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow init;
        public View.Main Main = new View.Main();
        public View.MainCategory Category = new View.MainCategory();
        public MainWindow()
        {
            InitializeComponent();
            init = this;
            frame.Navigate(Main);
        }

        private void OpenIndex(object sender, MouseButtonEventArgs e)
        {
            frame.Navigate(Main);
        }

        private void OpenCategory(object sender, MouseButtonEventArgs e)
        {
            frame.Navigate(Category);
        }
    }
}