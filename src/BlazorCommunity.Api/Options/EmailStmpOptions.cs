namespace BlazorCommunity.Api.Options
{


    public class EmailStmpOption
    {
        public EmailStmpConfig EmailSetting { get; set; }

    }
    public class EmailStmpConfig
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
    }
}