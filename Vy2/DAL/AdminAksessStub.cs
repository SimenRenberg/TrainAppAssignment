using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class AdminAksessStub : IAdminAksess
    {
        public bool InstillRute(int RuteId)
        {
            if (RuteId <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool GjenopprettRute(int RuteId)
        {
            if (RuteId <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EndreRute(TogRute valgtRute)
        {
            if (valgtRute == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public Administrator GetAdministrator(string Epost)
        {
            if (Epost == "" || Epost == null)
            {
                return null;
            }
            else
            {
                var admin = new Administrator()
                {
                    AnsattNr = 1,
                    Rolle = "Super",
                    Epost = "Super@Admin.no",
                    Passord = new Byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 },
                    Salt = new Byte[8] { 2, 3, 4, 5, 6, 7, 8, 9 }
                };
                return admin;
            }
        }

        public bool RegistrerAdministrator(Administrator nyAdmin)
        {
            if (nyAdmin == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Administrator> GetAlleAdmins()
        {
            List<Administrator> alleAdmins = new List<Administrator>();
            var enAdmin = new Administrator()
            {
                AnsattNr = 2,
                Epost = "admin2@admin.no",
                Passord = new Byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 },
                Rolle = "Vanlig",
                Salt = new Byte[8] { 2, 3, 4, 5, 6, 7, 8, 9 }
            };

            alleAdmins.Add(enAdmin);
            alleAdmins.Add(enAdmin);
            alleAdmins.Add(enAdmin);

            return alleAdmins;
        }

        public TogRute GetTogrute(int RuteId)
        {
            if (RuteId == 0)
            {
                var rute = new TogRute();
                rute.RuteId = 0;
                return rute;
            }
            else
            {
                var rute = new TogRute()
                {
                    AnkomstTid = DateTime.Now.AddHours(2),
                    AvgangTid = DateTime.Now,
                    EndeStasjon = "Bergen jernbanestasjon",
                    StartStasjon = "Oslo sentralstasjon",
                    Instillt = false,
                    Platform = "1",
                    Pris = 700,
                    RuteId = 1
                };

                return rute;
            }
        }

        public List<TogRute> VisAlleRuter()
        {
            var alleRuter = new List<TogRute>();
            var rute = new TogRute()
            {
                AnkomstTid = DateTime.Now.AddHours(2),
                AvgangTid = DateTime.Now,
                EndeStasjon = "Bergen jernbanestasjon",
                StartStasjon = "Oslo sentralstasjon",
                Instillt = false,
                Platform = "1",
                Pris = 700,
                RuteId = 1
            };

            alleRuter.Add(rute);
            alleRuter.Add(rute);
            alleRuter.Add(rute);

            List<TogRute> alleAktiveRuter = alleRuter.Where(r => r.Instillt == false).ToList();
            return alleAktiveRuter;
        }

        public List<TogRute> VisInstillteRuter()
        {
            var alleInstillteRuter = new List<TogRute>();
            var instilltRute = new TogRute()
            {
                AnkomstTid = DateTime.Now.AddHours(2),
                AvgangTid = DateTime.Now,
                EndeStasjon = "Bergen jernbanestasjon",
                StartStasjon = "Oslo sentralstasjon",
                Instillt = true,
                Platform = "1",
                Pris = 700,
                RuteId = 1
            };

            alleInstillteRuter.Add(instilltRute);
            alleInstillteRuter.Add(instilltRute);
            alleInstillteRuter.Add(instilltRute);

            return alleInstillteRuter;
        }

        public bool LoggInn(string Epost, string Passord)
        {
            if (Epost == "" || Passord == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool LagAdmin(string rolle, string nyEpost, string nyPassord)
        {
            if (rolle == "" || nyEpost == "" || nyPassord == "")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool SlettAdmin(int AnsattNr)
        {
            if (AnsattNr == 0)
            {
                return false;
            }
            else
            {
                Administrator admin = new Administrator()
                {
                    AnsattNr = 1,
                    Rolle = "Vanlig",
                };

                List<Administrator> admins = new List<Administrator>()
                {
                    admin
                };

                var funnetAdmin = admins.First(a => a.AnsattNr == AnsattNr);
                if (admins.Remove(funnetAdmin))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<DBLogg> GetEndringer()
        {
            DBLogg logg = new DBLogg()
            {
                EndringsId = 1,
                Tabell = "TogRute",
                Kolonne = "Pris",
                ID = "15",
                GammelVerdi = "50",
                NyVerdi = "500",
                DatoEndret = new DateTime()
            };

            List<DBLogg> loggListe = new List<DBLogg>()
            {
                logg,
                logg,
                logg
            };

            return loggListe;
        }

        public bool LeggTilRute(TogRute rute)
        {
            if (rute.RuteId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EndreAdmin(Administrator admin)
        {

            var enadmin = new Administrator()
            {
                AnsattNr = 1,
                Epost = "test@test",
                Rolle = "vanlig,"
            };

            var enAnnenadmin = new Administrator()
            {
                AnsattNr = 2,
                Epost = "test@admin",
                Rolle = "super"
            };

            var alleadmin = new List<Administrator>
            {
                enadmin,
                enAnnenadmin
            };

            try
            {
                alleadmin.FirstOrDefault(a => a.AnsattNr == admin.AnsattNr).Epost = admin.Epost;
                alleadmin.FirstOrDefault(a => a.AnsattNr == admin.AnsattNr).Rolle = admin.Rolle;
                return true;
            }
            catch (Exception feil)
            {
                return false;
            }

        }
    }
}
