using ModelsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClaimsPoC.ClientHome.Models
{
    public class ClientClaim
    {
        public Claim Claim { get; set; }

        public string Name { get; set; }
    }
}