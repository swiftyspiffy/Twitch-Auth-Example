using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchAuthExample.Events
{
    public class OnAuthorizationArgs
    {
        public string Code { get; }
        
        public OnAuthorizationArgs(string code)
        {
            Code = code;
        }
    }
}
