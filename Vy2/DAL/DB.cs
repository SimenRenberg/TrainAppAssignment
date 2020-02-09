using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

using System.IO;
using System.Linq;
using Model;

namespace DAL
{
    public class DB : DbContext
    {
        public DB() : base("name=VyDB")
        {
            Database.SetInitializer(new DBInit<DB>());
        }

        protected override void OnModelCreating(DbModelBuilder ModelBuilder)
        {
            ModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<Tilbakemelding> Tilbakemeldinger { get; set; }
        public virtual DbSet<Kunde> Kunder { get; set; }

        public virtual DbSet<Billett> Billetter { get; set; }

        public virtual DbSet<TogRute> TogRuter { get; set; }

        public virtual DbSet<Stasjoner> Stasjoner { get; set; }

        public virtual DbSet<Administrator> Administratorer { get; set; }

        public virtual DbSet<DBLogg> Endringer { get; set; }

        //LÅNT FRA https://exceptionnotfound.net/entity-change-tracking-using-dbcontext-in-entity-framework-6/

        //får tak i primærnøkkelen (ID) til entitene som endres
        //fantastisk guide fra exceptionnotfound, men vi må spørre om det er lov å låne dette.
        object GetID(DbEntityEntry innEntitet)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(innEntitet.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }


        public override int SaveChanges()
        {
            var endredeTabeller = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified);
            var Nå = DateTime.Now;

            foreach (var endringer in endredeTabeller)
            {
                var navnPåEndring = endringer.Entity.GetType().Name;
                var ID = GetID(endringer).ToString();

                foreach (var kolonne in endringer.OriginalValues.PropertyNames)
                {
                    var originalVerdi = endringer.OriginalValues[kolonne].ToString();
                    var nåværendeVerdi = endringer.CurrentValues[kolonne].ToString();
                    if (originalVerdi != nåværendeVerdi)
                    {
                        DBLogg logg = new DBLogg()
                        {
                            Tabell = navnPåEndring,
                            Kolonne = kolonne,
                            ID = ID,
                            GammelVerdi = originalVerdi,
                            NyVerdi = nåværendeVerdi,
                            DatoEndret = Nå
                        };
                        Endringer.Add(logg);
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
