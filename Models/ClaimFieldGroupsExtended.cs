using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
   
    public partial class ClaimFieldGroup : BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup
    {

        List<BrokingPlatformIntegrationBase.Interfaces.IClaimField> BrokingPlatformIntegrationBase.Interfaces.IClaimFieldGroup.ClaimFields
        {
            get
            {
                return this.ClaimFields.ToList<BrokingPlatformIntegrationBase.Interfaces.IClaimField>();
            }
            set
            {
            }
        }
    }
}