using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.Models
{
    internal class LayerPopupMessage : ValueChangedMessage<bool>
    {
        /// <summary>
        /// 컨트롤 이름
        /// </summary>
        public string ControlName { get; set; }
        /// <summary>
        /// 컨트롤에 전달할 파라메터
        /// </summary>
        public object Parameter { get; set; } = null;

        public LayerPopupMessage(bool value) : base(value)
        {
        }
    }
}
