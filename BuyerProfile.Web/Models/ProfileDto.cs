using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerProfile.Web.Models
{
    public class ProfileDto
    {
        public string Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string Image { get; set; }

        [Display(Name = "ProfileImage")]
        public IFormFile Files { get; set; }



    }
}
