using System;
using System.Collections.Generic;

namespace Message_AppUI.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
