using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model
{
    public class Stasjoner
    {
        [Key]
        public int StasjonsId { get; set; }
        public string StasjonsNavn { get; set; }

    }
}