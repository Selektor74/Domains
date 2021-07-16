using System;

namespace ChecksDomains
{
    /// <summary>
    /// Class used for storing information about checks.
    /// </summary>
    public class Check 
    {
        public string CheckName { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public  Type TypeOfCheck { get; set; }

    }

    /// <summary>
    /// Enum for status of check.
    /// </summary>
    public enum Status
    {
        Success,
        Warning,
        Error
    }
}