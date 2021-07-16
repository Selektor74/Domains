using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AngleSharp.Text;

namespace ChecksDomains
{
    /// <summary>
    /// Class of convertation data
    /// </summary>
    public class DataHandler
    {
        private Dictionary<string, List<Check>> DictionaryWithResults = new Dictionary<string, List<Check>>();
        private Dictionary<ICheck, List<ICheck>> DictWithDependences = new Dictionary<ICheck, List<ICheck>>();
        /// <summary>
        /// Convert from dictionary (with results of checks) to json.
        /// </summary>
        /// <returns></returns>
        public string ConvertFromDictToJson()
        {
            var json = JsonConvert.SerializeObject(DictionaryWithResults);
            
            return json;
        }
        /// <summary>
        /// Takes json file from path and convert to List
        /// </summary>
        /// <param name="path">Path where located json file with domains</param>
        /// <returns></returns>
        public List<string> ConvertFromJsonToList(string path)
        {
            var Text = File.ReadAllText(path);
            var Json = JsonConvert.DeserializeObject<Domain>(Text);
            return Json.domains.ToList();
        }

        /// <summary>
        /// Filling information about check
        /// </summary>
        /// <param name="Comment"></param>
        /// <param name="NameOfCheck"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public Check FillingCheck(string Comment, string NameOfCheck, Status Status,Type TypeOfCheck)
        {
            var CheckStatusCode = new Check()
            {
                Comment = Comment,
                CheckName = NameOfCheck,
                Status = Status.ToString(),
                TypeOfCheck=TypeOfCheck

            };
            return CheckStatusCode;
        }

        /// <summary>
        /// Check first this check for domain or not and write results to dict.
        /// </summary>
        /// <param name="KeyDomain">Domain that was checked</param>
        /// <param name="check">Result of check</param>
        public void FillingDictionary(string KeyDomain,
                                          Check check)
        {
            if (DictionaryWithResults.ContainsKey(KeyDomain))
            {
                DictionaryWithResults[KeyDomain].Add(check);
            }
            else
            {
                var ListChecks = new List<Check>();
                ListChecks.Add(check);
                DictionaryWithResults.Add(KeyDomain, ListChecks);
            }
        }
        /// <summary>
        /// Method check site status  and if status =="Success" allow to continue
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
      
        public bool DependenceCheck(string domain,List<ICheck> Dependenses)
        {
           
            if(DictionaryWithResults.ContainsKey(domain))
            {
                List<Check> CheckList = DictionaryWithResults[domain].ToList();
                //.Where(c => c.GetType() == CheckName)
                //.FirstOrDefault();
                foreach (var item in CheckList)
                {
                    if(Dependenses.Where(x=>x.GetType()==item.TypeOfCheck).FirstOrDefault()!=null)
                    {
                        if (item.Status == "Success")
                        {
                            return true;
                        }
                        
                    }
                   
                }
                 return false;
            }
            else return false;

        }

        /// <summary>
        /// Removes unnecessary content from the domain name
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public string RemoveExcessInDomain(string Domain)
        {
            if (Domain.Contains("https://"))
            {
                Domain= Domain.Replace("https://", "");
            }
            if(Domain.Contains("www."))
            {
                Domain = Domain.Replace("www.", "");
            }
            if (Domain.Contains("http://"))
            {
                Domain = Domain.Replace("http://", "");
            }
            if (Domain.Contains("/"))
            {
                Domain = Domain.TrimEnd("/".ToCharArray());
            }
            return Domain;


        }
        /// <summary>
        /// Insert "http://" in adrress Site
        /// </summary>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public string InsertHttp(string Domain)
        {
            string http = "http://";
            return Domain = Domain.Insert(0, http);
        }
    }
}
