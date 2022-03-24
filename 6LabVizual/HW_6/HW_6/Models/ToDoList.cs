using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_6.Models
{
    public class ToDoList
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public ToDoList(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }
    }
}