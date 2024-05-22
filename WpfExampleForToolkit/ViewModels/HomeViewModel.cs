﻿using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfExampleForToolkit.Models;

namespace WpfExampleForToolkit.ViewModels
{
    internal class HomeViewModel : ViewModelBase
    {
        public static int Count { get; set; }

        public ICommand BusyTestCommand { get; set; }

        public HomeViewModel()
        {
            Title = "Home";

            this.Init();
        }

        private void Init()
        {
            this.BusyTestCommand = new AsyncRelayCommand(OnBusyTestAsync);
        }

        /// <summary>
        /// BusyTestCommand의 '비동기 함수' 실행
        /// </summary>
        /// <returns></returns>
        private async Task OnBusyTestAsync()
        {
            WeakReferenceMessenger.Default.Send(new BusyMessage(true) { BusyId = "OnBusyTestAsync" });
            await Task.Delay(5000);
            WeakReferenceMessenger.Default.Send(new BusyMessage(false) { BusyId = "OnBusyTestAsync" });
        }

        public override void OnNavigated(object sender, object navigatedEventArgs)
        {
            Count++;
            Message = $"{Count} Navigated";
        }
    }
}
