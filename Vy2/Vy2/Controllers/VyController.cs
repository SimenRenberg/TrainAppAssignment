using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Vy.Controllers
{
    [RequireHttps]
    public class VyController : Controller
    {
        private IAdminBLL _adminBLL;

        public VyController()
        {
            _adminBLL = new AdminBLL();
        }

        public VyController(IAdminBLL stub)
        {
            _adminBLL = stub;
        }

        public ActionResult Index()
        {
            //så blir du ikke logget ut med en gang du går til index.
            if (Session["SuperLoggetInn"] == null)
            {
                Session["SuperLoggetInn"] = false;
                if (Session["loggetInn"] == null)
                {
                    Session["loggetInn"] = false;
                }
            }
            if (string.IsNullOrEmpty(Session["logginnFeil"] as string)) //ingen feil-login enda
            {
                return View();
            }
            else
            {
                ViewBag.feillogginn = (string)Session["logginnFeil"];
                return View();
            }
        }

        public string ajax(string sok)
        {
            var søker = new BLLAksess();
            string resultat = søker.HentStasjoner(sok);
            return resultat;
        }

        //finner passende ruter i databasen basert på kundens input (metodene er i funksjonalitet.cs) og
        //returnerer riktig actionresult, en for å vise frem tur-retur ruter, og en dersom det bare skal vises en rute.
        [HttpPost]
        public ActionResult Index(string StartStasjon, string EndeStasjon, DateTime AvgangTid, DateTime? ReturDato, string BillettType, string type)
        {
            var beregn = new Funksjonalitet();
            var funksjonalitet = new BLLAksess();

            DateTime avgangDato = AvgangTid.Date;

            //variabler som skal benyttes senere for å opprette en billett-variabel.
            Session["type"] = type;
            Session["BillettType"] = BillettType;


            /* Session["Dato"] er litt rar. C# har kun DateTime, det er derfor ikke mulig å dele opp
            attributtene i tabellen over TogRuter til datoen og klokkeslettet til avgangen. 
            alle avgangene i TogRuter-tabellen viser derfor samme dag - 06.10.19, med tre forskjellige klokkeslett
            da vi ville hente passende togruter basert på kundens input valgte vi å
            kun hente passende klokkeslett fra databasen, og "jukse" med datoen 
            slik at vi viser "avganger for <datoen kunden skrev>" i view-et.
            det er det denne variabelen sørger for. */
            Session["Dato"] = avgangDato;

            //sjekk om bruker skal en vei eller tur-retur
            switch (type)
            {
                case "enVei":
                    List<TogRute> PassendeRuter = funksjonalitet.VisRute(StartStasjon, EndeStasjon, AvgangTid);
                    PassendeRuter = beregn.BeregnPris(PassendeRuter, BillettType);
                    if (PassendeRuter.Count < 1) // kunde har spurt etter avganger senere enn kl 20:00 (grundigere kommentarer om dette i AvgangsOversiktTurRetur)
                    {
                        string DagString = avgangDato.ToString("dddd, dd MMMM yyyy");


                        // "feilmelding" til kunde
                        string beskjed = "Fant ingen ruter som går etter kl. 20:00 på " + DagString + ". Her er nærmeste tilgjengelige rute neste dag.";
                        Session["beskjed"] = beskjed;

                        PassendeRuter = funksjonalitet.NesteTilGjengeligeRute(StartStasjon, EndeStasjon);
                        PassendeRuter = beregn.BeregnPris(PassendeRuter, BillettType);
                        Session["Dato"] = avgangDato.AddDays(1); //legger til en dag på datoen kunden skrev inn for å vise korrekt dato (dagen etter) på billetten
                    }
                    //Session["pris"] = PassendeRuter[0].Pris;
                    Session["passenderuter"] = PassendeRuter;  //lagrer listen over passende ruter for å vise de i neste view

                    return RedirectToAction("AvgangsOversikt");
                case "turRetur":
                    //DateTime er vanligvis ikke null-able, så vi må sjekke om den har .Value
                    DateTime returDato = ReturDato.Value.Date;
                    List<TogRute> PassendeTilRuter = funksjonalitet.VisRute(StartStasjon, EndeStasjon, AvgangTid);
                    PassendeTilRuter = beregn.BeregnPris(PassendeTilRuter, BillettType);
                    List<TogRute> PassendeReturRuter = funksjonalitet.VisReturRute(StartStasjon, EndeStasjon, ReturDato);
                    PassendeReturRuter = beregn.BeregnPris(PassendeReturRuter, BillettType);

                    //for å overføre både rutene for tur og retur til et view opprettes en liste av lister.
                    List<List<TogRute>> PassendeTurReturRuter = new List<List<TogRute>>
                        {
                            PassendeTilRuter,
                            PassendeReturRuter
                        };

                    //dersom bruker har forespurt en eller flere togruter etter kl 20:00. 
                    //Det finnes ingen ruter som går etter 20:00 i databasen, så vi henter her første
                    //tilgjengelige togrute neste dag. Burde med stor fordel vært mulig å endre klokkeslettet 
                    //dynamisk i dette view-et, men slik løsningen vår er bygget blir bruker nødt til å gå tilbake
                    //til index dersom de ikke vil ha en togrute for neste dag.
                    if (PassendeTurReturRuter[0].Count < 1 || PassendeTurReturRuter[1].Count < 1)
                    { //hvis vi ikke har funnet togruter (skjer om de skriver inn f.eks 21:00).

                        //henter relevante Session-variabler
                        DateTime Dato = (DateTime)Session["dato"]; //dato kunde har skrevet inn
                        string DagString = Dato.ToString("dddd, dd MMMM yyyy");

                        //beskjeder til bruker
                        string beskjedReiseTil = "Fant ingen ruter som går etter kl. 20:00 på " + DagString + " \n Her er nærmeste tilgjengelige rute neste dag.";
                        string beskjedReiseTilbake = "Fant ingen retur-reiser som går etter kl 20:00 på " + DagString + ".\n Her er nærmeste tilgjengelige rute neste dag.";

                        //sjekker spesifikt hvilke ruter må endres til neste dags første tilgjengelige rute
                        if (PassendeTurReturRuter[0].Count < 1 && PassendeTurReturRuter[1].Count < 1) //dersom både til-og tilbake klokkeslettene er "ugyldige"
                        {
                            Session["Dato"] = Dato.AddDays(1); //legger til en dag på datoen kunden skrev inn for å vise korrekt dato (dagen etter) på billetten
                                                               //endre PassendeTurReturRuter til å være de nærmest tilgjengelige rutene (vi valgte kl 08:00 neste dag)
                            PassendeTurReturRuter[0] = funksjonalitet.NesteTilGjengeligeRute(StartStasjon, EndeStasjon);
                            PassendeTurReturRuter[0] = beregn.BeregnPris(PassendeTurReturRuter[0], BillettType);

                            PassendeTurReturRuter[1] = funksjonalitet.NesteTilGjengeligeRute(EndeStasjon, StartStasjon);
                            PassendeTurReturRuter[1] = beregn.BeregnPris(PassendeTurReturRuter[1], BillettType);


                            //viewbag-beskjedene instansieres ikke før if-testene returnerer true. Dette er for å sjekke om de er null i view-et.
                            //dersom f.eks ViewBag.BeskjedReiseTil != null vet vi at brukeren har valgt et tidspunkt etter det siste toget har gått,
                            //og det vil være nødvendig å vise denne beskjeden i stedet for "default"-beskjeden (Avganger for <dato kunde har valgt>)
                            Session["beskjedReiseTil"] = beskjedReiseTil;
                            Session["beskjedReiseTilbake"] = beskjedReiseTilbake;

                        }
                        else if (PassendeTurReturRuter[0].Count < 1) //dersom bare ruten TIL er "ugyldig"
                        {
                            Session["Dato"] = Dato.AddDays(1); //legger til en dag på datoen kunden skrev inn for å vise korrekt dato (dagen etter) på riktig billett
                                                               //turRetur[0] vil alltid være ruten TIL, kan da endre dette til neste tilgjengelige rute
                            PassendeTurReturRuter[0] = funksjonalitet.NesteTilGjengeligeRute(StartStasjon, EndeStasjon);
                            Session["beskjedReiseTil"] = beskjedReiseTil;

                        }
                        else if (PassendeTurReturRuter[1].Count < 1) //dersom bare ruten TILBAKE er "ugyldig"
                        {
                            Session["ReturDato"] = ReturDato.Value.AddDays(1); //legger til en dag på datoen kunden skrev inn for å vise korrekt dato (dagen etter) på riktig billett

                            //og turRetur[1] vil alltid være retur-ruten.
                            PassendeTurReturRuter[1] = funksjonalitet.NesteTilGjengeligeRute(EndeStasjon, StartStasjon);
                            Session["beskjedReiseTilbake"] = beskjedReiseTilbake;
                        }
                    }
                    Session["prisTil"] = PassendeTurReturRuter[0][0].Pris;
                    Session["prisFra"] = PassendeTurReturRuter[1][0].Pris;
                    Session["passendeTurReturRuter"] = PassendeTurReturRuter;
                    Session["returDato"] = returDato;
                    return RedirectToAction("AvgangsOversiktTurRetur");
            }
            return View();
        }

        //bruker sessions for å overføre listen over passende ruter basert på kundens ønske og vise dem i viewet
        public ActionResult AvgangsOversikt()
        {
            Session["logginnFeil"] = null;

            List<TogRute> ruter = Session["passenderuter"] as List<TogRute>;
            if (ruter == null)
            {
                Session["feil"] = "Fant ingen passende ruter. Har du skrevet inn gyldige stasjoner og tidspunkt? Prøv igjen.";
                return RedirectToAction("FeilSide");
            }
            if ((string)Session["beskjed"] != null)
            {
                ViewBag.beskjed = (string)Session["beskjed"];
            }

            DateTime avgangDato = (DateTime)Session["Dato"];
            string avgangDatoString = avgangDato.ToString("dddd, dd MMMM yyyy");
            //dato kunde skrev inn. Se den massive kommentaren for forklaring
            ViewBag.avgangDato = avgangDatoString;
            //billettobjekt er ikke opprettet enda, så vi bruker prisen som er kalkulert fra input i index
            ViewBag.pris = Session["pris"];
            return View(ruter);

        }

        //samme prosess som i AvgangsOversikt(), bare med en liste av lister, med både tur og retur-listene.
        public ActionResult AvgangsOversiktTurRetur()
        {
            Session["logginnFeil"] = null;

            //datoen kunden skrev inn, formattert til å se fin ut
            DateTime avgangDato = (DateTime)Session["Dato"];
            string avgangDatoString = avgangDato.ToString("dddd, dd MMMM yyyy");
            DateTime returDato = (DateTime)Session["returDato"];
            string returDatoString = returDato.ToString("dddd, dd MMMM yyyy");

            //kunne kanskje med fordel bare lagt passendeTurReturRuter i ViewBag, men vi var så fornøyd med 2d-arrayet vårt.
            ViewBag.turReturPris = Session["pris"];
            ViewBag.returDato = returDatoString;
            ViewBag.avgangDato = avgangDatoString;

            List<List<TogRute>> turRetur = Session["passendeTurReturRuter"] as List<List<TogRute>>;
            if (turRetur == null)
            {
                Session["feil"] = "Fant ingen passende ruter. Har du skrevet inn gyldige stasjoner og tidspunkt? Prøv igjen.";
                return RedirectToAction("FeilSide");


            }
            if ((string)Session["beskjedReiseTil"] != null)
            {
                ViewBag.beskjedReiseTil = (string)Session["beskjedReiseTil"];
            }
            if ((string)Session["beskjedReiseTilbake"] != null)
            {
                ViewBag.beskjedReiseTilbake = (string)Session["beskjedReiseTilbake"];
            }
            //returnerer listen over lister som inneholder listene over passende ruter TIL, og passende ruter FRA, 
            //med evt endrede verdier takket være if-testen som sjekker om klokkeslettet er godkjent.

            return View(turRetur);
        }

        //lagrer reisen kunden har valgt som session og sender videre til kundeInfo-viewet
        [HttpPost]
        public ActionResult HentReise(int RuteId)
        {
            Session["logginnFeil"] = null;

            var funksjonalitet = new BLLAksess();

            TogRute valgtRute = funksjonalitet.HentRute(RuteId);

            if (valgtRute == null)
            {
                Session["feil"] = "Fant ikke ruten du har valgt i vår database! Prøv igjen.";
                return RedirectToAction("FeilSide");
            }
            else
            {
                Session["valgtRute"] = valgtRute;
                return RedirectToAction("KundeInfo");
            }
        }

        //samme som HentReise
        [HttpPost]
        public ActionResult HentTurReturReise(int RuteId, int ReturRuteId)
        {
            var funksjonalitet = new BLLAksess();
            List<TogRute> turRetur = funksjonalitet.HentTurReturRute(RuteId, ReturRuteId);

            if (turRetur == null || turRetur.Count <= 1)
            {
                Session["feil"] = "Kunne ikke finne tur-retur reisen din! Prøv igjen.";
                return RedirectToAction("FeilSide");
            }
            else
            {
                Session["turRetur"] = turRetur;
                return RedirectToAction("KundeInfo");
            }
        }

        public ActionResult KundeInfo()
        {
            Session["logginnFeil"] = null;

            return View();
        }

        [HttpPost]
        public ActionResult KundeInfo(string tlfNrInn, string epost)
        {
            if (tlfNrInn == null || epost == null)
            {
                Session["feil"] = "Du har ikke fylt inn kundeinformasjonen vi trenger. Prøv igjen.";
                return RedirectToAction("FeilSide");
            }
            string tlfNr = tlfNrInn.Replace(" ", string.Empty); //formatterer slik at tlfNr går fra ### ## ### til ########. Var nødvendig for å legge til i DB.
            //lagrer tlfnr for å bruke senere for å finne og vise frem billetten(e) kunden har kjøpt
            Session["tlfnr"] = tlfNr;
            //får tak i input fra Index som er nødvendige for å opprette billettobjekt
            string billettType = (string)Session["billettType"];

            string enVeiEllerTurRetur = (string)Session["type"];

            var funksjonalitet = new BLLAksess();
            switch (enVeiEllerTurRetur)
            {
                case "turRetur":
                    List<TogRute> turRetur = (List<TogRute>)Session["turRetur"];
                    //ruten til ligger fremst i tur-returlisten
                    TogRute ruteTil = turRetur.First();
                    double prisTil = (double)Session["prisTil"];
                    //ruten fra er sist i retur-ruten (listen har alltid en count på bare 2)
                    TogRute ReturRute = turRetur.Last();
                    double prisFra = (double)Session["prisFra"];

                    //hvis til-billetten (og kunden) kan legges til så..
                    if (funksjonalitet.LeggTilDB(tlfNr, epost, billettType, prisTil, ruteTil))
                    {
                        //legges retur-billetten til samme kunde, så..
                        if (funksjonalitet.LeggTilDB(tlfNr, epost, billettType, prisFra, ReturRute))
                        {
                            //viser vi billettene
                            return RedirectToAction("BillettInfoTurRetur");
                        }
                        else
                        {
                            Session["feil"] = "Kunne ikke legge til retur-billetten din i vår database. Prøv igjen.";
                            return RedirectToAction("FeilSide");
                        }
                    }
                    else
                    {
                        Session["feil"] = "Kunne ikke legge til-billetten din i vår database. Prøv igjen.";
                        return View();
                    }
                case "enVei":
                    //hent ruten kunden valgte
                    TogRute valgtRute = (TogRute)Session["valgtRute"];
                    double pris = (double)Session["pris"];
                    //prøv å legg til kunde og billett til db
                    if (funksjonalitet.LeggTilDB(tlfNr, epost, billettType, pris, valgtRute))
                    {
                        //vis billetten
                        return RedirectToAction("BillettInfoEnVei");
                    }
                    else
                    {
                        Session["feil"] = "Kunne ikke legge til billetten i vår database. Prøv igjen.";
                        return RedirectToAction("FeilSide");
                    }
            }
            return View();
        }

        public ActionResult BillettInfoEnVei()
        {
            try
            {
                string kundensTlfNr = (string)Session["tlfnr"];
                string datoForKjøp = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime avreiseDato = (DateTime)Session["Dato"];
                string avreiseDatoString = avreiseDato.ToString("dd/MM/yyyy");
                var funksjonalitet = new BLLAksess();
                Billett nyesteBillett = funksjonalitet.HentEnBillett(kundensTlfNr); //bruker tlfnr til å finne denne kundens siste billett
                if (nyesteBillett == null)
                {
                    Session["feil"] = "Kunne ikke finne billetten din i vår database.";
                    return RedirectToAction("FeilSide");
                }

                ViewBag.datoForKjøp = datoForKjøp;
                ViewBag.avreiseDato = avreiseDatoString;
                ViewBag.billett = nyesteBillett;
                ViewBag.togRute = (TogRute)Session["valgtRute"];

                return View(nyesteBillett);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Session["feil"] = "Noe gikk galt. Vennligst prøv igjen fra startsiden.";
                return RedirectToAction("FeilSide");
            }


        }

        public ActionResult BillettInfoTurRetur()
        {
            Session["logginnFeil"] = null;

            var funksjonalitet = new BLLAksess();

            //datoer
            DateTime datoForKjøp = DateTime.Today;
            DateTime avreiseDato = (DateTime)Session["Dato"];
            DateTime returDato = (DateTime)Session["returDato"];

            //formatering av datoer
            string datoForKjøpString = datoForKjøp.ToString("dd/MM/yyyy");
            string avreiseDatoString = avreiseDato.ToString("dd/MM/yyyy");
            string returDatoString = returDato.ToString("dd/MM/yyyy");

            string kundensTlfNr = (string)Session["tlfnr"];
            List<Billett> turReturBilletter = funksjonalitet.HentTurReturBillett(kundensTlfNr); //bruker tlf til å finne denne kundens siste billeter
            if (turReturBilletter == null || turReturBilletter.Count <= 1)
            {
                Session["feil"] = "Kunne ikke hente din tur-retur billett fra vår database. Prøv igjen.";
                return RedirectToAction("FeilSide");
            }
            Billett tilBillett = turReturBilletter[turReturBilletter.Count - 2]; //billetten til er nest bakerst i listen
            Billett fraBillett = turReturBilletter.Last(); //billetten fra er bakerst i listen

            //trenger togruten kunden valgte for å vise Startstasjon, endestasjon etc på billetten.
            List<TogRute> turRetur = (List<TogRute>)Session["turRetur"];
            if (turRetur == null || turRetur.Count <= 1)
            {
                Session["feil"] = "kunne ikke hente tur-retur ruten du hadde valgt. Prøv igjen.";
                return RedirectToAction("FeilSide");
            }
            TogRute togRuteTil = turRetur[turRetur.Count - 2];
            TogRute togRuteFra = turRetur.Last();


            ViewBag.returDato = returDatoString;
            ViewBag.datoForKjøp = datoForKjøpString;
            ViewBag.avreiseDato = avreiseDatoString;
            ViewBag.tilBillett = tilBillett;
            ViewBag.fraBillett = fraBillett;
            ViewBag.togRuteTil = togRuteTil;
            ViewBag.togRuteFra = togRuteFra;
            return View();
        }


        //feilside vil oftest bli vist dersom bruker prøver å aksessere 
        //en side uten å først fylle inn nødvendige skjema.
        //f.eks kan de skrive vy.no/BillettInfo mens de er i Index, da vil 
        //try-catchen fange opp at det ikke er mulig å vise billetten som ikke eksisterer, 
        //og sender brukeren til feilsiden i stedet for å vise en null-exception.
        public ActionResult Feilside()
        {
            Session["logginnFeil"] = null;

            string feilmelding = (string)Session["feil"]; //strengen feilmelding blir endret basert på hvor feilen oppstod.
            ViewBag.feilmelding = feilmelding;
            return View();
        }

        public ActionResult Kontakt()
        {
            Session["logginnFeil"] = null;

            return View();
        }

        [HttpPost]
        public ActionResult Kontakt(string tilbakemelding, string epost, string tlfNrInn)
        {
            string tlfNr = tlfNrInn.Replace(" ", string.Empty); //formatterer slik at tlfNr går fra ### ## ### til ########. Var nødvendig for å legge til i DB.
            var funksjonalitet = new BLLAksess();
            if (funksjonalitet.LeggTilTilbakemelding(tilbakemelding, epost, tlfNr))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //en del if-tester i hvert av view-Actionresult-ene da det sjekkes på både loggetInn og SuperLoggetinn.
        //test utført
        [ValidateAntiForgeryToken]
        public ActionResult Logginn(string Epost, string Passord)
        {
            Session["loggetInn"] = _adminBLL.LoggInn(Epost, Passord);
            if ((bool)Session["loggetInn"])
            {
                Session["logginnFeil"] = null;
                Session["SuperLoggetInn"] = false;
                if (_adminBLL.FinnRolle(Epost) == "Super")
                {
                    Session["SuperLoggetInn"] = true;
                    return RedirectToAction("AdminIndex"); //adminindex som super
                }
                return RedirectToAction("AdminIndex"); //adminindex som vanlig
            }
            else
            {
                Session["logginnFeil"] = "Feil epost eller passord";
                return RedirectToAction("Index"); //index for ikke-brukere.
            }
        }

        //test utført
        public ActionResult AdminIndex()
        {
            if ((bool)Session["SuperLoggetInn"] != true)
            {
                if ((bool)Session["loggetInn"] != true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //ny cshtml fil i Shared, slik at alle 
                    //admin-viewsene har den samme headeren
                    return View("", "~/Views/Shared/_AdminLayout.cshtml");
                }
            }
            else
            {
                return View("", "~/Views/Shared/_AdminLayout.cshtml");
            }

        }
        //testet
        public ActionResult AdminRute()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            return View("", "~/Views/Shared/_AdminLayout.cshtml");
        }

        //testet
        public ActionResult Vedlikehold()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            return View("", "~/Views/Shared/_AdminLayout.cshtml");
        }

        //testet
        public ActionResult Administrativt()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            else if ((bool)Session["SuperLoggetInn"] != true)
            {
                return RedirectToAction("AdminIndex");
            }
            return View("", "~/Views/Shared/_AdminLayout.cshtml");
        }
        //test utført
        public ActionResult EndreRuteStatus()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            //VisAlleRuter() har en sjekk på om Innstilt == false;
            List<TogRute> alleAktiveRuter = _adminBLL.VisAlleRuter();
            //InstillteRuter() har såklart det motsatte.
            List<TogRute> alleInstillteRuter = _adminBLL.VisInstillteRuter();
            List<List<TogRute>> AktiveOgInstillteRuter = new List<List<TogRute>>()
            {
                alleInstillteRuter,
                alleAktiveRuter
            };
            //shared layout
            return View("", "~/Views/Shared/_AdminLayout.cshtml", AktiveOgInstillteRuter);
        }

        //test utført
        [HttpPost]
        public ActionResult InstillRute(int RuteId)
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index", "~/Views/Shared/_AdminLayout.cshtml");
            }

            _adminBLL.InstillRute(RuteId);
            return RedirectToAction("EndreRuteStatus");
        }

        //test utført
        [HttpPost]
        public ActionResult GjenopprettRute(int RuteId)
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            _adminBLL.GjenopprettRute(RuteId);
            return RedirectToAction("EndreRuteStatus");
        }

        //test utført
        public ActionResult RegistrerAdmin()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            else if ((bool)Session["SuperLoggetInn"] != true)
            {
                return RedirectToAction("AdminIndex");
            }
            List<Administrator> alleAdmins = _adminBLL.GetAlleAdministratorer();
            return View("", "~/Views/Shared/_AdminLayout.cshtml", alleAdmins);
        }

        //test utført
        [HttpPost]
        public ActionResult RegistrerAdmin(string NyEpost, string NyPassord, string Rolle)
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            else if ((bool)Session["SuperLoggetInn"] != true)
            {
                return RedirectToAction("AdminIndex");
            }

            ViewBag.melding = _adminBLL.LagAdmin(Rolle, NyEpost, NyPassord);
            List<Administrator> alleAdmins = _adminBLL.GetAlleAdministratorer();
            return View("", "~/Views/Shared/_AdminLayout.cshtml", alleAdmins);
        }

        //test utført
        public ActionResult EndreRuter()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            List<TogRute> alleRuter = _adminBLL.VisAlleRuter();
            return View("", "~/Views/Shared/_AdminLayout.cshtml", alleRuter);
        }

        //test utført
        [HttpPost]
        public bool EndreRuter(TogRute RuteMedNyeVerdier)
        {
            return _adminBLL.EndreRute(RuteMedNyeVerdier);
        }

        //testet
        public ActionResult LeggTilRute()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            ViewBag.melding = null;
            return View();
        }

        //testet
        [HttpPost]
        public ActionResult LeggTilRute(DateTime AvgangTid, DateTime AnkomstTid, string StartStasjon, string EndeStasjon, string Platform, double Pris)
        {
            TogRute nyRute = new TogRute
            {
                AvgangTid = AvgangTid,
                AnkomstTid = AnkomstTid,
                StartStasjon = StartStasjon,
                EndeStasjon = EndeStasjon,
                Platform = Platform,
                Pris = Pris,
                Instillt = false
            };

            if (_adminBLL.LeggTilRute(nyRute))
            {
                ViewBag.melding = "Ruten ble lagt til i databasen.";
                return View();

            }
            else
            {
                ViewBag.melding = "Ruten kunne ikke legges til i databasen";
                return View();
            }

        }

        //testet
        public ActionResult EndreAdmin()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            else if ((bool)Session["SuperLoggetInn"] != true)
            {
                return RedirectToAction("AdminIndex", "~/Views/Shared/_AdminLayout.cshtml");
            }
            else
            {
                var alleAdmins = _adminBLL.GetAlleAdministratorer();
                return View("", "~/Views/Shared/_AdminLayout.cshtml", alleAdmins);
            }

        }

        [HttpPost]
        public ActionResult EndreAdmin(Administrator admin)
        {
            var alleAdmins = _adminBLL.GetAlleAdministratorer();
            ViewBag.beskjed = null;
            if (_adminBLL.EndreAdmin(admin))
            {
                ViewBag.Beskjed = "Admin er endret";

                return View(alleAdmins);

            }
            else
            {
                ViewBag.Beskjed = "Noe gikk galt og adminen ble ikke endret.";
                return View(alleAdmins);
            }
        }

        //testet
        public ActionResult SlettAdmin(int AnsattNr)
        {
            _adminBLL.SlettAdmin(AnsattNr);
            return RedirectToAction("RegistrerAdmin");
        }

        //testet
        public ActionResult SeEndringer()
        {
            if ((bool)Session["loggetInn"] != true)
            {
                return RedirectToAction("Index");
            }
            List<DBLogg> alleEndringer = _adminBLL.GetEndringer();
            return View(alleEndringer);
        }



        //test utført
        public ActionResult LoggUt()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }



    }
}