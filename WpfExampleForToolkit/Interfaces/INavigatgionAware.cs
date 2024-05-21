using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.Interfaces
{
    /// <summary>
    /// 네비게이션 시작, 종료되는 시점을 뷰모델에 알려주는 인터페이스
    /// </summary>
    public interface INavigationAware
    {
        void OnNavigating(object sender, object navigationEventArgs);
        void OnNavigated(object sender, object navigatedEventArgs);
    }
}
