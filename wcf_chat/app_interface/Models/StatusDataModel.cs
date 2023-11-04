using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_interface.Models
{
    public class StatusDataModel
    {
        public string ContactName{ get; set; }
        public Uri ContactPhoto { get; set; }
        /// <summary>
        /// We will be converting in one cpksdpck
        /// to-do satus massage
        /// </summary>
        public Uri StatusImage { get; set; }
       
        //if we want to add our status
        public bool IsMeAddStatus { get; set; }


    }
}
