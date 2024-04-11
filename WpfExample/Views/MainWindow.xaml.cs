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

namespace WpfExample.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HierarchyControl hierarchy = new();
        private ChatControl chat = new();

        private List<UserControl> users = new();

        public MainWindow()
        {
            InitializeComponent();

            users.AddRange([hierarchy, chat]);
        }

        private void SetUserControlToGrid(UserControl userControl)
        {
            var any = users.Where(x => x != userControl);

            foreach (var item in any)
            {
                if (this.grid.Children.Contains(item))
                {
                    this.grid.Children.Remove(item);
                }
            }

            if (!this.grid.Children.Contains(userControl))
            {
                this.grid.Children.Add(userControl);
            }

            Grid.SetColumn(userControl, 1);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            SetUserControlToGrid(hierarchy);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            SetUserControlToGrid(chat);
        }
    }
}