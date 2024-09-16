using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowDoYouFeel.MainMenu
{
    public struct CreditContent
    {
        public string Header { get; set; }
        public string Content { get; set; }

        public CreditContent(string _header, string _content)
        {
            Header = _header;
            Content = _content;
        }
    }
}
