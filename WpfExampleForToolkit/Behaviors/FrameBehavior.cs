using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Xaml.Behaviors;
using WpfExampleForToolkit.Interfaces;

namespace WpfExampleForToolkit.Behaviors
{
    /// <summary>
    /// Frame 비헤이비어
    /// </summary>
    public class FrameBehavior : Behavior<Frame>
    {
        /// <summary>
        /// NavigationSource DP 변경 때문에 발생하는 프로퍼티 체인지 이벤트를 막기 위해 사용
        /// </summary>
        private bool _isWork;

        /// <summary>
        /// NavigationSource DP
        /// ViewModel의 Property와 Binding 한다.
        /// </summary>
        public static readonly DependencyProperty NavigationSourceProperty = DependencyProperty.Register(nameof(NavigationSource), typeof(string), typeof(FrameBehavior), new PropertyMetadata(null, NavigationSourceChanged));

        public string NavigationSource
        {
            get { return (string)GetValue(NavigationSourceProperty); }
            set { SetValue(NavigationSourceProperty, value); }
        }

        protected override void OnAttached()
        {
            //네비게이션 시작
            AssociatedObject.Navigating += AssociatedObject_Navigating;
            //네비게이션 종료
            AssociatedObject.Navigated += AssociatedObject_Navigated;
        }

        /// <summary>
        /// 3. Frame이 Navigating 되기 시작할 때 발생하는 이벤트.
        ///    네비게이션이 시작 전, 상황을 ViewModel에게 알린다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            //Frame의 Content에는 Page가 존재한다.
            if (AssociatedObject.Content is Page pageContent
                && pageContent.DataContext is INavigationAware navigationAware)
            {
                navigationAware?.OnNavigating(sender, e);
            }
        }

        /// <summary>
        /// 네비게이션 종료 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Navigated(object sender, NavigationEventArgs e)
        {
            _isWork = true;
            //네비게이션이 완료된 Uri를 NavigationSource에 입력
            NavigationSource = e.Uri.ToString();
            _isWork = false;
            //네비게이션이 완료된 상황을 뷰모델에 알려주기
            if (AssociatedObject.Content is Page pageContent
                && pageContent.DataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigated(sender, e);
            }
        }


        protected override void OnDetaching()
        {
            AssociatedObject.Navigating -= AssociatedObject_Navigating;
            AssociatedObject.Navigated -= AssociatedObject_Navigated;
        }

        /// <summary>
        /// NavigationSource PropertyChanged
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void NavigationSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (FrameBehavior)d;
            if (behavior._isWork)
            {
                return;
            }
            behavior.Navigate();
        }

        /// <summary>
        /// FrameBehavior의 Navigate 함수.
        /// FrameBehavior의 의존 프로퍼티에 따른 Frame(AssociatedObject)에 Page를 Navigate한다.
        /// </summary>
        private void Navigate()
        {
            switch (NavigationSource)
            {
                case "GoBack":
                    //GoBack으로 오면 뒤로가기
                    if (AssociatedObject.CanGoBack)
                    {
                        AssociatedObject.GoBack();
                    }
                    break;
                case null:
                case "":
                    //아무것도 안함
                    return;
                default:
                    //navigate
                    AssociatedObject.Navigate(new Uri(NavigationSource, UriKind.RelativeOrAbsolute));
                    break;
            }
        }
    }
}
