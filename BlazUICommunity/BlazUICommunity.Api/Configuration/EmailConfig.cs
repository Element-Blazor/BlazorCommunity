using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazui.Community.Api.Configuration
{
    public class EmailConfig: IOptions<EmailConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Auth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StmpHost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StmpPort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FromName { get; set; }

        public EmailConfig Value => this;
    }
}
