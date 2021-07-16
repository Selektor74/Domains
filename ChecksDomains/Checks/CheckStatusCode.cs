using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChecksDomains
{
    /// <summary>
    /// Makes a request to site to get status code.
    /// </summary>
    public class CheckStatusCode : ICheck
    {
        /// <summary>
        /// Makes a request for each item in list domains. The returned status code with comment and status write to dictionary.
        /// </summary>
        /// <param name="Domain"> List with domains to check</param>
        /// <returns></returns>
        public async Task<Check> Check(string Domain)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage Response;
            int StatusCode;
            Check CheckResult = new Check();
            var DataHandler = new DataHandler();
            Domain = DataHandler.InsertHttp(Domain);
            try
            {
                Response = await client.GetAsync(Domain);
                StatusCode = (int)Response.StatusCode;
                switch (StatusCode)
                {
                    case 200:
                        CheckResult = DataHandler.FillingCheck(StatusCode.ToString(),
                                     "Проверка статус кода",
                                     Status.Success, 
                                     GetType());
                        break;
                    case 302:
                        CheckResult = DataHandler.FillingCheck(StatusCode.ToString(),
                                     "Проверка статус кода",
                                     Status.Warning,
                                     GetType());
                        break;
                    case 404:
                        CheckResult = DataHandler.FillingCheck(StatusCode.ToString(),
                                     "Проверка статус кода",
                                     Status.Error,
                                     GetType());
                        break;
                }

            }
            catch (HttpRequestException e)
            {
                CheckResult = DataHandler.FillingCheck(e.Message,
                             "Проверка статус кода",
                             Status.Error,
                             GetType());

            }
            return CheckResult;
        }
    }
}
