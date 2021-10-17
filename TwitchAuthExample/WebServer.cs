using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchAuthExample.Models;

namespace TwitchAuthExample
{
    public class WebServer
    {
        private HttpListener listener;

        public WebServer(string uri)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uri);
        }

        public async Task<Models.Authorization> Listen()
        {
            listener.Start();
            return await onRequest();
        }

        private async Task<Models.Authorization> onRequest()
        {
            while(listener.IsListening)
            {
                var ctx = await listener.GetContextAsync();
                var req = ctx.Request;
                var resp = ctx.Response;

                using (var writer = new StreamWriter(resp.OutputStream))
                {
                    if (req.QueryString.AllKeys.Any("code".Contains))
                    {
                        writer.WriteLine("Authorization started! Check your application!");
                        writer.Flush();
                        return new Models.Authorization(req.QueryString["code"]);
                    }
                    else
                    {
                        writer.WriteLine("No code found in query string!");
                        writer.Flush();
                    }
                }
            }
            return null;
        }
    }

    /*
        private void Server_RequestReceived(object sender, HttpRequestEventArgs e)
        {
            using(var writer = new StreamWriter(e.Response.OutputStream))
            {
                if(e.Request.QueryString.AllKeys.Any("code".Contains))
                {
                    OnAuthorization?.Invoke(sender, new OnAuthorizationArgs(e.Request.QueryString["code"]));
                    writer.WriteLine("Authorization started! Check your application!");
                } else
                {
                    writer.WriteLine("No code found in query string!");
                }
                writer.Flush();
            }
        }
    }
    */
}
