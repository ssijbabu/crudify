using System;
using Xunit;
using KiteConnect;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KiteonnectUnitTest
{
    public class UnitTest
    {
        [Fact]
        public void TestSetAccessToken()
        {
            Kite kite = new Kite("apikey");
            kite.SetAccessToken("access_token");
            Assert.Throws<TokenException>(() => kite.GetPositions());
        }

        [Fact]
        public void TestProfile()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\profile.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            Profile profile = kite.GetProfile();
            Assert.Equal( "xxxyyy@gmail.com", profile.Email);
        }

        [Fact]
        public void TestPositions()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\positions.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            PositionResponse positionResponse = kite.GetPositions();
            Assert.Equal("LEADMINI17DECFUT", positionResponse.Net[0].TradingSymbol);
            Assert.Equal("GOLDGUINEA17DECFUT", positionResponse.Day[0].TradingSymbol);
        }

        [Fact]
        public void TestHoldings()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\holdings.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<Holding> holdings = kite.GetHoldings();
            Assert.Single(holdings);
        }

        [Fact]
        public void TestMargins()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\margins.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            UserMarginsResponse margins = kite.GetMargins();

            Assert.Equal(margins.Equity.Net, (decimal)1697.7);
            Assert.Equal(margins.Commodity.Net, (decimal)-8676.296);
        }

        [Fact]
        public void TestEquityMargins()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\equity_margins.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            UserMargin margin = kite.GetMargins("equity");

            Assert.Equal(margin.Net, (decimal)1812.3535);
        }

        [Fact]
        public void TestCommodityMargins()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\equity_margins.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            UserMargin margin = kite.GetMargins("commodity");

            Assert.Equal(margin.Net, (decimal)1812.3535);
        }

        [Fact]
        public void TestOHLC()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\ohlc.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            Dictionary<string, OHLC> ohlcs = kite.GetOHLC(new string[] { "408065", "NSE:INFY" });

            Assert.Equal(ohlcs["408065"].LastPrice, (decimal)966.8);
        }

        [Fact]
        public void TestLTP()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\ltp.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            Dictionary<string, LTP> ltps = kite.GetLTP(new string[] { "NSE:INFY" });

            Assert.Equal(ltps["NSE:INFY"].LastPrice, (decimal)989.2);
        }

        [Fact]
        public void TestQuote()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\quote.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            Dictionary<string, Quote> quotes = kite.GetQuote(new string[] { "NSE:INFY" });

            Assert.Equal(quotes["NSE:INFY"].LastPrice, (decimal)1034.25);
        }

        [Fact]
        public void TestOrders()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\orders.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<Order> orders = kite.GetOrders();

            Assert.Equal(90, orders[0].Price);
        }

        [Fact]
        public void TestOrderInfo()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\orderinfo.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<Order> orderhistory = kite.GetOrderHistory("171124000819854");

            Assert.Equal(100, orderhistory[0].PendingQuantity);
        }

        [Fact]
        public void TestInstruments()
        {
            string csv = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\instruments_all.csv", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "text/csv", csv);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<Instrument> instruments = kite.GetInstruments();

            Assert.Equal(instruments[0].InstrumentToken, (uint)3813889);
        }

        [Fact]
        public void TestSegmentInstruments()
        {
            string csv = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\instruments_nse.csv", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "text/csv", csv);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<Instrument> instruments = kite.GetInstruments(Constants.EXCHANGE_NSE);

            Assert.Equal(instruments[0].InstrumentToken, (uint)3813889);
        }

        [Fact]
        public void TestTrades()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\trades.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<Trade> trades = kite.GetOrderTrades("151220000000000");

            Assert.Equal("159918", trades[0].TradeId);
        }

        [Fact]
        public void TestMFSIPs()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\mf_sips.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<MFSIP> sips = kite.GetMFSIPs();

            Assert.Equal("1234", sips[0].SIPId);
        }

        [Fact]
        public void TestMFSIP()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\mf_sip.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            MFSIP sip = kite.GetMFSIPs("1234");

            Assert.Equal("1234", sip.SIPId);
        }

        [Fact]
        public void TestMFOrders()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\mf_orders.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<MFOrder> orders = kite.GetMFOrders();

            Assert.Equal("123123", orders[0].OrderId);
        }

        [Fact]
        public void TestMFOrder()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\mf_order.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            MFOrder order = kite.GetMFOrders("123123");

            Assert.Equal("123123", order.OrderId);
        }

        [Fact]
        public void TestMFHoldings()
        {
            string json = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\mf_holdings.json", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "application/json", json);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<MFHolding> holdings = kite.GetMFHoldings();

            Assert.Equal("123123/123", holdings[0].Folio);
        }

        [Fact]
        public void TestMFInstruments()
        {
            string csv = File.ReadAllText(@"C:\Users\ssijb\source\repos\ssijbabu\crudify\KiteonnectUnitTest\responses\mf_instruments.csv", Encoding.UTF8);
            MockServer ms = new MockServer("http://localhost:8080/", "text/csv", csv);
            Kite kite = new Kite("apikey", Root: "http://localhost:8080");
            List<MFInstrument> instruments = kite.GetMFInstruments();

            Assert.Equal("INF209K01157", instruments[0].TradingSymbol);
        }
    }
}
