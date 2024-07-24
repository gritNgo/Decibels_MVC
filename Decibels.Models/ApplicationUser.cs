using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decibels.Models
{
    // custom extension of IdentityUser
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        // Discriminator column found in database will have a value that will tell whether the User on that record is an ApplicationUser or an IdentityUser
        public string? Street { get; set; }
        public string? City{ get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

    }
}
