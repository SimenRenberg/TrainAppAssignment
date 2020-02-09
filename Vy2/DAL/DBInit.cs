using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Model;
namespace DAL
{
    //fyller databasen med stasjonsnavnene bruker kan velge mellom i Index.cshtml, og legger inn de forskjellige
    //rutene som det er mulig å velge mellom i vår applikasjon. Det går tog mellom alle disse strekningene 3 ganger om dagen.
    //kl 08:00, 14:00 og 20:00. Datoen er irrelevant da vi velger å "jukse" med datoen i AvgangsOversikt.cshtml, ved å bruke 
    //datoen kunden skrev inn selv. ("Datoer for avganger <datoen kunden skrev inn>", i stedet for "Datoer for avganger <dato i database>")
    //I en "ekte" webapplikasjon ville det selvfølgelig blitt gjort slik at datoen for reisen var riktig i databasen, men i denne løsningen
    //går "egentlig" alle togene bare 6. oktober 2019.

    public class DBInit<T> : DropCreateDatabaseIfModelChanges<DB>
    {
        //Vi måtte her ha disse metodene her også fordi DAL ikke har en referanse til BLL og fant da ikke disse metodene i AdminBLL.
        //Disse er da helt like som de i AdminBLL.
        public static byte[] LagHash(string innPassord, byte[] innSalt)
        {
            const int Lengde = 24;
            var hash = new Rfc2898DeriveBytes(innPassord, innSalt, 50);
            return hash.GetBytes(Lengde);
        }

        public static byte[] LagSalt()
        {
            var Randomnumber = new RNGCryptoServiceProvider(); // Random number generator
            var Salt = new byte[24];
            Randomnumber.GetBytes(Salt);
            return Salt;
        }
        protected override void Seed(DB context)
        {
            string[] stasjonsnavn = new string[] {
            "Oslo sentralstasjon",
            "Trondheim sentralstasjon",
            "Bergen jernbanestasjon",
            "Kristiansand stasjon",
            "Bodø stasjon"
        };
            for (int i = 0; i < stasjonsnavn.Length; i++)
            {
                var nyStasjon = new Stasjoner()
                {
                    StasjonsNavn = stasjonsnavn[i]
                };
                context.Stasjoner.Add(nyStasjon);

            }

            byte[] salt = LagSalt();
            Administrator superAdmin = new Administrator()
            {
                Rolle = "Super",
                Epost = "Super@Admin.no",
                Passord = LagHash("admin", salt),
                Salt = salt
            };
            context.Administratorer.Add(superAdmin);


            //Oslo
            TogRute OsloTrondheim0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 15, 00, 00),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(OsloTrondheim0800);

