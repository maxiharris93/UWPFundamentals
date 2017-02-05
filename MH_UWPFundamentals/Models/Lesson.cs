using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_UWPFundamentals.Models
{
    //represents a lesson
    internal class Lesson
    {
        //gets or sets the identifier
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //gets or sets the lesson date
        public DateTime LessonDate { get; set; }

        //gets or sets the cost
        public int Cost { get; set; }

        //gets or sets the focus
        [MaxLength(64)]
        public string Focus { get; set; }

        //gets or sets the notes
        public string Notes { get; set; }
    }
}
