using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        public static int Count { get; set; }

        public HomeViewModel()
        {
            Title = "Home";
        }

        public override void OnNavigated(object sender, object navigatedEventArgs)
        {
            Count++;
            Message = $"{Count} Navigated";
        }
    }
}
