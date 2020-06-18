using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerProfile.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل را بدرستی وارد کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}
