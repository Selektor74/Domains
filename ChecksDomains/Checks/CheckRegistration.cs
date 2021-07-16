using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Whois;
using Whois.Models;

namespace ChecksDomains.Checks
{
    /// <summary>
    /// Checks the domain on registration
    /// </summary>
    public class CheckRegistration : ICheck
    {
        DataHandler DataHandler = new DataHandler();
        private CheckStatusCode CheckStatusCodeType = new CheckStatusCode();
        /// <summary>
        /// Sends a request to the server and receives domain registration data
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public async Task<Check> Check(string Domain)
        {
            var whois = new WhoisLookup();
            Check CheckResult;
            DataHandler Data = new DataHandler();
            try
            {
                var response = whois.Lookup(Domain);
                var InfoAboutDomain = response.ParsedResponse;
                if (InfoAboutDomain == null)
                {
                    CheckResult = Data.FillingCheck("Отсутствует информация", "Проверка регистрации", Status.Warning, GetType());
                    return CheckResult;
                }
                var Registrad = InfoAboutDomain?.Registered?.ToShortDateString() ?? "null";
                var Expiration = InfoAboutDomain?.Expiration?.Subtract(DateTime.Now).Days.ToString() ?? "null";
                var EndOfRegistration = InfoAboutDomain?.Expiration?.ToShortDateString() ?? "null";
                var NameServers = "";
                var Name = InfoAboutDomain?.Registrar?.Name ?? "null";
                if (InfoAboutDomain?.NameServers!=null)
                {
                    foreach (var NameServer in InfoAboutDomain?.NameServers)
                    {
                        NameServers += NameServer+ ". ";
                    }
                }
                var RegistryDomainId = InfoAboutDomain?.RegistryDomainId ?? "null";
                var RegistratorUrl = InfoAboutDomain?.Registrar?.Url ?? "null";
                string comment = $"Домен был зарегистрирован: {Registrad}, " +
                                 $"Окончание регистрации: {EndOfRegistration}, " +
                                 $"истекается через {Expiration} дней, " +
                                 $"Список серверов: {NameServers}, " +
                                 $"RegistryDomainId: {RegistryDomainId}, " +
                                 $"RegistratorUrl: {RegistratorUrl} " +
                                 $"RegistratorName: {Name}";
                CheckResult = Data.FillingCheck(comment, "Проверка регистрации", Status.Success, GetType());
            }
            catch (WhoisException ex)
            {
                CheckResult = Data.FillingCheck(ex.Message, "Проверка регистрации", Status.Error, GetType());
            }
            return CheckResult;
        }
        public List<ICheck> GetDependensesList()
        {
            List<ICheck> ListWithDependensesChecks = new List<ICheck>();
            ListWithDependensesChecks.Add(CheckStatusCodeType);
            return ListWithDependensesChecks;
        }

        public async Task<Check> DependencyError(string domain)
        {
            return DataHandler.FillingCheck("Ошибка или отсутствие обязательной предварительной проверки ",
                    "Проверка регистрации",
                                        Status.Error,
                                        GetType());
        }
    }
}
