using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlexaSkill.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(256)]
        [Column("ExternalId")]
        public string ExternalId { get; set; }
    }
}
