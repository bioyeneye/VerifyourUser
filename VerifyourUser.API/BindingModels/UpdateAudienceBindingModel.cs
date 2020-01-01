using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VerifyourUser.API.BindingModels
{
    public class UpdateAudienceBindingModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public List<string> Resources { get; set; }
        public List<string> Scope { get; set; }
        public int EmailTokenLength { get; set; }
        public int PhoneNumberTokenLength { get; set; }

    }
}
