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
        private bool _showLayerPopup;

        public bool ShowLayerPopup
        {
            get { return this._showLayerPopup; }
            set { SetProperty(ref this._showLayerPopup, value); }
        }

        private string _controlName;

        public string ControlName
        {
            get { return this._controlName; }
            set { SetProperty(ref this._controlName, value); }
        }

        /// <summary>
        /// Busy 목록
        /// </summary>
        private IList<BusyMessage> _busys = new List<BusyMessage>();

        private bool _isBusy;
        /// <summary>
        /// IsBusy
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }


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

            //MainViewModel - CustomerViewModel 간의 네비게이션 메세지 수신을 위한 등록
            //WeakReferencMessenger 클래스를 사용하여 간단히 Pub/Sub 형태의 이벤트 큐 구현 가능 (Publish / Subscribe)
            //1. MainViewModel에서 수신할 메세지를 등록한다.
            //2. CustomerViewModel에서 메세지를 송신한다.
            WeakReferenceMessenger.Default.Register<NavigationMessage>(this, OnNavigationMessage);

            //MainViewModel - HomeViewModel 간의 Busy 메세지 수신을 위한 등록
            WeakReferenceMessenger.Default.Register<BusyMessage>(this, OnBusyMessage);

            WeakReferenceMessenger.Default.Register<LayerPopupMessage>(this, OnLayerPopupMessage);
        }

        private void OnLayerPopupMessage(object recipient, LayerPopupMessage message)
        {
            ShowLayerPopup = message.Value;
            ControlName = message.ControlName;
        }

        /// <summary>
        /// 비지 메시지 수신 처리
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="message"></param>
        private void OnBusyMessage(object recipient, BusyMessage message)
        {
            if (message.Value)
            {
                var existBusy = _busys.FirstOrDefault(b => b.BusyId == message.BusyId);
                if (existBusy != null)
                {
                    //이미 추가된 녀석이기 때문에 추가하지 않음
                    return;
                }
                _busys.Add(message);
            }
            else
            {
                var existBusy = _busys.FirstOrDefault(b => b.BusyId == message.BusyId);
                if (existBusy == null)
                {
                    //없기 때문에 나감
                    return;
                }
                _busys.Remove(existBusy);
            }
            //_busys에 아이템이 있으면 true, 없으면 false
            IsBusy = _busys.Any();
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
