using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchAuthExample.Models
{
    public class Authorization
    {
        public string Code { get; }
        
        public Authorization(string code)
        {
            Code = code;
        }
    }
}
