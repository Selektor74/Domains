using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChecksDomains.Checks
{
    /// <summary>
    /// Checks ip address in Domain
    /// </summary>
    public class CheckIpAddress : ICheck
    {
        DataHandler DataHandler = new DataHandler();
        private CheckStatusCode CheckStatusCodeType = new CheckStatusCode();
        /// <summary>
        /// Returns ip address for domain
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public async Task<Check> Check(string Domain)
        {
            Check CheckResult;
           
            var Ip = "";
            try
            {
                var ResponseBody = await Dns.GetHostAddressesAsync(Domain);
                foreach (var IpAddress in ResponseBody)
                {
                    Ip += IpAddress + ", ";
                }
                CheckResult = DataHandler.FillingCheck(Ip, "Проверка Ip адреса", Status.Success, GetType());
            }
            catch (SocketException ex)
            {
                CheckResult = DataHandler.FillingCheck(ex.Message, "Проверка Ip адреса", Status.Error, GetType());
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
