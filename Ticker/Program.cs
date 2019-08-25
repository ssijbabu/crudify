using System;
using System.Collections.Generic;

using KC = KiteConnect;
using Redis = StackExchange.Redis;

namespace CrudifyTicker
{
    class Program
    {
        static KC.Ticker Ticker;
        static KC.Kite Kite;

        static string Access_Token = "";
        static string Public_Token = "";
        static string API_Key = "";
        
        static UInt32 Instrument_ID = 256265;

        static void Main(string[] args)
        {            
            InitSession();
            InitTicker();
        }
        
        static string getTokenDetails(string Key)
        {
            Redis.ConnectionMultiplexer connection = Redis.ConnectionMultiplexer.Connect("localhost,password=ZLDe3wse");
            Redis.IDatabase db = connection.GetDatabase();

            Redis.RedisValue Value = db.StringGet(Key);

            return Value;
        }

        static void InitSession()
        {
            API_Key = getTokenDetails("api_key");
            Public_Token = getTokenDetails("public_token");
            Access_Token = getTokenDetails("access_token");

            if (API_Key.Trim().Length != 0 || Public_Token.Trim().Length != 0 || Access_Token.Trim().Length != 0) 
            {
                //Kite = new KC.Kite(API_Key, Debug: true);
                //Kite.SetAccessToken(Access_Token);                
                Console.WriteLine("Session successfully initiated.");
            }
            else {
                Console.WriteLine("Unable to start the session.");
            }
        }
        
        static void InitTicker()
        {
            try
            {
                Ticker = new KC.Ticker(API_Key, Access_Token);

                Ticker.OnTick += OnTick;
                Ticker.OnReconnect += OnReconnect;
                Ticker.OnNoReconnect += OnNoReconnect;
                Ticker.OnError += OnError;
                Ticker.OnClose += OnClose;
                Ticker.OnConnect += OnConnect;
                Ticker.OnOrderUpdate += OnOrderUpdate;

                Ticker.EnableReconnect(Interval: 5, Retries: 50);
                Ticker.Connect();

                // Subscribing to NIFTY50 and setting mode to LTP
                Ticker.Subscribe(Tokens: new UInt32[] { Instrument_ID });
                Ticker.SetMode(Tokens: new UInt32[] { Instrument_ID }, Mode: KC.Constants.MODE_LTP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void GetDayRange()
        {
            double high,low;

            DateTime Today = DateTime.Now;

            List<KC.Historical> historical = Kite.GetHistoricalData(
                InstrumentToken: "5633", 
                FromDate: new DateTime(Today.Year, Today.Month, Today.Day, 9, 0, 0),
                ToDate: new DateTime(Today.Year, Today.Month, Today.Day, 13, 30, 0),
                Interval: KC.Constants.INTERVAL_30MINUTE,
                Continuous: false
            );         

        }

        private static void OnTick(KC.Tick TickData)
        {
            Console.WriteLine("LTP: " + TickData.LastPrice);
        }

        private static void OnOrderUpdate(KC.Order OrderData)
        {
           // Console.WriteLine("OrderUpdate " + Utils.JsonSerialize(OrderData));
        }

        private static void OnConnect()
        {
            Console.WriteLine("Connected ticker");
        }

        private static void OnClose()
        {
            Console.WriteLine("Closed ticker");
        }

        private static void OnError(string Message)
        {
            Console.WriteLine("Error: " + Message);
        }

        private static void OnNoReconnect()
        {
            Console.WriteLine("Not reconnecting");
        }

        private static void OnReconnect()
        {
            Console.WriteLine("Reconnecting");
        }

        private static void OnTokenExpire()
        {
            Console.WriteLine("Need to login again");
        }
    }
}
