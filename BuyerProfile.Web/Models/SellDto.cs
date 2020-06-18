﻿using DAL.Models;
using System;

namespace BuyerProfile.Web.Models
{
    public class SellDto
    {
        public int Id { get; set; }
        public string AffiliateCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Adress2 { get; set; }
        public string Phone { get; set; }
        public string full_number { get; set; }
        public string FromUrl { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public double Price { get; set; }
        public string ProductName { get; set; }
        public string TransActionId { get; set; }
        public int Rank { get; set; }
        public string DiliveryStatus { get; set; }
        public string PayStatus { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
