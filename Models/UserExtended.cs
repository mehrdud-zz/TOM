using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ModelsLayer
{
    [MetadataType(typeof(ModelsLayer.UserMetadata))]
    public partial class User
    {
        public override string ToString()
        {
            return this.UserName;
        }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password
        {
            get;
            set;
        }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ComparePassword
        {
            get;
            set;
        }

        public aspnet_Membership Membership { get; set; }
    }



    class UserMetadata
    {
        [Display(Name = "Client")]
        [Required]
        public Nullable<int> ClientID { get; set; }


        [Required]
        public Nullable<int> UserName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}