namespace NCDSB_ConferenceForm_Submit.DAL.ConferenceFormMigrations
{
    using NCDSB_ConferenceForm_Submit.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<NCDSB_ConferenceForm_Submit.DAL.ConferenceFormEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\ConferenceFormMigrations";
        }

        protected override void Seed(NCDSB_ConferenceForm_Submit.DAL.ConferenceFormEntities context)
        {
            var cities = new List<City>
            {
                new City { Name = "Thorold" },
                new City { Name = "Grimsby" },
                new City { Name = "St. Catharines" },
                new City { Name = "Welland" },
                new City { Name = "Niagara Falls" },
                new City { Name = "Pelham" },
                new City { Name = "Fonthill" },
                new City { Name = "Lincoln" },
                new City { Name = "Port Colborne" },
                new City { Name = "Fort Erie" },
                new City { Name = "Vineland" },
                new City { Name = "Jordan" },
                new City { Name = "Hamilton" },
                new City { Name = "Toronto" },
                new City { Name = "North York" },
            };
            cities.ForEach(m => context.Cities.AddOrUpdate(n => n.Name, m));
            SaveChanges(context);

            var addresses = new List<Address>
            {
                new Address
                {
                    SiteName = "Monsignor Clancy",
                    StreetAddress = "41 Collier Rd S",
                    CityID = (context.Cities.Where(m => m.Name == "Thorold").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Hamilton Board Office",
                    StreetAddress = "90 Mulberry St",
                    CityID = (context.Cities.Where(m => m.Name == "Hamilton").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Toronto Board Office",
                    StreetAddress = "80 Sheppard Ave E",
                    CityID = (context.Cities.Where(m => m.Name == "North York").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Dennis Morris",
                    StreetAddress = "40 Glen Morris Dr",
                    CityID = (context.Cities.Where(m => m.Name == "St. Catharines").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "St. Michaels",
                    StreetAddress = "8699 McLeod Rd",
                    CityID = (context.Cities.Where(m => m.Name == "Niagara Falls").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "NCDSB Office",
                    StreetAddress = "427 Rice Rd",
                    CityID = (context.Cities.Where(m => m.Name == "Welland").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Hamilton Convention Center",
                    StreetAddress = "1 Summers Ln",
                    CityID = (context.Cities.Where(m => m.Name == "Hamilton").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Lincoln Alexander Center",
                    StreetAddress = "160 King St E",
                    CityID = (context.Cities.Where(m => m.Name == "Hamilton").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Mohawk College Conference Center",
                    StreetAddress = "235 Fennell Avenue West",
                    CityID = (context.Cities.Where(m => m.Name == "Hamilton").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Vantage Venues",
                    StreetAddress = "150 King St W",
                    CityID = (context.Cities.Where(m => m.Name == "Toronto").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "The Westin Harbour Castle Conference Centre",
                    StreetAddress = "11 Bay Street, Toronto",
                    CityID = (context.Cities.Where(m => m.Name == "Toronto").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Automotive Building",
                    StreetAddress = "105 Princes' Blvd",
                    CityID = (context.Cities.Where(m => m.Name == "Toronto").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Brock University",
                    StreetAddress = "1812 Sir Isaac Brock Way",
                    CityID = (context.Cities.Where(m => m.Name == "St. Catharines").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Blessed Trinity",
                    StreetAddress = "145 Livingston Ave",
                    CityID = (context.Cities.Where(m => m.Name == "Grimsby").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Holy Cross",
                    StreetAddress = "460 Linwell Road",
                    CityID = (context.Cities.Where(m => m.Name == "St. Catharines").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Lakeshore Catholic",
                    StreetAddress = "150 Janet St",
                    CityID = (context.Cities.Where(m => m.Name == "Port Colborne").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Saint Francis",
                    StreetAddress = "541 Lake St",
                    CityID = (context.Cities.Where(m => m.Name == "St. Catharines").SingleOrDefault().ID)
                },

                new Address
                {
                    SiteName = "Saint Paul",
                    StreetAddress = "3834 Windermere Rd ",
                    CityID = (context.Cities.Where(m => m.Name == "Niagara Falls").SingleOrDefault().ID)
                }
            };
            addresses.ForEach(m => context.Addresses.AddOrUpdate(n => n.SiteName, m));
            SaveChanges(context);

            var conferences = new List<Conference>
            {

                new Conference
                {
                    StartDate = DateTime.Parse("2018-4-22"),
                    EndDate = DateTime.Parse("2018-4-22"),
                    Name = "Classroom Communications Techniques",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Toronto Board Office").SingleOrDefault().ID),
                    Description = "A comprehensive look at cutting edge techniques aimed at improving teacher" +
                    " and student relations inside the classroom.",
                    Cost = 395.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-5-25"),
                    EndDate = DateTime.Parse("2018-5-25"),
                    Name = "De-Escalation in the Workplace",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "NCDSB Office").SingleOrDefault().ID),
                    Description = "Looking at creative ways to ease tension in all settings of the classroom or " +
                    "between co-workers.",
                    Cost = 505.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-6-21"),
                    EndDate = DateTime.Parse("2018-6-21"),
                    Name = "Fundamental Database Skills Workshop",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Hamilton Board Office").SingleOrDefault().ID),
                    Description = "Aimed at school board IT staff to keep in touch with new developments " +
                    "and techniques that pertain to data structures.",
                    Cost = 375.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-7-21"),
                    EndDate = DateTime.Parse("2018-7-21"),
                    Name = "Annual Teacher Conference",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Vantage Venues").SingleOrDefault().ID),
                    Description = "For all teachers from all Ontario school boards. Aimed to address " +
                    "the current climate in educations and where we are headed.",
                    Cost = 550.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-7-21"),
                    EndDate = DateTime.Parse("2018-7-21"),
                    Name = "IT Support Staff Seminar",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Lincoln Alexander Center").SingleOrDefault().ID),
                    Description = "A skills sharpening seminar for all school board IT staff",
                    Cost = 199.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-7-05"),
                    EndDate = DateTime.Parse("2018-7-05"),
                    Name = "Math Teacher Conference",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Automotive Building").SingleOrDefault().ID),
                    Description = "Discussing upcoming changes to the math curriculum.",
                    Cost = 149.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-8-02"),
                    EndDate = DateTime.Parse("2018-8-02"),
                    Name = "Coaching Seminar for Educators",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Brock University").SingleOrDefault().ID),
                    Description = "For educators that are involved in coaching after school sports " +
                    "or chair activity groups. Aimed at enhancing communication and team building.",
                    Cost = 295.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-5-04"),
                    EndDate = DateTime.Parse("2018-5-04"),
                    Name = "Administration Diversity Seminar",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "Hamilton Convention Center").SingleOrDefault().ID),
                    Description = "Discussing the growing need for active and flexibile administrators. " +
                    "This conference targets anyone in an administration position and is focused on " +
                    "diversity in the workplace.",
                    Cost = 495.00m
                },

                new Conference
                {
                    StartDate = DateTime.Parse("2018-9-01"),
                    EndDate = DateTime.Parse("2018-9-01"),
                    Name = "Physical Education Teachers Conference",
                    AddressID = (context.Addresses.Where(m => m.SiteName == "The Westin Harbour Castle Conference Centre").SingleOrDefault().ID),
                    Description = "Inviting all Phys. Ed teachers from all boards in Ontario to take part " +
                    "in an informative information session involving fitness and nutrition.",
                    Cost = 275.00m
                }
            };
            conferences.ForEach(m => context.Conferences.AddOrUpdate(n => n.Name, m));
            SaveChanges(context);

            var formStatus = new List<FormStatus>
            {
                new FormStatus { StatusType = "Pending"},
                new FormStatus { StatusType = "Approved"},
                new FormStatus { StatusType = "Denied"},
                new FormStatus { StatusType = "Requires Clarification"}
            };
            formStatus.ForEach(m => context.FormStatus.AddOrUpdate(n => n.StatusType, m));
            SaveChanges(context);

            var budgetCode = new List<BudgetCode>
            {
                new BudgetCode { CodeType = "Paid By Corporate Credit Card"},
                new BudgetCode { CodeType = "Reimbursement by Union"},
                new BudgetCode { CodeType = "Pay by Corporate Cheque Requisition"},
                new BudgetCode { CodeType = "Paid by Staff Member"}
            };
            budgetCode.ForEach(m => context.BudgetCodes.AddOrUpdate(n => n.CodeType, m));
            SaveChanges(context);

            var expenseType = new List<ExpenseType>
            {
                new ExpenseType { TypeOfExpense = "Meal"},
                new ExpenseType { TypeOfExpense = "Train Ticket"},
                new ExpenseType { TypeOfExpense = "Bus Ticket"},
                new ExpenseType { TypeOfExpense = "Cab Fare"},
                new ExpenseType { TypeOfExpense = "Airfare"},
                new ExpenseType { TypeOfExpense = "Accommodation"},
                new ExpenseType { TypeOfExpense = "Parking"},
                new ExpenseType { TypeOfExpense = "Mileage"}
            };
            expenseType.ForEach(m => context.ExpenseTypes.AddOrUpdate(n => n.TypeOfExpense, m));
            SaveChanges(context);

            var staff = new List<StaffMember>
            {
                new StaffMember
                {
                    FirstName = "Steve",
                    MiddleName = "Jonathan",
                    LastName = "Driscoll",
                    Position = "Junior Database Admin",
                    Email = "sdriscoll@ncdsb.ca"
                },

                new StaffMember
                {
                    FirstName = "Michael",
                    MiddleName = "The Fixer",
                    LastName = "Manieri",
                    Position = "Teacher",
                    Email = "mmanieri@outlook.com"
                }
            };
            staff.ForEach(m => context.Staff.AddOrUpdate(n => n.Email, m));
            SaveChanges(context);

            var mileage = new List<MileageForm>
            {
                new MileageForm
                {
                    StaffMemberID = (context.Staff.Where(s=>s.Email == "sdriscoll@ncdsb.ca").SingleOrDefault().ID),
                    Date = DateTime.Parse("2018-02-15"),
                    FormStatusID = (context.FormStatus.Where(m=>m.StatusType == "Pending").SingleOrDefault().ID)
                },

               new MileageForm
                {
                    StaffMemberID = (context.Staff.Where(s=>s.Email == "mmanieri@outlook.com").SingleOrDefault().ID),
                    Date = DateTime.Parse("2018-02-17"),
                    FormStatusID = (context.FormStatus.Where(m=>m.StatusType == "Pending").SingleOrDefault().ID)
                }
            };
            //need to add something that will be unique to each form other than ID
            mileage.ForEach(m => context.MileageForms.AddOrUpdate(n => n.ID, m));
            SaveChanges(context);

            var trips = new List<Trip>
            {
                new Trip
                {
                    StartAddressID = (context.Addresses.Where(a=>a.SiteName=="Monsignor Clancy").SingleOrDefault().ID),
                    EndAddressID = (context.Addresses.Where(a=>a.SiteName=="Dennis Morris").SingleOrDefault().ID),
                    Distance = 10.2m,
                    Date = DateTime.Parse("2018-02-01"),
                    MileageFormID = (context.MileageForms.Where(m=>m.ID==1).SingleOrDefault().ID)
                },


                new Trip
                {
                    StartAddressID = (context.Addresses.Where(a=>a.SiteName=="NCDSB Office").SingleOrDefault().ID),
                    EndAddressID = (context.Addresses.Where(a=>a.SiteName=="Dennis Morris").SingleOrDefault().ID),
                    Distance = 17.5m,
                    Date = DateTime.Parse("2018-02-03"),
                    MileageFormID = (context.MileageForms.Where(m=>m.ID==2).SingleOrDefault().ID)
                },

                new Trip
                {
                    StartAddressID = (context.Addresses.Where(a=>a.SiteName=="Dennis Morris").SingleOrDefault().ID),
                    EndAddressID = (context.Addresses.Where(a=>a.SiteName=="Monsignor Clancy").SingleOrDefault().ID),
                    Distance = 10.2m,
                    Date = DateTime.Parse("2018-02-03"),
                    MileageFormID = (context.MileageForms.Where(m=>m.ID==2).SingleOrDefault().ID)
                }
            };
            trips.ForEach(m => context.Trips.AddOrUpdate(n => n.ID, m));
            SaveChanges(context);


            var conferenceForm = new List<ConferenceForm>
            {
                new ConferenceForm
                {
                    StaffMemberID = (context.Staff.Where(s=>s.Email == "sdriscoll@ncdsb.ca").SingleOrDefault().ID),
                    ConferenceID = (context.Conferences.Where(m=>m.Name == "Classroom Communications Techniques").SingleOrDefault().ID),
                    BenefitOfAttending = "It will enhance my ability to do my job.",
                    ReqReplacementStaff = false,
                    FormStatusID = (context.FormStatus.Where(m=>m.StatusType == "Pending").SingleOrDefault().ID)
                    
                },

                new ConferenceForm
                {
                    StaffMemberID = (context.Staff.Where(s=>s.Email == "mmanieri@outlook.com").SingleOrDefault().ID),
                    ConferenceID = (context.Conferences.Where(m=>m.Name == "De-Escalation in the Workplace").SingleOrDefault().ID),
                    BenefitOfAttending = "It will enhance my ability to be a better professional in the world.",
                    ReqReplacementStaff = true,
                    FormStatusID = (context.FormStatus.Where(m=>m.StatusType == "Pending").SingleOrDefault().ID)
                }
            };
            //need to add something that will be unique to each form other than ID
            conferenceForm.ForEach(m => context.ConferenceForms.AddOrUpdate(n => n.ID, m));
            SaveChanges(context);



            var expenses = new List<Expense>
            {
                new Expense
                {
                    ConferenceFormID = (context.ConferenceForms.Where(m=>m.ID==1).SingleOrDefault().ID),
                    ExpenseTypeID = (context.ExpenseTypes.Where(m=>m.TypeOfExpense == "Meal").SingleOrDefault().ID),
                    ExpenseEstAmount = 15.99m,
                    BudgetCodeID = (context.BudgetCodes.Where(m=>m.CodeType == "Reimbursement by Union").SingleOrDefault().ID)
                },

                new Expense
                {
                    ConferenceFormID = (context.ConferenceForms.Where(m=>m.ID==1).SingleOrDefault().ID),
                    ExpenseTypeID = (context.ExpenseTypes.Where(m=>m.TypeOfExpense == "Mileage").SingleOrDefault().ID),
                    ExpenseEstAmount = 30.0m,
                    BudgetCodeID = (context.BudgetCodes.Where(m=>m.CodeType == "Paid By Corporate Credit Card").SingleOrDefault().ID)
                },

                new Expense
                {
                    ConferenceFormID = (context.ConferenceForms.Where(m=>m.ID==1).SingleOrDefault().ID),
                    ExpenseTypeID = (context.ExpenseTypes.Where(m=>m.TypeOfExpense == "Parking").SingleOrDefault().ID),
                    ExpenseEstAmount = 10.0m,
                    BudgetCodeID = (context.BudgetCodes.Where(m=>m.CodeType == "Paid By Corporate Credit Card").SingleOrDefault().ID)
                },

                //test expense for second conference form request
                new Expense
                {
                    ConferenceFormID = (context.ConferenceForms.Where(m=>m.ID==2).SingleOrDefault().ID),
                    ExpenseTypeID = (context.ExpenseTypes.Where(m=>m.TypeOfExpense == "Meal").SingleOrDefault().ID),
                    ExpenseEstAmount = 12.99m,
                    BudgetCodeID = (context.BudgetCodes.Where(m=>m.CodeType == "Reimbursement by Union").SingleOrDefault().ID)
                },

                new Expense
                {
                    ConferenceFormID = (context.ConferenceForms.Where(m=>m.ID==2).SingleOrDefault().ID),
                    ExpenseTypeID = (context.ExpenseTypes.Where(m=>m.TypeOfExpense == "Accommodation").SingleOrDefault().ID),
                    ExpenseEstAmount = 45.0m,
                    BudgetCodeID = (context.BudgetCodes.Where(m=>m.CodeType == "Paid by Staff Member").SingleOrDefault().ID)
                },

                new Expense
                {
                    ConferenceFormID = (context.ConferenceForms.Where(m=>m.ID==2).SingleOrDefault().ID),
                    ExpenseTypeID = (context.ExpenseTypes.Where(m=>m.TypeOfExpense == "Meal").SingleOrDefault().ID),
                    ExpenseEstAmount = 20.0m,
                    BudgetCodeID = (context.BudgetCodes.Where(m=>m.CodeType == "Reimbursement by Union").SingleOrDefault().ID)
                }

            };
            //needs something unique for LINQ query
            expenses.ForEach(m => context.Expenses.AddOrUpdate(n => n.ID, m));
            SaveChanges(context);
        }

        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
            catch (Exception e)
            {
                throw new Exception(
                     "Seed Failed - errors follow:\n" +
                     e.InnerException.InnerException.Message.ToString(), e
                 ); // Add the original exception as the innerException
            }
        }
    }
}

