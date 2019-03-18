using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDemo.Models
{
    public class OrderStateModel : NotificationObject
    {

        public string OrderID { get; set; }
        public string OrderStartTime { get; set; }
        public string CurrentState { get; set; }
        public string ProcessingName { get; set; }
        public bool TakeOrderIsOK { get; set; }
    }
}
