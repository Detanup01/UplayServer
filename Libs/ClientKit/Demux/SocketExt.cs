using Google.Protobuf;
using Uplay.Demux;

namespace ClientKit.Demux
{
    public static class SocketExt
    {
        public static void KeepAlivePush(this Socket socket)
        {
            Push keepalive = new()
            {
                KeepAlive = new()
                {

                }
            };

            socket.SendPush(keepalive);
        }

        /// <summary>
        /// Checking if the Current version is same as the Latest
        /// </summary>
        /// <returns>True or False</returns>
        public static bool VersionCheck(this Socket socket)
        {
            socket.RequestId++;
            Req patchreq = new()
            {
                GetPatchInfoReq = new()
                {
                    PatchTrackId = "DEFAULT",
                    TestConfig = socket.TestConfig,
                    TrackType = 0
                },
                RequestId = socket.RequestId
            };
            var patchrsp = socket.SendReq(patchreq);
            if (patchrsp != null)
            {
                return socket.ClientVersion == patchrsp.GetPatchInfoRsp.LatestVersion;
            }
            return false;
        }

        public static GetPatchInfoRsp? GetPatch(this Socket socket)
        {
            socket.RequestId++;
            Req patchreq = new()
            {
                GetPatchInfoReq = new()
                {
                    PatchTrackId = "DEFAULT",
                    TestConfig = socket.TestConfig,
                    TrackType = 0
                },
                RequestId = socket.RequestId
            };
            var patchrsp = socket.SendReq(patchreq);
            if (patchrsp != null)
            {
                return patchrsp.GetPatchInfoRsp;
            }
            return null;
        }

        /// <summary>
        /// Initially pushing the Current version
        /// </summary>
        public static void PushVersion(this Socket socket)
        {
            Push versionPush = new()
            {
                ClientVersion = new()
                {
                    Version = socket.ClientVersion
                }
            };

            socket.SendPush(versionPush);
        }

        /// <summary>
        /// Trying to Authenticate with UbiTicket
        /// </summary>
        /// <param name="token">Ubi Ticket</param>
        /// <param name="KeepAlive">Keeping the Sokcet Alive</param>
        /// <returns>True or False</returns>
        public static bool Authenticate(this Socket socket, string token, bool KeepAlive = false)
        {
            socket.RequestId++;
            Req authReq = new()
            {
                RequestId = socket.RequestId,
                AuthenticateReq = new()
                {
                    ClientId = "uplay_pc",
                    SendKeepAlive = KeepAlive,
                    Token = new()
                    {
                        UbiToken = ByteString.CopyFromUtf8(token)
                    }
                }
            };
            var authRsp = socket.SendReq(authReq);
            if (authRsp == null)
            {
                return false;
            }
            socket.IsAuthed = authRsp.AuthenticateRsp.Success;
            return authRsp.AuthenticateRsp.Success;
        }
    }
}