            TogRute OsloTrondheim1400 = new TogRute()
            {
                AvgangTid = OsloTrondheim0800.AvgangTid.AddHours(6),
                AnkomstTid = OsloTrondheim0800.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo Sentralstasjon",
                EndeStasjon = "Trondheim Sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(OsloTrondheim1400);

            TogRute OsloTrondheim2000 = new TogRute()
            {
                AvgangTid = OsloTrondheim1400.AvgangTid.AddHours(6),
                AnkomstTid = OsloTrondheim1400.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo Sentralstasjon",
                EndeStasjon = "Trondheim Sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(OsloTrondheim2000);

            TogRute OsloBergen0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 15, 00, 00),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(OsloBergen0800);

            TogRute OsloBergen1400 = new TogRute()
            {
                AvgangTid = OsloBergen0800.AvgangTid.AddHours(6),
                AnkomstTid = OsloBergen0800.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(OsloBergen1400);

            TogRute OsloBergen2000 = new TogRute()
            {
                AvgangTid = OsloBergen1400.AvgangTid.AddHours(6),
                AnkomstTid = OsloBergen1400.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(OsloBergen2000);

            TogRute OsloKristiansand0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 13, 00, 00),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(OsloKristiansand0800);

            TogRute OsloKristiansand1400 = new TogRute()
            {
                AvgangTid = OsloKristiansand0800.AvgangTid.AddHours(6),
                AnkomstTid = OsloKristiansand0800.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(OsloKristiansand1400);

            TogRute OsloKristiansand2000 = new TogRute()
            {
                AvgangTid = OsloKristiansand1400.AvgangTid.AddHours(6),
                AnkomstTid = OsloKristiansand1400.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(OsloKristiansand2000);

            TogRute OsloBodø0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 7, 01, 00, 00),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(OsloBodø0800);

            TogRute OsloBodø1400 = new TogRute()
            {
                AvgangTid = OsloBodø0800.AvgangTid.AddHours(6),
                AnkomstTid = OsloBodø0800.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1200,
                Instillt = false
            };
            context.TogRuter.Add(OsloBodø1400);

            TogRute OsloBodø2000 = new TogRute()
            {
                AvgangTid = OsloBodø1400.AvgangTid.AddHours(6),
                AnkomstTid = OsloBodø1400.AnkomstTid.AddHours(6),
                StartStasjon = "Oslo sentralstasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1200,
                Instillt = false
            };
            context.TogRuter.Add(OsloBodø2000);


            //Trondheim
            TogRute TrondheimOslo0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 15, 00, 00),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimOslo0800);

            TogRute TrondheimOslo1400 = new TogRute()
            {
                AvgangTid = TrondheimOslo0800.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimOslo0800.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim Sentralstasjon",
                EndeStasjon = "Oslo Sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimOslo1400);

            TogRute TrondheimOslo2000 = new TogRute()
            {
                AvgangTid = TrondheimOslo1400.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimOslo1400.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim Sentralstasjon",
                EndeStasjon = "Oslo Sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimOslo2000);


            TogRute TrondheimBergen0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 17, 00, 00),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimBergen0800);

            TogRute TrondheimBergen1400 = new TogRute()
            {
                AvgangTid = TrondheimBergen0800.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimBergen0800.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimBergen1400);

            TogRute TrondheimBergen2000 = new TogRute()
            {
                AvgangTid = TrondheimBergen1400.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimBergen1400.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimBergen2000);


            TogRute TrondheimKristiansand0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 21, 00, 00),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimKristiansand0800);

            TogRute TrondheimKristiansand1400 = new TogRute()
            {
                AvgangTid = TrondheimKristiansand0800.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimKristiansand0800.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimKristiansand1400);

            TogRute TrondheimKristiansand2000 = new TogRute()
            {
                AvgangTid = TrondheimKristiansand1400.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimKristiansand1400.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimKristiansand2000);


