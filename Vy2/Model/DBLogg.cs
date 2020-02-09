using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class DBLogg
    {
        [Key]
        public int EndringsId { get; set; }
        public string Tabell { get; set; }
        public string Kolonne { get; set; }
        public string ID { get; set; }
        public string GammelVerdi { get; set; }
        public string NyVerdi { get; set; }
        public DateTime DatoEndret { get; set; }
    }
}
