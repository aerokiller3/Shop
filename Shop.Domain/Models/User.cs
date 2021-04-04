﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Shop.Domain.Models
{
    public class User : IdentityUser
    {
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
    }
}