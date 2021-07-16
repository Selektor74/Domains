using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChecksDomains
{ 
    /// <summary>
    /// Interface for checks
    /// </summary>
    public interface ICheck
    {
        public async Task<Check> Check(string Domain)
        {
            Check check = new Check();
            return  check;
        }
        public List<ICheck> GetDependensesList()
        {
            return new List<ICheck>();
        }
       
    }
}
