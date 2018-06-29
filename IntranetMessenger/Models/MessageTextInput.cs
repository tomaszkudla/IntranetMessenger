using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetMessenger.Models
{
    public class MessageTextInput
    {
        [Required(ErrorMessage = "Please type message")]
        public string MessageText { get; set; }

        [Required(ErrorMessage = "There is no message reciever")]
        public string Reciever { get; set; }
    }
}