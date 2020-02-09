using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class Funksjonalitet
    {

        //enkel måte å beregne pris på reiser. Går utifra om de er f.eks Voksen eller barn, og selve strekningen.
        public List<TogRute> BeregnPris(List<TogRute> passendeRuter, string billettype)
        {
            //og til slutt etter hva slags billett-type er valgt.
            foreach (var enRute in passendeRuter)
            {
                switch (billettype)
                {
                    case "Voksen":
                        break;
                    case "Barn":
                        enRute.Pris *= 0.5;
                        break;
                    case "Student":
                        enRute.Pris *= 0.4;
                        break;
                    case "Honnør":
                        enRute.Pris *= 0.5;
                        break;
                }
            }
            return passendeRuter;
        }
    }
}
