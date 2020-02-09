using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model
{
    public class Kunde
    {
        [Key]
        public int KId { get; set; }

        [Display(Name = "Telefonnummer")]
        [Required(ErrorMessage = "Telefonnummer må oppgis")]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Telefonnummeret må bestå av 8 siffer")]
        public string TlfNr { get; set; }


        // Se readme.txt for kildehenvisning til regex
        [Display(Name = "E-post")]
        [Required(ErrorMessage = "E-post må oppgis")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}", ErrorMessage = "Ugyldig e-post")]
        public string Epost { get; set; }

        //Kunde kan kjøpe flere billetter
        public virtual List<Billett> KjøpteBilletter { get; set; }

    }
}