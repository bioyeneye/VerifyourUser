using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VerifyourUser.API.BindingModels
{
    public class AudienceBindingModel
    {
        [Required]
        public string AppName { get; set; }
        public string DisplayName { get; set; }
    }
}
