using ModdableWebServer;
using ModdableWebServer.Helper;
using NetCoreServer;
using System.Net.Sockets;

namespace ServerCore;

internal class NewCustomWssSession : NewWssSession
{
    internal WebSocketStruct ws_Struct;
    public NewCustomWssSession(NewServer server) : base(server)
    {
    }

    #region Overrides
    protected override void OnReceivedRequest(HttpRequest request)
    {
        if (request.Method == "GET" && !request.Url.Contains("?") && this.Cache.FindPath(request.Url))
        {
            var cache = this.Cache.Find(request.Url);
            // Check again to make sure.
            if (cache.Item1)
                this.SendAsync(cache.Item2);
        }

        ServerStruct serverStruct = new()
        {
            WSS_Session = this,
            Response = this.Response,
            Enum = ServerEnum.WSS
        };

        //  this could check
        //  if (request.GetHeaders()["upgrade"] == "websocket" && request.GetHeaders()["connection"] == "Upgrade")
        if (request.GetHeaders().ContainsValue("websocket"))
        {
            DebugPrinter.Debug("[WssSession.OnReceivedRequest] websocket Value on Headers Send back to base!");
            base.OnReceivedRequest(request);
            return;
        }

        bool IsSent = serverStruct.SendRequestHTTP(request, NewServer.HTTP_AttributeToMethods);
        bool IsSent_header = serverStruct.SendRequestHTTPHeader(request, NewServer.HeaderAttributeToMethods);

        DebugPrinter.Debug($"[WssSession.OnReceivedRequest] Request sent! Normal? {IsSent} Header? {IsSent_header} ");

        if (!IsSent && !IsSent_header)
            NewServer.ReceivedFailed?.Invoke(this, request);

        if (NewServer.DoReturn404IfFail && (!IsSent && !IsSent_header))
            SendResponse(Response.MakeErrorResponse(404));

    }

    public override void OnWsConnected(HttpRequest request)
    {
        ws_Struct = new()
        {
            IsConnecting = false,
            IsConnected = true,
            IsClosed = false,
            Request = new()
            {
                Body = request.Body,
                Url = request.Url,
                Headers = request.GetHeaders()
            },
            WSRequest = null,
            Enum = WSEnum.WSS,
            WS_Session = null,
            WSS_Session = this
        };
        DebugPrinter.Debug("[WssSession.OnWsConnected] Request sent!");
        ws_Struct.SendRequestWS(NewServer.WS_AttributeToMethods);
    }

    public override void OnWsConnecting(HttpRequest request)
    {
        ws_Struct = new()
        {
            IsConnecting = true,
            IsConnected = false,
            IsClosed = false,
            Request = new()
            {
                Body = request.Body,
                Url = request.Url,
                Headers = request.GetHeaders()
            },
            WSRequest = null,
            Enum = WSEnum.WSS,
            WS_Session = null,
            WSS_Session = this
        };
        DebugPrinter.Debug("[WssSession.OnWsConnecting] Request sent!");
        ws_Struct.SendRequestWS(NewServer.WS_AttributeToMethods);
    }

    public override bool OnWsConnecting(HttpRequest request, HttpResponse response)
    {
        ws_Struct = new()
        {
            IsConnecting = true,
            IsConnected = false,
            IsClosed = false,
            Request = new()
            {
                Body = request.Body,
                Url = request.Url,
                Headers = request.GetHeaders()
            },
            WSRequest = null,
            Enum = WSEnum.WSS,
            WS_Session = null,
            WSS_Session = this
        };
        DebugPrinter.Debug("[WssSession.OnWsConnecting] Request sent!");
        ws_Struct.SendRequestWS(NewServer.WS_AttributeToMethods);
        return base.OnWsConnecting(request, response);
    }


    public override void OnWsReceived(byte[] buffer, long offset, long size)
    {
        ws_Struct.WSRequest = new(buffer, offset, size);
        DebugPrinter.Debug("[WssSession.OnWsReceived] Request sent!");
        ws_Struct.SendRequestWS(NewServer.WS_AttributeToMethods);
    }

    public override void OnWsDisconnected()
    {
        ws_Struct.IsConnecting = false;
        ws_Struct.IsConnected = false;
        ws_Struct.IsClosed = true;
        ws_Struct.WSRequest = null;
        DebugPrinter.Debug("[WssSession.OnWsDisconnected] Request sent!");
        ws_Struct.SendRequestWS(NewServer.WS_AttributeToMethods);
    }

    public override void OnWsError(string error)
    {
        NewServer.WSError?.Invoke(this, error);
    }

    protected override void OnReceivedRequestError(HttpRequest request, string error) => NewServer.ReceivedRequestError?.Invoke(this, (request, error));

    public override void OnError(SocketError error) => NewServer.OnSocketError?.Invoke(this, error);
    #endregion
}
