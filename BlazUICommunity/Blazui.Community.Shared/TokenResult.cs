using System;

namespace Blazui.Community.Shared
{
    public class TokenResult
    {
        /// <summary>
        ///
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime RefreshTokenExpired { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
