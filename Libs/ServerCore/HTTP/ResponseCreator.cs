using NetCoreServer;

namespace ServerCore.HTTP
{
    public class ResponseCreator
    {
        private HttpResponse response;
        public ResponseCreator(int status = 200) 
        {
            New(status);
        }

        public void New(int status = 200)
        {
            response = new();
            response.Clear();
            response.SetBegin(status);
        }

        public void SetHeaders(Dictionary<string, string> kv)
        {
            foreach (var item in kv)
            {
                SetHeader(item.Key, item.Value);
            }
        }

        public void SetHeader(string key, string value)
        {
            response.SetHeader(key, value);
        }

        public void SetCookie(string name, string value)
        {
            response.SetCookie(name, value);
        }

        public void SetBody(string content)
        {
            response.SetBody(content);
        }

        public void SetBody(byte[] content)
        {
            response.SetBody(content);
        }

        public void SetResponse(HttpResponse rsp)
        {
            response = rsp;
        }

        public HttpResponse GetResponse()
        {
            return response;
        }
    }
}
