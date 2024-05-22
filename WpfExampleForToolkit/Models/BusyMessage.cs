using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WpfExampleForToolkit.Models
{
    /// <summary>
    /// Busy 화면의 보기 속성을 컨트롤...
    /// </summary>
    public class BusyMessage : ValueChangedMessage<bool>
    {
        public string BusyId { get; set; }

        public string BusyText { get; set; }

        public BusyMessage(bool value) : base(value)
        {
        }
    }
}
