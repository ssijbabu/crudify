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
        static KC.Kite kc = null;

        static void Main(string[] args)
        {
            string API_Key = "z5xy8uzid74zcvu5"; 
            string API_Secret = "yhy9sgcjflq45goxjul3c763dvgncvuw"; 
            string User_ID = "DS0416";

            kc = new KC.Kite(API_Key, Debug: true);

            //Clean up the Redis Server.
            Console.WriteLine("Setting up Redis...");
            CleanRedis();

            Console.WriteLine("Generating request token from Kite.");
            //Generate Kite Login URL.
            GenerateURL();

            //Generate Tokens
            GenerateTokens(API_Secret);
            Console.WriteLine();

            //Write all the token into cache
            WriteToCache("api_key",API_Key,DateTime.Parse("11:30 PM"));
            WriteToCache("api_secret",API_Secret,DateTime.Parse("11:30 PM"));
            WriteToCache("user_id",User_ID,DateTime.Parse("11:30 PM"));                        
        }

        static void CleanRedis()
        {
            connection = Redis.ConnectionMultiplexer.Connect("localhost,allowAdmin=true");
            
            server = connection.GetServer("localhost",6379);
            server.FlushDatabase();

            db = connection.GetDatabase();
        }

        static void WriteToCache(string Key, string Value, DateTime expires)
        {
            var expiryTimeSpan = expires.Subtract(DateTime.Now);
            if (expiryTimeSpan.Ticks < 0)
            {
                Console.WriteLine("Cannot set negative TTL value for the Key:{0} and Value:{0}", Key, Value);
            }
            else
            {
                Redis.RedisValue val = db.StringSet(Key, Value, expiryTimeSpan);
            }
        }   

        static void GenerateURL()
        {
            string Login_URL = kc.GetLoginURL();

            Console.WriteLine("Please open the browser and enter the following URL");
            Console.WriteLine(Login_URL);
        }

        static void GenerateTokens(string API_Secret)
        {
            string Request_Token, Public_Token, Access_Token;

            Console.WriteLine("Please enter the request_token:");
            Request_Token = Console.ReadLine();

            KC.User user = kc.GenerateSession(Request_Token, API_Secret);

            Public_Token = user.PublicToken;
            Access_Token = user.AccessToken;

            WriteToCache("public_token", Public_Token, DateTime.Parse("11:30 PM"));
            WriteToCache("access_token", Access_Token, DateTime.Parse("11:30 PM"));
        }             
    }
}
