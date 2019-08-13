using System.Collections.Generic;

namespace KiteConnect
{
    public interface IWebSocket
    {
        event OnConnectHandler OnConnect;
        event OnCloseHandler OnClose;
        event OnDataHandler OnData;
        event OnErrorHandler OnError;
        bool IsConnected();
        void Connect(string Url, Dictionary<string, string> headers = null);
        void Send(string Message);
        void Close(bool Abort = false);
    }
}
