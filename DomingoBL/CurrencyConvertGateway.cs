using DomingoBL.BlObjects;
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

        public static DomingoBlError GetCurrencyExchangeRate(string fromCurrency, string toCurrency, out double convertRate)
        {
            convertRate = 0D;

            if (String.IsNullOrEmpty(fromCurrency) || String.IsNullOrEmpty(toCurrency))
            {
                return new DomingoBlError() { ErrorCode = 200, ErrorMessage = "Invalid or NULL parameters" }; ;
            }

            try
            {
                var strJsonRequest = _SendJsonRequestToFixer();
                var _fixerCurrencyExchange = JsonConvert.DeserializeObject<FixerCurrencyExchange>(strJsonRequest);

                var fromRate = _fixerCurrencyExchange.rates[fromCurrency];
                var toRate = _fixerCurrencyExchange.rates[toCurrency];   
                
                if(fromRate > 0D)
                {
                    convertRate = toRate / fromRate;
                }             
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" }; ;
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
