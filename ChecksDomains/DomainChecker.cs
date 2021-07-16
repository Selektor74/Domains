using ChecksDomains.Checks;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ChecksDomains.Check;
using static System.Net.HttpStatusCode;


namespace ChecksDomains
{
    /// <summary>
    /// Recive Json, launch checks and return result of checks
    /// </summary>
    public class DomainChecker
    {
        private readonly List<ICheck> ListChecks = new List<ICheck>();
        private DataHandler Data = new DataHandler();
        private CheckStatusCode CheckStatusCode { get; set; }
        private CheckTitle CheckTitle { get; set; }
        private CheckRegistration CheckRegistration { get; set; }
        private CheckIpAddress CheckIpAddress { get; set; }

        private List<string> ListDomains = new List<string>();
        public DomainChecker()
        {
            CheckStatusCode = new CheckStatusCode();
            CheckTitle = new CheckTitle();
            CheckRegistration = new CheckRegistration();
            CheckIpAddress = new CheckIpAddress();
        }
        /// <summary>
        /// Run check status code
        /// </summary>
        /// <param name="Domains"></param>
        /// <returns></returns>
        public async Task RunCheckStatusCode(List<string> Domains)
        {
            foreach (var Domain in Domains)
            {
                Data.FillingDictionary(Domain, await CheckStatusCode.Check(Domain));
            }
        }
        /// <summary>
        /// Run check Title
        /// </summary>
        /// <param name="Domains"></param>
        /// <returns></returns>
        public async Task RunCheckTitle(List<string> Domains)
        {
            foreach (var Domain in Domains)
            {
                if (Data.DependenceCheck(Domain, CheckTitle.GetDependensesList()))
                {
                    Data.FillingDictionary(Domain, await CheckTitle.Check(Domain));
                }
               
            }
        }
        /// <summary>
        /// Run Check Registration
        /// </summary>
        /// <param name="Domains"></param>
        /// <returns></returns>
        public async Task RunCheckRegistration(List<string> Domains)
        {
            foreach (var Domain in Domains)
            {
                if (Data.DependenceCheck(Domain, CheckRegistration.GetDependensesList()))
                {
                    Data.FillingDictionary(Domain, await CheckRegistration.Check(Domain));
                }
              
            }
        }
        /// <summary>
        /// Run Check Ip Adress
        /// </summary>
        /// <param name="Domains"></param>
        /// <returns></returns>
        public async Task RunCheckIpAddress(List<string> Domains)
        {
            foreach (var Domain in Domains)
            {
                if (Data.DependenceCheck(Domain, CheckIpAddress.GetDependensesList()))
                {
                    Data.FillingDictionary(Domain, await CheckIpAddress.Check(Domain));
                }
               
            }
        }


        /// <summary>
        /// Launch given checks 
        /// </summary>
        /// <returns></returns>
        public async Task<string> RunAllChecks()
        {
            await RunCheckStatusCode(ListDomains);
            await RunCheckTitle(ListDomains);
            await RunCheckRegistration(ListDomains);
            await RunCheckIpAddress(ListDomains);
            var json = Data.ConvertFromDictToJson();
            return json;
        }
        /// <summary>
        /// Recive Json
        /// </summary>
        /// <param name="path"></param>
        public void TakeJson(string path)
        {
            ListDomains = Data.ConvertFromJsonToList(path);
            ListDomains = ListDomains.Select(x => Data.RemoveExcessInDomain(x)).ToList();
        }
    }
}
