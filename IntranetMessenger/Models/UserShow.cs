using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntranetMessenger.Models
{
    public class UserShow
    {
        public int ID;
        public string Name;

        public UserShow (User user)
        {
            ID = user.ID;
            Name = user.Name;
        }
    }
}