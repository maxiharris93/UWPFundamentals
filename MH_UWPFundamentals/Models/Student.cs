using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_UWPFundamentals.Models
{
    //represents a student
    internal class Student
    {
        //gets or sets the identifier
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //gets or sets the name
        [MaxLength(64)]
        public string Name { get; set; }

        //gets or sets the number
        [MaxLength(10)]
        public string Number { get; set; }

        //gets or sets the discipline
        public string Notes { get; set; }
    }
}
