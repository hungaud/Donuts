using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Donuts.Models
{
    public class VerificationProvider
    {
        public Regex Format { get; set; }
        public string ProvidersName { get; set; }
        public PublicKey PublicKey { get; set; }
        public int VerificationProviderId { get; set; }
        
    }
}
