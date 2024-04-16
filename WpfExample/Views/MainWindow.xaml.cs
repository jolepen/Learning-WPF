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
        private readonly LayoutSampleControl layoutSample = new();
        private readonly HierarchyControl hierarchy = new();
        private readonly ChatControl chat = new();

        private readonly List<UserControl> users = [];

        public MainWindow()
        {
            InitializeComponent();

            users.AddRange([hierarchy, chat, layoutSample]);
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

        private void ButtonLayoutSample_Click(object sender, RoutedEventArgs e)
        {
            SetUserControlToGrid(layoutSample);
        }

        private void ButtonHierarchy_Click(object sender, RoutedEventArgs e)
        {
            SetUserControlToGrid(hierarchy);
        }

        private void ButtonChat_Click(object sender, RoutedEventArgs e)
        {
            SetUserControlToGrid(chat);
        }
    }
}