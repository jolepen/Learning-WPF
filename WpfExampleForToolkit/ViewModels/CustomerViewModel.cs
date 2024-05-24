using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfExampleForToolkit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfExampleForToolkit.Services;

namespace WpfExampleForToolkit.ViewModels
{
    internal class CustomerViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }

        private readonly IDatabaseService _dbService;
        public CustomerViewModel(IDatabaseService databaseService)
        {
            this._dbService = databaseService;
            Init();
        }

        private void Init()
        {
            Title = "Customer";
            BackCommand = new RelayCommand(OnBack);
        }

        private void OnBack()
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage("GoBack"));
        }

        public override void OnNavigated(object sender, object navigatedEventArgs)
        {
            Message = "Navigated";
        }
    }
}
