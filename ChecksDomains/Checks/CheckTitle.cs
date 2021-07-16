
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp;

namespace ChecksDomains
{
    /// <summary>
    /// Makes a request to site to get title of this site.
    /// </summary>
    public class CheckTitle : ICheck
    {
        private CheckStatusCode CheckStatusCodeType = new CheckStatusCode();
        
        DataHandler DataHandler = new DataHandler();
        /// <summary>
        /// Makes a request to site, download html page, find title on html and write the result to Dictionary.
        /// </summary>
        /// <param name="Domain">List with domains to check</param>
        /// <returns></returns>
        public async Task<Check> Check(string Domain)
        {
            Check CheckResult = new Check();
            Domain = DataHandler.InsertHttp(Domain);
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            try
            {
                var document = await context.OpenAsync(Domain);
                var Title = document.Title;
                CheckResult = DataHandler.FillingCheck(Title,
                    "Проверка заголовка сайта",
                                        Status.Success, 
                                        GetType());
            }
            catch (WebException web)
            {
                CheckResult = DataHandler.FillingCheck(web.InnerException.Message,
                    "Проверка заголовка сайта",
                                        Status.Error, 
                                        GetType());
            }
            return CheckResult;
        }

        public async Task<Check> DependencyError(string Domain)
        {
            return DataHandler.FillingCheck("Ошибка или отсутствие обязательной предварительной проверки ",
                    "Проверка заголовка сайта",
                                        Status.Error,
                                        GetType());
        }
        public List<ICheck> GetDependensesList()
        {
            List<ICheck> ListWithDependensesChecks = new List<ICheck>();
            ListWithDependensesChecks.Add(CheckStatusCodeType);
            return ListWithDependensesChecks;
        }
    }
}
