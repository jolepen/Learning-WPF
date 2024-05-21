using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfExampleForToolkit.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;


namespace WpfExampleForToolkit.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string? _navigationSource;

        public string? NavigationSource
        {
            get { return _navigationSource; }
            set { SetProperty(ref _navigationSource, value); }
        }

        /// <summary>
        ///1. View에 NavigateCommand를 Binding 시키고, CommandParameter로 NavigateCommand로 페이지 주소를 입력 받음.
        /// </summary>
        public ICommand NavigateCommand { get; set; }

        public MainViewModel()
        {
            this.Title = "Main View";
            Init();
        }

        private void Init()
        {
            //시작페이지 설정
            NavigationSource = "Views/HomePage.xaml";
            NavigateCommand = new RelayCommand<string>(OnNavigate);

            //네비게이션 메시지 수신 등록
            WeakReferenceMessenger.Default.Register<NavigationMessage>(this, OnNavigationMessage);
        }

        /// <summary>
        /// 네비게이션 메시지 수신 처리
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        private void OnNavigationMessage(object recipient, NavigationMessage message)
        {
            NavigationSource = message.Value;
        }

        /// <summary>
        ///1. CommandParameter로 NavigateCommand로 페이지 주소를 입력 받은 값을 NavigationSource로 전달하여 PropertyChanged 이벤트 발생시킴
        ///2. 여기서 FrameBehavior의 NavigationSource 의존 프로퍼티와 해당 ViewModel의 NavigationSource 프로퍼티가 서로 TwoWay Binding이 이루어져 있어
        ///   ViewModel의 NavigationSource가 변경되는 경우 FrameBehavior의 NavigationSource의 이벤트 발생(그렇다면, 3번은 FrameBehavior에 존재)
        /// </summary>
        private void OnNavigate(string? pageUri)
        {
            NavigationSource = pageUri;
        }
    }
}
