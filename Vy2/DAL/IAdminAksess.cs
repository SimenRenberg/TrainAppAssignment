using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public interface IAdminAksess
    {
        bool InstillRute(int RuteId);
        bool GjenopprettRute(int RuteId);
        bool EndreRute(TogRute valgtRute);
        Administrator GetAdministrator(string Epost);
        bool RegistrerAdministrator(Administrator nyAdmin);
        List<Administrator> GetAlleAdmins();
        TogRute GetTogrute(int RuteId);
        List<TogRute> VisAlleRuter();
        List<TogRute> VisInstillteRuter();
        bool LoggInn(string Epost, string Passord);
        bool LagAdmin(string rolle, string nyEpost, string nyPassord);
        bool SlettAdmin(int AnsattNr);
        List<DBLogg> GetEndringer();
        bool LeggTilRute(TogRute nyRute);
        bool EndreAdmin(Administrator admin);
    }
}
