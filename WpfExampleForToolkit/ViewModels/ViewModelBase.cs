using CommunityToolkit.Mvvm.ComponentModel;
using WpfExampleForToolkit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, INavigationAware
    {

		private string? _title;

		public string? Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

        private string? _message;
        /// <summary>
        /// 메시지
        /// </summary>
        public string? Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        /// <summary>
        /// 네비게이션 완료시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="navigatedEventArgs"></param>
        public virtual void OnNavigated(object sender, object navigatedEventArgs)
        {
        }
        /// <summary>
        /// 네비게이션 시작시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="navigationEventArgs"></param>
        public virtual void OnNavigating(object sender, object navigationEventArgs)
        {
        }
	}
}
