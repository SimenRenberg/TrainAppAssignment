using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DAL;
using Model;
using System.IO;


namespace BLL
{
    public class AdminBLL : IAdminBLL
    {
        private IAdminAksess _repository;

        public AdminBLL()
        {
            _repository = new AdminAksess();
        }

        public AdminBLL(IAdminAksess stub)
        {
            _repository = stub;
        }



        public bool InstillRute(int RuteId)
        {
            if (_repository.InstillRute(RuteId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GjenopprettRute(int RuteId)
        {
            if (_repository.GjenopprettRute(RuteId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EndreRute(TogRute valgtRute)
        {
            if (_repository.EndreRute(valgtRute))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LeggTilRute(TogRute nyRute)
        {
            if (_repository.LeggTilRute(nyRute))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public TogRute GetTogRute(int RuteId)
        {
            TogRute funnetRute = _repository.GetTogrute(RuteId);
            return funnetRute;
        }

        public List<TogRute> VisAlleRuter()
        {
            List<TogRute> alleRuter = _repository.VisAlleRuter();
            return alleRuter;
        }

        public List<TogRute> VisInstillteRuter()
        {
            List<TogRute> instillteRuter = _repository.VisInstillteRuter();
            return instillteRuter;
        }

        public bool LoggInn(string Epost, string Passord)
        {
            return _repository.LoggInn(Epost, Passord);
        }

        public string LagAdmin(string rolle, string nyEpost, string nyPassord)
        {
            if ((bool)_repository.LagAdmin(rolle, nyEpost, nyPassord))
            {
                return "Ny administrator er lagt inn.";
            }
            else
            {
                return "En annen admin er registrert på samme epost.";
            }
        }



        public List<Administrator> GetAlleAdministratorer()
        {
            List<Administrator> alleAdmins = _repository.GetAlleAdmins();
            return alleAdmins;
        }

        public string FinnRolle(string epost)
        {
            Administrator funnetAdmin = _repository.GetAdministrator(epost);
            return funnetAdmin.Rolle;
        }

        public bool SlettAdmin(int AnsattNr)
        {
            if (AnsattNr <= 0)
            {
                return false;
            }
            else if (_repository.SlettAdmin(AnsattNr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<DBLogg> GetEndringer()
        {
            return _repository.GetEndringer();
        }

        public bool EndreAdmin(Administrator admin)
        {
            return _repository.EndreAdmin(admin);
        }
    }
}
