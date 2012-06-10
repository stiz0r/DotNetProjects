using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackClone.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string UserID { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}