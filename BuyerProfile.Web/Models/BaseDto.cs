﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BuyerProfile.Models
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; } = Guid.Empty;

        [Display(Name = "Create DateTime")]
        public DateTime CreateDateTime { get; set; }
    }

}
