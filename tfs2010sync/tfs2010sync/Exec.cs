using System;
using System.IO;
using System.Net;

namespace tfs2010sync
{
    class Exec
    {
        public static string GetQuery()
        {
            var request = WebRequest.Create("https://www.googleapis.com/fusiontables/v1/query?sql=SELECT%20*%20FROM%201Sr364zm8ZgOQVdWTLD4xe3nsFKQCNqpSvW1kVfA&key=AIzaSyA1P1wjBr3XAJWppVv2WN2foac6415tuaI");
            var response = request.GetResponse();

            if (response == null) throw new Exception("response is null");
            var stream = response.GetResponseStream();
            if (stream == null) throw new Exception("stream is null");
            var reader = new StreamReader(stream);
            dynamic res = new DynamicJSON(reader.ReadToEnd());
            string query;

            try
            {
                query = res.rows[0][0].Value;
            }
            catch { throw new Exception("field Value is not exists"); }
            return query;
        }

        public static void Go(string query)
        {
            var manager = new Manager();
            manager.ConnectionOpen(Connection.RMSP, "rmsprd", "golive104");

            try
            {
                manager.Call(query);
            }
            finally { manager.ConnectionClose(); }            
        }
    }
}
