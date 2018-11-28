namespace NCDSB_ConferenceForm_Submit.DAL.UserAccounts
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NCDSB_ConferenceForm_Submit.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NCDSB_ConferenceForm_Submit.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\UserAccounts";
        }

        protected override void Seed(NCDSB_ConferenceForm_Submit.DAL.ApplicationDbContext context)
        {
            //Create a Role Manager
            var roleManager = new RoleManager<IdentityRole>(new
                                          RoleStore<IdentityRole>(context));
            //Create Role Admin if it does not exist
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Admin"));
            }
            //Create Role Security if it does not exist
            if (!context.Roles.Any(r => r.Name == "Staff"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Staff"));
            }

            //Create a User Manager
            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //Now the Admin user named admin with password password
            var adminUser = new ApplicationUser
            {
                UserName = "sdriscoll@ncdsb.ca",
                FirstName = "Steve",
                LastName = "Driscoll",
                Position = "Junior Database Admin",
                Email = "sdriscoll@ncdsb.ca"
            };
            //Assign admin user to role
            if (!context.Users.Any(u => u.UserName == "sdriscoll@ncdsb.ca"))
            {
                manager.Create(adminUser, "password");
                manager.AddToRole(adminUser.Id, "Admin");
            }

            //Now the Steward user named steward with password password
            var staffUser = new ApplicationUser
            {
                UserName = "mmanieri@outlook.com",
                FirstName = "Micheal",
                LastName = "Manieri",
                Position = "Teacher",
                Email = "mmanieri@outlook.com"
            };
            //Assign steward user to role
            if (!context.Users.Any(u => u.UserName == "mmanieri@outlook.com"))
            {
                manager.Create(staffUser, "password");
                manager.AddToRole(staffUser.Id, "Staff");
            }
        }
    }
}
