using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NCDSB_ConferenceForm_Submit.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace NCDSB_ConferenceForm_Submit.DAL
{
    public class ConferenceFormEntities : DbContext
    {
        public ConferenceFormEntities() : base("name=ConferenceFormEntities")
        {
            // SHOW ME THE MONEY... (Or at least the SQL)
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public DbSet<StaffMember> Staff { get; set; }
        public DbSet<Conference> Conferences { get; set; }
        public DbSet<FormStatus> FormStatus { get; set; }
        public DbSet<BudgetCode> BudgetCodes { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MileageForm> MileageForms { get; set; }
        public DbSet<ConferenceForm> ConferenceForms { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            
            //to reference same pk twice
            modelBuilder.Entity<Trip>().HasRequired(m => m.StartAddress)
                                 .WithMany(m => m.Trips).HasForeignKey(m => m.StartAddressID);

            modelBuilder.Entity<Trip>().HasRequired(m => m.EndAddress)
                                        .WithMany().HasForeignKey(m => m.EndAddressID);

            modelBuilder.Entity<Expense>().HasOptional(m => m.Receipt)
                                    .WithRequired(n => n.Expense)
                                    .WillCascadeOnDelete(true);

        }

        public override int SaveChanges()
        {
            //Get Audit Values if not supplied
            string auditUser = "Anonymous";
            try //Need to try because HttpContext might not exist
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    auditUser = HttpContext.Current.User.Identity.Name;
            }
            catch (Exception)
            { }

            DateTime auditDate = DateTime.UtcNow;
            foreach (DbEntityEntry<IAuditable> entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = auditDate;
                    entry.Entity.CreatedBy = auditUser;
                    entry.Entity.UpdatedOn = auditDate;
                    entry.Entity.UpdatedBy = auditUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedOn = auditDate;
                    entry.Entity.UpdatedBy = auditUser;
                }
                else 
                {
                    entry.Entity.RemovedOn = auditDate;
                    entry.Entity.RemovedBy = auditUser;
                }

            }
            return base.SaveChanges();
        }
    }
}