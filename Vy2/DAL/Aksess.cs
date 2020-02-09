using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Model;

namespace DAL
{
    public class Aksess
    {
        ErrorLogg loggTilFil = new ErrorLogg();

        public bool LeggTilDB(string tlfNr, string epost, string billettType, double pris, TogRute rute)
        {
            using (var db = new DB())
            {
                //billett opprettes uansett
                Billett billett = new Billett
                {
                    BillettType = billettType,
                    Pris = pris
                };

                Kunde funnetKunde = db.Kunder.FirstOrDefault(k => k.TlfNr == tlfNr);
                if (funnetKunde == null) //kunden finnes ikke fra før
                {
                    Kunde nyKunde = new Kunde()
                    {
                        TlfNr = tlfNr,
                        Epost = epost
                    };

                    //legg til billetten
                    nyKunde.KjøpteBilletter = new List<Billett>
                    {
                        billett
                    };
                    billett.Kunde = nyKunde;
                    billett.RuteId = rute.RuteId;
                    try
                    {
                        db.Kunder.Add(nyKunde);
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception feil)
                    {
                        loggTilFil.SkrivTilFil(feil);
                        return false;
                    }
                }
                else //kunden finnes
                {
                    try
                    {
                        billett.Kunde = funnetKunde;
                        billett.RuteId = rute.RuteId;
                        funnetKunde.KjøpteBilletter.Add(billett);

                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception feil)
                    {
                        loggTilFil.SkrivTilFil(feil);
                        return false;
                    }
                }
            }
        }

        //finner ruter basert på kundens ønske i Index.cshtml
        public List<TogRute> VisRute(string StartStasjon, string EndeStasjon, DateTime AvgangTid)
        {
            using (var db = new DB())
            {
                try
                {
                    List<TogRute> PassendeRuter = db.TogRuter.Where(r => r.Instillt == false).Where(r => r.AvgangTid.Hour > AvgangTid.Hour).Where(r => r.StartStasjon == StartStasjon).Where(r => r.EndeStasjon == EndeStasjon).ToList();
                    return PassendeRuter;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }

        //finner retur-ruter basert på kundens ønske i Index.cshtml, dersom de vil ha tur-retur
        //måtte ha en egen metode for retur-ruten pga DateTime? (nullable datetime)
        public List<TogRute> VisReturRute(string StartStasjon, string EndeStasjon, DateTime? ReturDato)
        {
            using (var db = new DB())
            {
                try
                {
                    List<TogRute> PassendeReturRuter = db.TogRuter.Where(r => r.Instillt == false).Where(r => r.AvgangTid.Hour > ReturDato.Value.Hour).Where(r => r.StartStasjon == EndeStasjon).Where(r => r.EndeStasjon == StartStasjon).ToList();
                    return PassendeReturRuter;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }

        public TogRute HentRute(int RuteId)
        {
            using (var db = new DB())
            {
                TogRute valgtRute = db.TogRuter.FirstOrDefault(r => r.RuteId == RuteId);
                if (valgtRute == null)
                {
                    return null;
                }
                return valgtRute;
            }
        }

        public List<TogRute> HentTurReturRute(int RuteId, int ReturRuteId)
        {
            using (var db = new DB())
            {
                TogRute valgtRuteTil = db.TogRuter.FirstOrDefault(r => r.RuteId == RuteId);
                TogRute valgtReturRute = db.TogRuter.FirstOrDefault(r => r.RuteId == ReturRuteId);
                List<TogRute> turRetur = new List<TogRute>
                {
                    valgtRuteTil,
                    valgtReturRute
                };
                if (turRetur == null || turRetur.Count <= 1)
                {
                    return null;
                }
                return turRetur;
            }
        }

        public List<TogRute> NesteTilGjengeligeRute(string Startstasjon, string EndeStasjon)
        {
            using (var db = new DB())
            {
                DateTime klÅtte = new DateTime(2019, 10, 6, 08, 00, 00);
                List<TogRute> ruter = db.TogRuter.Where(r => r.StartStasjon == Startstasjon && r.EndeStasjon == EndeStasjon).Where(r => r.AvgangTid.Hour == klÅtte.Hour).ToList();

                return ruter;
            }
        }

        //Henter stasjonsnavn i stasjoner-tabellen. Disse stasjonene er de kunden per dags dato har mulighet å reise mellom.
        //vi henter navnene i databasen for å auto-complete søk i index-siden.
        public string HentStasjoner(string sok)
        {
            using (var db = new DB())
            {
                List<Stasjoner> stasjoner = db.Stasjoner.Where(s => s.StasjonsNavn.StartsWith(sok)).ToList();
                var alleStasjoner = new List<string>();
                foreach (Stasjoner stasjon in stasjoner)
                {
                    alleStasjoner.Add(stasjon.StasjonsNavn);
                }
                var json = new JavaScriptSerializer();
                db.SaveChanges();
                string resultat = json.Serialize(alleStasjoner);

                return resultat;
            }
        }

        public Billett HentEnBillett(string tlfNr)
        {
            using (var db = new DB())
            {
                //vi hadde problemer med using(), fordi da vi skulle vise frem billettene i billettinfo-viewene var koblingen til 
                //databasen lukket. Include(), som sier hvilke objekter som skal inkluderes i spørringsresultatet stoppet dette fra å skje
                //kommenter gjerne i rettingen om det finnes en "bedre" løsning!
                Billett kundensSisteBillett = db.Billetter.Include("Kunde").ToList().Last(b => b.Kunde.TlfNr == tlfNr);
                return kundensSisteBillett;
            }
        }

        public List<Billett> HentTurReturBillett(string tlfNr)
        {
            using (var db = new DB())
            {
                //samme bruk av using() som i HentBillett().
                List<Billett> kundensBilletter = db.Billetter.Include("Kunde").Where(b => b.Kunde.TlfNr == tlfNr).ToList();
                Billett kundensTilBillett = kundensBilletter[kundensBilletter.Count - 2];
                Billett kundensFraBillett = kundensBilletter.Last();
                List<Billett> turRetur = new List<Billett>
                {
                    kundensTilBillett,
                    kundensFraBillett
                };
                return turRetur;
            }
        }

        public bool LeggTilTilbakemelding(string tilbakemelding, string epost, string tlf)
        {
            using (var db = new DB())
            {

                Tilbakemelding nyTilbakemelding = new Tilbakemelding()
                {
                    Tilbakemeldinger = tilbakemelding,
                    Epost = epost,
                    Telefonnr = tlf
                };
                try
                {
                    db.Tilbakemeldinger.Add(nyTilbakemelding);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return false;
                }
            }
        }


    }
}
