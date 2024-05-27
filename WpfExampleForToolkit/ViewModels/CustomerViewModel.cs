using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WpfExampleForToolkit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfExampleForToolkit.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace WpfExampleForToolkit.ViewModels
{
    public partial class CustomerViewModel : ViewModelBase
    {

        private readonly IDatabaseService _dbService;

        [ObservableProperty]
        private IList<Customer> _customers;

        [ObservableProperty]
        private Customer _selectedCustomer;

        [ObservableProperty]
        private string _errorMessage;
        public IRelayCommand SaveCommand { get; set; }

        public ICommand BackCommand { get; set; }

        public CustomerViewModel(IDatabaseService databaseService)
        {
            this._dbService = databaseService;
            Init();
        }

        private void Init()
        {
            Title = "Customer";
            BackCommand = new RelayCommand(OnBack);
            SaveCommand = new RelayCommand(Save, () => Customers != null && Customers.Any(c => string.IsNullOrWhiteSpace(c?.CustomerID)));

            PropertyChanging += CustomerViewModel_PropertyChanging;
            PropertyChanged += CustomerViewModel_PropertyChanged;
        }

        private void OnBack()
        {
            WeakReferenceMessenger.Default.Send(new NavigationMessage("GoBack"));
        }

        /// <summary>
        /// AddCommand
        /// </summary>
        [RelayCommand]
        private void Add()
        {
            var newCustomer = new Customer();
            Customers.Insert(0, newCustomer);
            SelectedCustomer = newCustomer;
            SaveCommand.NotifyCanExecuteChanged();
        }

        private void Save()
        {
            MessageBox.Show("Save");
            SaveCommand.NotifyCanExecuteChanged();
        }

        public override async void OnNavigated(object sender, object navigatedEventArgs)
        {
            Message = "Navigated";

            var datas = await this._dbService.GetDatasAsync<Customer>("Select * from dbo.Customers");
            Customers = new ObservableCollection<Customer>(datas);
        }

        private void CustomerViewModel_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedCustomer):
                    if (SelectedCustomer != null)
                    {
                        SelectedCustomer.ErrorsChanged -= SelectedCustomer_ErrorsChanged;
                        ErrorMessage = string.Empty;
                    }
                    break;
            }
        }

        private void CustomerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedCustomer):
                    if (SelectedCustomer != null)
                    {
                        SelectedCustomer.ErrorsChanged += SelectedCustomer_ErrorsChanged;
                        SetErrorMessage(SelectedCustomer);
                    }
                    break;
            }
        }

        private void SelectedCustomer_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            SetErrorMessage(sender as Customer);
        }

        private void SetErrorMessage(Customer customer)
        {
            if (customer == null)
            {
                return;
            }
            var errors = customer.GetErrors();
            ErrorMessage = string.Join("\n", errors.Select(e => e.ErrorMessage));
        }
    }
}
