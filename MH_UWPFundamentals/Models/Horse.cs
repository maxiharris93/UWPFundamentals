using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH_UWPFundamentals.Models
{
    //represents a horse
    internal class Horse
    {
        //gets or sets the identifier
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        //gets or sets the name
        [MaxLength(64)]
        public string Name { get; set; }

        //gets or sets the age
        public int Age { get; set; }

        //gets or sets the discipline
        [MaxLength(64)]
        public string Discipline { get; set; }

        //gets or sets the notes
        public string Notes { get; set; }

        //gets or sets the picture
        public byte[] Picture { get; set; }
    }
}
