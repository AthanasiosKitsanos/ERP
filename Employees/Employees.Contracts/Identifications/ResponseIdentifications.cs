using System;
using System.ComponentModel.DataAnnotations;

namespace Employees.Contracts.Identifications;

public class ResponseIdentifications
{
    public class Get
    {
        public string TIN { get; set; } = string.Empty;
        public string WorKAuth { get; set; } = string.Empty;
        public string TaxInformation { get; set; } = string.Empty;
    }
}
