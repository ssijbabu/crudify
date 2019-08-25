﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KiteonnectUnitTest
{

    class MockServer
    {
        HttpListener httpListener = new HttpListener();
        string contentType = "";
        string responseString = "";

        public MockServer(string url, string contentType, string responseString)
        {
            this.contentType = contentType;
            this.responseString = responseString;

            httpListener.Prefixes.Add(url);
            httpListener.Start();
            Task.Run(() => HandleRequest());
        }

        public void HandleRequest()
        {
            var context = httpListener.GetContext();
            var response = context.Response;
            response.ContentType = contentType;

            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            httpListener.Stop();
        }
    }
}
