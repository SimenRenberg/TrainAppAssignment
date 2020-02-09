using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model
{
    public class Administrator
    {
        [Key]
        public int AnsattNr { get; set; }

        public string Rolle { get; set; }

        [Display(Name = "E-post")]
        [Required(ErrorMessage = "E-post må oppgis")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", ErrorMessage = "Ugyldig e-post")]
        public string Epost { get; set; }


        public byte[] Passord { get; set; }

        public byte[] Salt { get; set; }
    }
}
