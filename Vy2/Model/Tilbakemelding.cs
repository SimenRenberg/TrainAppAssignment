using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model
{
    public class Tilbakemelding
    {
        [Key]
        public int MeldingsNr { get; set; }
        public string Telefonnr { get; set; }
        public string Epost { get; set; }
        public string Tilbakemeldinger { get; set; }

    }
}