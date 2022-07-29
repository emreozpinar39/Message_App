using System;

namespace Message_App.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string MessageDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");


        public Guid UserId { get; set; }
        public virtual Users User { get; set; }
    }
}
