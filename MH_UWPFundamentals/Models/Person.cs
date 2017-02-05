using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace MH_UWPFundamentals.Models
{
    //represents a person
    internal class Person
    {
        //gets or sets the identifier
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //gets or sets the name
        [MaxLength(64)]
        public string Name { get; set; }

        //gets or sets the description
        public string Description { get; set; }

        //gets or sets the day of birth
        public DateTime DayOfBirth { get; set; }

        //gets or sets the picture
        public byte[] Picture { get; set; }
    }
}
