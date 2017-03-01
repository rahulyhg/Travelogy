using DomingoBL.BlObjects;
using DomingoDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrencyConvertGateway
    {
        /// <summary>
        /// 
        /// </summary>
        internal static readonly string _FixerApiurl = "http://api.fixer.io/latest"; //"http://api.fixer.io/latest?callback=?";

        /// <summary>
        /// Get the currency exchange rate
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <param name="convertRate"></param>
        /// <returns></returns>
        public static DomingoBlError GetCurrencyExchangeRate(string fromCurrency, string toCurrency, out double convertRate)
        {
            convertRate = 0D;
            if (String.IsNullOrEmpty(fromCurrency) || String.IsNullOrEmpty(toCurrency))
            {
                return new DomingoBlError() { ErrorCode = 200, ErrorMessage = "Invalid or NULL parameters" }; ;
            }

            // return a 1 when we do not have to compare
            if(String.Compare(fromCurrency, toCurrency, false) == 0)
            {
                convertRate = 1D;
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }

            // try to look if we have already cached it
            convertRate = _GetCachedCurrencyXchangeRate(fromCurrency, toCurrency);
            if(convertRate > 0D)
            {
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" }; ;
            }            

            // if not, then call the Fixer API to get it
            try
            {
                var strJsonRequest = _SendJsonRequestToFixer();
                var _fixerCurrencyExchange = JsonConvert.DeserializeObject<FixerCurrencyExchange>(strJsonRequest);

                var fromRate = _fixerCurrencyExchange.rates[fromCurrency];
                var toRate = String.Compare(toCurrency.ToUpper(), "EUR") == 0 ? 1.0D : _fixerCurrencyExchange.rates[toCurrency];   
                
                if(fromRate > 0D)
                {
                    convertRate = toRate / fromRate;
                }

                // once we get it, save it - so we dont look again
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                { 
                    // find the record if exists
                    var cx = context.CurrencyExchanges.Where(p => p.CurrencyFrom.ToUpper() == fromCurrency.ToUpper() && p.CurrencyTo.ToUpper() == toCurrency.ToUpper()).FirstOrDefault();
                    // or else create one
                    if(cx == null)
                    {
                        cx = new CurrencyExchange();

                        cx.CurrencyFrom = fromCurrency;
                        cx.CurrencyTo = toCurrency;
                        cx.DateOfUpdate = DateTime.Now;
                        cx.XchangeRate = (decimal)convertRate;
                        context.CurrencyExchanges.Add(cx);
                    }

                    else
                    {
                        cx.XchangeRate = (decimal)convertRate;
                    }

                    // copy the values
                    
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" }; 
        }

        /// <summary>
        /// get the xchange rate from our own database
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <returns></returns>
        private static double _GetCachedCurrencyXchangeRate(string fromCurrency, string toCurrency)
        {
            try
            {
                double xRate = 0;

                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var currencyXcgObj = context.CurrencyExchanges.Where(p => p.CurrencyFrom.ToUpper() == fromCurrency.ToUpper() && p.CurrencyTo.ToUpper() == toCurrency.ToUpper()).FirstOrDefault(); 
                    if(currencyXcgObj != null)
                    {
                        if((currencyXcgObj.DateOfUpdate - DateTime.Now).TotalDays > 1)
                        {
                            return 0;
                        }

                        xRate = (double)currencyXcgObj.XchangeRate;
                    }
                }

                return xRate;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payLoad"></param>
        /// <param name="fixerApiurl"></param>
        /// <returns></returns>
        private static string _SendJsonRequestToFixer()
        {
            string returnJson = string.Empty;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(_FixerApiurl);
            WebResponse objResponse = objRequest.GetResponse();
            Stream dataStream = objResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            returnJson = reader.ReadToEnd();

            return returnJson;
        }
    }
}
