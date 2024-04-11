using System.Configuration;
using System.Data;
using System.Windows;
using WpfExample.Views;

namespace WpfExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //App.xaml의 StartUp 이벤트 핸들러에 등록하여 시작
        private void StartUpApp(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }

        // TODO : 해당 메서드로 수행시 App.xaml에 정의된 App.Resources를 읽지 못하는 이유는 ?

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    MainWindow window = new MainWindow();
        //    window.DataContext = new MainViewModel();
        //    window.ShowDialog();
        //}
    }
}