            TogRute TrondheimBodø0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 17, 00, 00),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1000,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimBodø0800);


            TogRute TrondheimBodø1400 = new TogRute()
            {
                AvgangTid = TrondheimBodø0800.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimBodø0800.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1000,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimBodø1400);


            TogRute TrondheimBodø2000 = new TogRute()
            {
                AvgangTid = TrondheimBodø1400.AvgangTid.AddHours(6),
                AnkomstTid = TrondheimBodø1400.AnkomstTid.AddHours(6),
                StartStasjon = "Trondheim sentralstasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1000,
                Instillt = false
            };
            context.TogRuter.Add(TrondheimBodø2000);


            //Bergen
            TogRute BergenOslo0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 15, 00, 00),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(BergenOslo0800);

            TogRute BergenOslo1400 = new TogRute()
            {
                AvgangTid = BergenOslo0800.AvgangTid.AddHours(6),
                AnkomstTid = BergenOslo0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(BergenOslo1400);

            TogRute BergenOslo2000 = new TogRute()
            {
                AvgangTid = BergenOslo1400.AvgangTid.AddHours(6),
                AnkomstTid = BergenOslo1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(BergenOslo2000);

            TogRute BergenTrondheim0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 17, 00, 00),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(BergenTrondheim0800);

            TogRute BergenTrondheim1400 = new TogRute()
            {
                AvgangTid = BergenTrondheim0800.AvgangTid.AddHours(6),
                AnkomstTid = BergenTrondheim0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(BergenTrondheim1400);

            TogRute BergenTrondheim2000 = new TogRute()
            {
                AvgangTid = BergenTrondheim1400.AvgangTid.AddHours(6),
                AnkomstTid = BergenTrondheim1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "2",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(BergenTrondheim2000);

            TogRute BergenKristiansand0800 = new TogRute
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 20, 00, 00),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 600,
                Instillt = false
            };
            context.TogRuter.Add(BergenKristiansand0800);

            TogRute BergenKristiansand1400 = new TogRute
            {
                AvgangTid = BergenKristiansand0800.AvgangTid.AddHours(6),
                AnkomstTid = BergenKristiansand0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 600,
                Instillt = false
            };
            context.TogRuter.Add(BergenKristiansand1400);

            TogRute BergenKristiansand2000 = new TogRute
            {
                AvgangTid = BergenKristiansand1400.AvgangTid.AddHours(6),
                AnkomstTid = BergenKristiansand1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "3",
                Pris = 600,
                Instillt = false
            };
            context.TogRuter.Add(BergenKristiansand2000);

            TogRute BergenBodø0800 = new TogRute
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 7, 09, 00, 00),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1100,
                Instillt = false
            };
            context.TogRuter.Add(BergenBodø0800);

            TogRute BergenBodø1400 = new TogRute
            {
                AvgangTid = BergenBodø0800.AvgangTid.AddHours(6),
                AnkomstTid = BergenBodø0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1100,
                Instillt = false
            };
            context.TogRuter.Add(BergenBodø1400);

            TogRute BergenBodø2000 = new TogRute
            {
                AvgangTid = BergenBodø1400.AvgangTid.AddHours(6),
                AnkomstTid = BergenBodø1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bergen jernbanestasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1100,
                Instillt = false
            };
            context.TogRuter.Add(BergenBodø2000);

            //Kristiansand
            TogRute KristiansandOslo0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 13, 00, 00),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Oslo Sentralstasjon",
                Platform = "1",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandOslo0800);

            TogRute KristiansandOslo1400 = new TogRute()
            {
                AvgangTid = KristiansandOslo0800.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandOslo0800.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Oslo Sentralstasjon",
                Platform = "1",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandOslo1400);

            TogRute KristiansandOslo2000 = new TogRute()
            {
                AvgangTid = KristiansandOslo1400.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandOslo1400.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Oslo Sentralstasjon",
                Platform = "1",
                Pris = 500,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandOslo2000);

            TogRute KristiansandBergen0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 20, 00, 00),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 600,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandBergen0800);

            TogRute KristiansandBergen1400 = new TogRute()
            {
                AvgangTid = KristiansandBergen0800.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandBergen0800.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 600,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandBergen1400);

            TogRute KristiansandBergen2000 = new TogRute()
            {
                AvgangTid = KristiansandBergen1400.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandBergen1400.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "2",
                Pris = 600,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandBergen2000);

            TogRute KristiansandTrondheim0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 21, 00, 00),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "3",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandTrondheim0800);

            TogRute KristiansandTrondheim1400 = new TogRute()
            {
                AvgangTid = KristiansandTrondheim0800.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandTrondheim0800.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "3",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandTrondheim1400);

            TogRute KristiansandTrondheim2000 = new TogRute()
            {
                AvgangTid = KristiansandTrondheim1400.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandTrondheim1400.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "3",
                Pris = 700,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandTrondheim2000);

            TogRute KristiansandBodø0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 7, 08, 00, 00),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1500,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandBodø0800);

            TogRute KristiansandBodø1400 = new TogRute()
            {
                AvgangTid = KristiansandBodø0800.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandBodø0800.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1500,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandBodø1400);

            TogRute KristiansandBodø2000 = new TogRute()
            {
                AvgangTid = KristiansandBodø1400.AvgangTid.AddHours(6),
                AnkomstTid = KristiansandBodø1400.AnkomstTid.AddHours(6),
                StartStasjon = "Kristiansand stasjon",
                EndeStasjon = "Bodø stasjon",
                Platform = "4",
                Pris = 1500,
                Instillt = false
            };
            context.TogRuter.Add(KristiansandBodø2000);


            //Bodø
            TogRute BodøOslo0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 7, 01, 00, 00),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 1200,
                Instillt = false
            };
            context.TogRuter.Add(BodøOslo0800);

            TogRute BodøOslo1400 = new TogRute()
            {
                AvgangTid = BodøOslo0800.AvgangTid.AddHours(6),
                AnkomstTid = BodøOslo0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 1200,
                Instillt = false
            };
            context.TogRuter.Add(BodøOslo1400);

            TogRute BodøOslo2000 = new TogRute()
            {
                AvgangTid = BodøOslo1400.AvgangTid.AddHours(6),
                AnkomstTid = BodøOslo1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Oslo sentralstasjon",
                Platform = "1",
                Pris = 1200,
                Instillt = false
            };
            context.TogRuter.Add(BodøOslo2000);

            TogRute BodøTrondheim0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 6, 17, 00, 00),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "2",
                Pris = 1000,
                Instillt = false
            };
            context.TogRuter.Add(BodøTrondheim0800);

            TogRute BodøTrondheim1400 = new TogRute()
            {
                AvgangTid = BodøTrondheim0800.AvgangTid.AddHours(6),
                AnkomstTid = BodøTrondheim0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "2",
                Pris = 1000,
                Instillt = false
            };
            context.TogRuter.Add(BodøTrondheim1400);

            TogRute BodøTrondheim2000 = new TogRute()
            {
                AvgangTid = BodøTrondheim1400.AvgangTid.AddHours(6),
                AnkomstTid = BodøTrondheim1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Trondheim sentralstasjon",
                Platform = "2",
                Pris = 1000,
                Instillt = false
            };
            context.TogRuter.Add(BodøTrondheim2000);

            TogRute BodøBergen0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 7, 09, 00, 00),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "3",
                Pris = 1100,
                Instillt = false
            };
            context.TogRuter.Add(BodøBergen0800);

            TogRute BodøBergen1400 = new TogRute()
            {
                AvgangTid = BodøBergen0800.AvgangTid.AddHours(6),
                AnkomstTid = BodøBergen0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "3",
                Pris = 1100,
                Instillt = false
            };
            context.TogRuter.Add(BodøBergen1400);

            TogRute BodøBergen2000 = new TogRute()
            {
                AvgangTid = BodøBergen1400.AvgangTid.AddHours(6),
                AnkomstTid = BodøBergen1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Bergen jernbanestasjon",
                Platform = "3",
                Pris = 1100,
                Instillt = false
            };
            context.TogRuter.Add(BodøBergen2000);

            TogRute BodøKristiansand0800 = new TogRute()
            {
                AvgangTid = new DateTime(2019, 10, 6, 08, 00, 00),
                AnkomstTid = new DateTime(2019, 10, 7, 08, 00, 00),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "4",
                Pris = 1500,
                Instillt = false
            };
            context.TogRuter.Add(BodøKristiansand0800);

            TogRute BodøKristiansand1400 = new TogRute()
            {
                AvgangTid = BodøKristiansand0800.AvgangTid.AddHours(6),
                AnkomstTid = BodøKristiansand0800.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "4",
                Pris = 1500,
                Instillt = false
            };
            context.TogRuter.Add(BodøKristiansand1400);

            TogRute BodøKristiansand2000 = new TogRute()
            {
                AvgangTid = BodøKristiansand1400.AvgangTid.AddHours(6),
                AnkomstTid = BodøKristiansand1400.AnkomstTid.AddHours(6),
                StartStasjon = "Bodø stasjon",
                EndeStasjon = "Kristiansand stasjon",
                Platform = "4",
                Pris = 1500,
                Instillt = false
            };
            context.TogRuter.Add(BodøKristiansand2000);
            //slutt togruter 
            base.Seed(context);
        }
    }
}

