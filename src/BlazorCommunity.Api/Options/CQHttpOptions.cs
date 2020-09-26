using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCommunity.Api.Options
{
    public class CQHttpOptions
    {
        public string ApiUrl { get; set; }
        public string AccessToken { get; set; }

        public List<QQS> ManageQQ { get; set; }
    }


    public class QQS
    {
        public string QQ { get; set; }
        public int Weight { get; set; }
    }
}
