using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;

namespace BLL
{
    public class BLLAksess
    {
        public bool LeggTilDB(string tlfNr, string epost, string billettType, double pris, TogRute rute)
        {
            var OK = new Aksess();
            if (OK.LeggTilDB(tlfNr, epost, billettType, pris, rute))
            {
                return true;
            }
            else { return false; }
        }

        public List<TogRute> VisRute(string StartStasjon, string EndeStasjon, DateTime AvgangTid)
        {
            var Aksess = new Aksess();
            List<TogRute> rute = Aksess.VisRute(StartStasjon, EndeStasjon, AvgangTid);
            return rute;
        }

        public List<TogRute> VisReturRute(string StartStasjon, string EndeStasjon, DateTime? ReturDato)
        {
            var Aksess = new Aksess();
            List<TogRute> retur = Aksess.VisReturRute(StartStasjon, EndeStasjon, ReturDato);
            return retur;
        }

        public TogRute HentRute(int RuteId)
        {
            var Aksess = new Aksess();
            TogRute valgtRute = Aksess.HentRute(RuteId);
            return valgtRute;
        }

        public List<TogRute> HentTurReturRute(int RuteId, int ReturRuteId)
        {
            var Aksess = new Aksess();
            List<TogRute> turRetur = Aksess.HentTurReturRute(RuteId, ReturRuteId);
            return turRetur;
        }

        public List<TogRute> NesteTilGjengeligeRute(string Startstasjon, string EndeStasjon)
        {
            var Aksess = new Aksess();
            List<TogRute> nesteRute = Aksess.NesteTilGjengeligeRute(Startstasjon, EndeStasjon);
            return nesteRute;
        }

        public string HentStasjoner(string sok)
        {
            var Aksess = new Aksess();
            string resultat = Aksess.HentStasjoner(sok);
            return resultat;
        }

        public Billett HentEnBillett(string tlfNr)
        {
            var Aksess = new Aksess();
            Billett enBillett = Aksess.HentEnBillett(tlfNr);
            return enBillett;
        }

        public List<Billett> HentTurReturBillett(string tlfNr)
        {
            var Aksess = new Aksess();
            List<Billett> enBillett = Aksess.HentTurReturBillett(tlfNr);
            return enBillett;
        }

        public bool LeggTilTilbakemelding(string tilbakemelding, string epost, string tlf)
        {
            var OK = new Aksess();
            if (OK.LeggTilTilbakemelding(tilbakemelding, epost, tlf))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
