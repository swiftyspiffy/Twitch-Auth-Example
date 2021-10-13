using NHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TwitchAuthExample.Events;

namespace TwitchAuthExample
{
    public class WebServer
    {
        public event EventHandler<OnAuthorizationArgs> OnAuthorization;

        private HttpServer server;

        public WebServer()
        {
            server = new HttpServer();
            server.EndPoint = new IPEndPoint(IPAddress.Loopback, 80);
            server.RequestReceived += Server_RequestReceived;
        }

        public void Start()
        {
            server.Start();
        }

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
}
