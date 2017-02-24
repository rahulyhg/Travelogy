using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.BlObjects
{
    // the api url 
    // http://api.fixer.io/latest?callback=?

    // sample rates json
    //{"base":"EUR","date":"2017-02-24","rates":
    //{"AUD":1.3816,"BGN":1.9558,"BRL":3.277,"CAD":1.3907,
    //"CHF":1.0649,"CNY":7.2873,"CZK":27.021,"DKK":7.4344,
    //"GBP":0.84503,"HKD":8.2341,"HRK":7.4275,"HUF":308.59,
    //"IDR":14128.0,"ILS":3.9241,"INR":70.664,"JPY":119.04,"KRW":1198.2,
    //"MXN":20.893,"MYR":4.7109,"NOK":8.8365,"NZD":1.4711,"PHP":53.255,
    //"PLN":4.3107,"RON":4.517,"RUB":61.644,"SEK":9.5188,"SGD":1.4892,
    //"THB":37.006,"TRY":3.7991,"USD":1.0609,"ZAR":13.702}}

    /// <summary>
    /// JSON object
    /// </summary>
    public class FixerCurrencyExchange
    {
        public string date { get; set; }

        public Dictionary<string, double> rates { get; set; }
    }
}
