using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuwAdmin.Web.Models.SharedViewModels
{
    public class PageMessageViewModel
    {
        public string MessageType
        {
            get { return _messageType;  }
            set { SetMessageType(value); }
        }
        public string MessageText { get; set; }
        public string CssClass { private set; get; }
        public string IconClass { private set; get; }

        private string _messageType;

        private void SetMessageType(string value)
        {
            _messageType = value;
            switch (value.ToLower())
            {
                case "success":
                {
                    CssClass = "success";
                    IconClass = "fa fa-check-square";
                    break;
                }
                case "error":
                {
                    CssClass = "error";
                    IconClass = "fa fa-times-circle";
                    break;
                }
                case "warning":
                {
                    CssClass = "warning";
                    IconClass = "fa fa-exclamation-triangle";
                    break;
                }
                case "information":
                {
                    CssClass = "information";
                    IconClass = "fa fa-info-circle";
                    break;
                }
                default:
                {
                    //_messageType = null;
                    break;
                }
            }
        }
    }
}
