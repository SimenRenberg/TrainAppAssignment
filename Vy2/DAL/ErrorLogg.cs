using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL
{
    class ErrorLogg
    {

        public void SkrivTilFil(Exception Feilmelding)
        {
            string FilSti = AppDomain.CurrentDomain.BaseDirectory + @"\DBFeilLogg.txt";

            if (!File.Exists(FilSti))
            {
                File.Create(FilSti).Dispose();
            }

            using (StreamWriter Logg = new StreamWriter(FilSti, append: true))
            {

                Logg.WriteLine("---------- " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ----------");
                Logg.WriteLine("Feilmelding: " + Feilmelding.Message);
                Logg.WriteLine("InnerException: " + Feilmelding.InnerException);
                Logg.WriteLine("");
                Logg.Close();
            }
        }
    }
}
