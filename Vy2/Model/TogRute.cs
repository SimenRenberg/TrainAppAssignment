using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model
{
    public class TogRute
    {
        [Key]
        public int RuteId { get; set; }
        public DateTime AvgangTid { get; set; }
        public DateTime AnkomstTid { get; set; }
        public string StartStasjon { get; set; }
        public string EndeStasjon { get; set; }
        public string Platform { get; set; }
        public double Pris { get; set; }
        public bool Instillt { get; set; }
    }
}