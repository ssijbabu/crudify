using System;
using KC = KiteConnect;
using Redis = StackExchange.Redis;

namespace AuthToken
{
    class Program
    {
        static Redis.ConnectionMultiplexer connection = null;
        static Redis.IDatabase db = null;
        static Redis.IServer server = null;
        
        static void Main(string[] args)
        {
            string API_Key = "z5xy8uzid74zcvu5"; 
            string API_Secret = "yhy9sgcjflq45goxjul3c763dvgncvuw"; 
            string User_ID = "DS0416";
            string Request_Token;

            //Clean up the Redis Server.
            Console.WriteLine("Setting up Redis...");
            CleanRedis();

            //Generate Kite Login URL.
            Console.WriteLine("Generating request token from Kite.");
            GenerateURL(API_Key);

            //Get Request Token
            Request_Token = GetRequestToken();
            Console.WriteLine();

            //Write all the token into cache
            WriteToCache("api_key",API_Key,DateTime.Parse("11:30 PM"));
            WriteToCache("api_secret",API_Secret,DateTime.Parse("11:30 PM"));
            WriteToCache("user_id",User_ID,DateTime.Parse("11:30 PM"));
            WriteToCache("request_token",Request_Token,DateTime.Parse("11:30 PM"));            
        }

        static void CleanRedis()
        {
            connection = Redis.ConnectionMultiplexer.Connect("localhost,password=ZLDe3wse,allowAdmin=true");
            
            server = connection.GetServer("localhost",6379);
            server.FlushDatabase();

            db = connection.GetDatabase();
        }

        static void WriteToCache(string Key, string Value, DateTime expires)
        {
            var expiryTimeSpan = expires.Subtract(DateTime.Now);
            //Console.WriteLine("DT1={0}",expires);
            //Console.WriteLine("DT2={0}",DateTime.Now);
            //Console.WriteLine("TTL={0}",expiryTimeSpan);

            Redis.RedisValue val = db.StringSet(Key,Value,expiryTimeSpan);
        }   

        static void GenerateURL(String API_Key)
        {
            KC.Kite kc = new KC.Kite(API_Key, Debug: true);
            string Login_URL = kc.GetLoginURL();

            Console.WriteLine("Please open the browser and enter the following URL");
            Console.WriteLine(Login_URL);
        }

        static string GetRequestToken()
        {
            string Request_Token;
            Console.WriteLine("Please enter the request_token:");
            Request_Token = Console.ReadLine();

            return Request_Token;
        }             
    }
}
