using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerProfile.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "لطفا ایمیل را بدرستی وارد کنید")]
        [Required(ErrorMessage = "لطفا فیلد {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا فیلد {0} را وارد کنید")]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
