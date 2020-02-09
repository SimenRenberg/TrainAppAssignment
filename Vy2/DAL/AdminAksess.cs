using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Data.Entity.Infrastructure;

namespace DAL
{
    public class AdminAksess : IAdminAksess
    {
        ErrorLogg loggTilFil = new ErrorLogg();

        //TODO: Logg feilmeldinger (som null fra DBKall)
        public bool InstillRute(int RuteId)
        {
            using (var db = new DB())
            {
                try
                {
                    db.TogRuter.FirstOrDefault(r => r.RuteId == RuteId).Instillt = true;
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

        public bool GjenopprettRute(int RuteId)
        {
            using (var db = new DB())
            {
                try
                {
                    db.TogRuter.First(r => r.RuteId == RuteId).Instillt = false;
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

        public bool EndreRute(TogRute valgtRute)
        {
            using (var db = new DB())
            {
                try //TogRuter.First fordi vi vil ikke tillate null.
                {
                    db.TogRuter.First(r => r.RuteId == valgtRute.RuteId).AnkomstTid = valgtRute.AnkomstTid;
                    db.TogRuter.First(r => r.RuteId == valgtRute.RuteId).AvgangTid = valgtRute.AvgangTid;
                    db.TogRuter.First(r => r.RuteId == valgtRute.RuteId).Platform = valgtRute.Platform;
                    db.TogRuter.First(r => r.RuteId == valgtRute.RuteId).Pris = valgtRute.Pris;

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



        public Administrator GetAdministrator(string Epost)
        {
            using (var db = new DB())
            {
                Administrator funnetAdmin = db.Administratorer.FirstOrDefault(a => a.Epost == Epost);
                try
                {
                    return funnetAdmin;
                }
                catch (Exception feil)
                {

                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }



        public List<Administrator> GetAlleAdmins()
        {
            using (var db = new DB())
            {
                try
                {
                    List<Administrator> alleAdmins = db.Administratorer.ToList();
                    return alleAdmins;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }

            }
        }

        public TogRute GetTogrute(int RuteId)
        {
            using (var db = new DB())
            {
                try
                {
                    TogRute funnetRute = db.TogRuter.First(r => r.RuteId == RuteId);
                    return funnetRute;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }
        public List<TogRute> VisAlleRuter()
        {
            using (var db = new DB())
            {
                try
                {
                    List<TogRute> alleRuter = db.TogRuter.Where(r => r.Instillt == false).ToList();
                    return alleRuter;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }

        public List<TogRute> VisInstillteRuter()
        {
            using (var db = new DB())
            {
                try
                {
                    List<TogRute> instillteRuter = db.TogRuter.Where(r => r.Instillt == true).ToList();
                    return instillteRuter;
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }



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

        public bool LoggInn(string Epost, string Passord)
        {
            Administrator funnetAdmin = GetAdministrator(Epost);
            if (funnetAdmin == null)
            {
                return false;
            }
            byte[] hashetPassord = LagHash(Passord, funnetAdmin.Salt);

            if (Enumerable.SequenceEqual(hashetPassord, funnetAdmin.Passord))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool RegistrerAdministrator(Administrator nyAdmin)
        {
            using (var db = new DB())
            {
                var funnetAdmin = db.Administratorer.FirstOrDefault(a => a.Epost == nyAdmin.Epost);
                if (funnetAdmin == null)
                {
                    try
                    {
                        db.Administratorer.Add(nyAdmin);
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception feil)
                    {
                        loggTilFil.SkrivTilFil(feil);
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
        }
        public bool LagAdmin(string rolle, string nyEpost, string nyPassord)
        {
            if (rolle == "" || rolle == null)
            {
                return false;
            }
            else if (nyEpost == "" || nyEpost == null)
            {
                return false;
            }
            else if (nyPassord == "" || nyPassord == null)
            {
                return false;
            }
            else
            {
                byte[] salt = LagSalt();
                byte[] hashetPassord = LagHash(nyPassord, salt);

                Administrator nyAdministrator = new Administrator
                {
                    Rolle = rolle,
                    Epost = nyEpost,
                    Passord = hashetPassord,
                    Salt = salt
                };


                if (RegistrerAdministrator(nyAdministrator))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public bool SlettAdmin(int AnsattNr)
        {
            using (var db = new DB())
            {
                try
                {
                    var funnetAdmin = db.Administratorer.First(a => a.AnsattNr == AnsattNr);
                    db.Administratorer.Remove(funnetAdmin);
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

        public List<DBLogg> GetEndringer()
        {
            using (var db = new DB())
            {
                try
                {
                    return db.Endringer.ToList();
                }
                catch (Exception feil)
                {
                    loggTilFil.SkrivTilFil(feil);
                    return null;
                }
            }
        }

        public bool LeggTilRute(TogRute nyRute)
        {
            using (var db = new DB())
            {
                try
                {
                    var funnetStartstasjon = db.Stasjoner.FirstOrDefault(s => s.StasjonsNavn == nyRute.StartStasjon);
                    var funnetEndestasjon = db.Stasjoner.FirstOrDefault(s => s.StasjonsNavn == nyRute.EndeStasjon);
                    if (funnetStartstasjon == null)
                    {
                        Stasjoner nyStasjon = new Stasjoner
                        {
                            StasjonsNavn = nyRute.StartStasjon
                        };
                        db.Stasjoner.Add(nyStasjon);
                    }
                    if (funnetEndestasjon == null)
                    {
                        Stasjoner nyStasjon = new Stasjoner
                        {
                            StasjonsNavn = nyRute.EndeStasjon
                        };
                        db.Stasjoner.Add(nyStasjon);
                    }
                    db.TogRuter.Add(nyRute);
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

        public bool EndreAdmin(Administrator admin)
        {
            using (var db = new DB())
            {
                Administrator funnetAdmin = db.Administratorer.FirstOrDefault(a => a.AnsattNr == admin.AnsattNr);
                if (funnetAdmin != null)
                {
                    try
                    {
                        db.Administratorer.First(a => a.AnsattNr == admin.AnsattNr).Epost = admin.Epost;
                        db.Administratorer.First(a => a.AnsattNr == admin.AnsattNr).Rolle = admin.Rolle;
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception feil)
                    {
                        loggTilFil.SkrivTilFil(feil);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

    }
}

