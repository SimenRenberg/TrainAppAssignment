using System.Collections.Generic;
using Model;

namespace BLL
{
    public interface IAdminBLL
    {
        bool EndreRute(TogRute valgtRute);
        string FinnRolle(string epost);
        List<Administrator> GetAlleAdministratorer();
        TogRute GetTogRute(int RuteId);
        bool GjenopprettRute(int RuteId);
        bool InstillRute(int RuteId);
        string LagAdmin(string rolle, string nyEpost, string nyPassord);
        bool LoggInn(string Epost, string Passord);
        List<TogRute> VisAlleRuter();
        List<TogRute> VisInstillteRuter();
        bool SlettAdmin(int AnsattNr);
        List<DBLogg> GetEndringer();
        bool LeggTilRute(TogRute nyRute);
        bool EndreAdmin(Administrator admin);
    }
}