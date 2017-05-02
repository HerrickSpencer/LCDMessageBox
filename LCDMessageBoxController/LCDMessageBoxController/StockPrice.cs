using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Diagnostics;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.ServiceProcess;
using System.Text;
using System.IO.Ports;

namespace LCDMessageBoxController
{
    public class StockPrice
    {
        public string TickerSymbol { get; set; }

        public StockPrice(string tickerSymbol)
        {
            TickerSymbol = tickerSymbol;
        }

        public string GetPriceFromYahoo(string tickerSymbol)
        {
            string price;

            string url = string.Format("http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20csv%20where%20url='http%3A//download.finance.yahoo.com/d/quotes.csv?s={0}%26f=sl1'%20and%20columns='symbol%2Cprice'", tickerSymbol);
        
            try
            {
                Uri uri = new Uri(url);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                XDocument doc = XDocument.Load(resp.GetResponseStream());

                resp.Close();

                var ticker = from query in doc.Descendants("query")
                             from results in query.Descendants("results")
                             from row in query.Descendants("row")
                             let xElement = row.Element("price")
                             where xElement != null
                             select new { price = xElement.Value };

                price = ticker.First().price;
            }
            catch (Exception ex)
            {
                LCDMessageBoxController lm = new LCDMessageBoxController();
                lm.LogEvent("Exception retrieving symbol: " + tickerSymbol + " \n" + ex.Message, EventLogEntryType.Error);
                price = "Exception retrieving symbol: " + tickerSymbol;
            }

            return price;
        }

        public string GetPrice()
        {
            return GetPriceFromYahoo(TickerSymbol);
        }
    }
}
