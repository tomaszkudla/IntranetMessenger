﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetMessenger.Models
{
    public static class ActiveUser
    {
        public static int ID { get; set; }
        public static string Name { get; set; } = "";
        public static string Hash { get; set; }
    }
}