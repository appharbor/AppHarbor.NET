using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppHarbor
{
    public class AuthInfo
    {
        public string AccessToken { get; set; }

        /// <summary>
        /// This is optional, currently this is not used it is added for future support
        /// </summary>
        public string TokenType { get; set; }
    }
}
