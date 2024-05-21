using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfExampleForToolkit.Models
{
    internal class NavigationMessage : ValueChangedMessage<string>
    {
        public NavigationMessage(string value) : base(value)
        {
        }
    }
}
