using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model
{
    public class Billett
    {
        [Key]
        public int BillettID { get; set; }
        public string BillettType { get; set; }
        public double Pris { get; set; }
        public int RuteId { get; set; }
        public double BillettTypePris { get; set; } //Brukes til å kalkulere prisen basert på billettype

        //en billett eies av en Kunde
        public virtual Kunde Kunde { get; set; }

    }
}