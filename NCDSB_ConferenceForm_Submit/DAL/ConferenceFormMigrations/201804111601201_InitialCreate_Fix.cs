namespace NCDSB_ConferenceForm_Submit.DAL.ConferenceFormMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate_Fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false, maxLength: 50),
                        StreetAddress = c.String(nullable: false, maxLength: 255),
                        CityID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.City", t => t.CityID)
                .Index(t => t.SiteName, unique: true, name: "IX_Unique_SiteName")
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true, name: "IX_Unique_City");
            
            CreateTable(
                "dbo.Conference",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        AddressID = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 255),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Address", t => t.AddressID)
                .Index(t => new { t.Name, t.StartDate }, unique: true, name: "IX_Unique_Name_Date")
                .Index(t => t.AddressID);
            
            CreateTable(
                "dbo.ConferenceForm",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ConferenceID = c.Int(nullable: false),
                        BenefitOfAttending = c.String(nullable: false),
                        ReqReplacementStaff = c.Boolean(nullable: false),
                        ReasonForStatusChange = c.String(),
                        FormStatusID = c.Int(nullable: false),
                        StaffMemberID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Conference", t => t.ConferenceID)
                .ForeignKey("dbo.FormStatus", t => t.FormStatusID)
                .ForeignKey("dbo.StaffMember", t => t.StaffMemberID)
                .Index(t => t.ConferenceID)
                .Index(t => t.FormStatusID)
                .Index(t => t.StaffMemberID);
            
            CreateTable(
                "dbo.Expense",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ConferenceFormID = c.Int(nullable: false),
                        ExpenseTypeID = c.Int(nullable: false),
                        ExpenseEstAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Reason = c.String(maxLength: 250),
                        ExpenseActAmount = c.Decimal(precision: 18, scale: 2),
                        BudgetCodeID = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BudgetCode", t => t.BudgetCodeID)
                .ForeignKey("dbo.ConferenceForm", t => t.ConferenceFormID)
                .ForeignKey("dbo.ExpenseType", t => t.ExpenseTypeID)
                .Index(t => t.ConferenceFormID)
                .Index(t => t.ExpenseTypeID)
                .Index(t => t.BudgetCodeID);
            
            CreateTable(
                "dbo.BudgetCode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodeType = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.CodeType, unique: true, name: "IX_Unique_CodeType");
            
            CreateTable(
                "dbo.ExpenseType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeOfExpense = c.String(nullable: false, maxLength: 35),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.TypeOfExpense, unique: true, name: "IX_Unique_TypeOfExpense");
            
            CreateTable(
                "dbo.Receipt",
                c => new
                    {
                        ExpenseReceiptID = c.Int(nullable: false),
                        Content = c.Binary(),
                        MimeType = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ExpenseReceiptID)
                .ForeignKey("dbo.Expense", t => t.ExpenseReceiptID, cascadeDelete: true)
                .Index(t => t.ExpenseReceiptID);
            
            CreateTable(
                "dbo.FormStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StatusType = c.String(nullable: false, maxLength: 25),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.StatusType, unique: true, name: "IX_Unique_StatusType");
            
            CreateTable(
                "dbo.MileageForm",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ReasonForStatusChange = c.String(),
                        FormStatusID = c.Int(nullable: false),
                        StaffMemberID = c.Int(nullable: false),
                        ConferenceFormID = c.Int(),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ConferenceForm", t => t.ConferenceFormID)
                .ForeignKey("dbo.FormStatus", t => t.FormStatusID)
                .ForeignKey("dbo.StaffMember", t => t.StaffMemberID)
                .Index(t => t.FormStatusID)
                .Index(t => t.StaffMemberID)
                .Index(t => t.ConferenceFormID);
            
            CreateTable(
                "dbo.StaffMember",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        MiddleName = c.String(maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Position = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Email, unique: true, name: "IX_Unique_Email");
            
            CreateTable(
                "dbo.Trip",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartAddressID = c.Int(nullable: false),
                        EndAddressID = c.Int(nullable: false),
                        Distance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        MileageFormID = c.Int(),
                        ConferenceFormID = c.Int(),
                        CreatedBy = c.String(maxLength: 256),
                        CreatedOn = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        UpdatedOn = c.DateTime(),
                        RemovedBy = c.String(maxLength: 256),
                        RemovedOn = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ConferenceForm", t => t.ConferenceFormID)
                .ForeignKey("dbo.Address", t => t.EndAddressID)
                .ForeignKey("dbo.MileageForm", t => t.MileageFormID)
                .ForeignKey("dbo.Address", t => t.StartAddressID)
                .Index(t => t.StartAddressID)
                .Index(t => t.EndAddressID)
                .Index(t => t.MileageFormID)
                .Index(t => t.ConferenceFormID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trip", "StartAddressID", "dbo.Address");
            DropForeignKey("dbo.Trip", "MileageFormID", "dbo.MileageForm");
            DropForeignKey("dbo.Trip", "EndAddressID", "dbo.Address");
            DropForeignKey("dbo.Trip", "ConferenceFormID", "dbo.ConferenceForm");
            DropForeignKey("dbo.MileageForm", "StaffMemberID", "dbo.StaffMember");
            DropForeignKey("dbo.ConferenceForm", "StaffMemberID", "dbo.StaffMember");
            DropForeignKey("dbo.MileageForm", "FormStatusID", "dbo.FormStatus");
            DropForeignKey("dbo.MileageForm", "ConferenceFormID", "dbo.ConferenceForm");
            DropForeignKey("dbo.ConferenceForm", "FormStatusID", "dbo.FormStatus");
            DropForeignKey("dbo.Receipt", "ExpenseReceiptID", "dbo.Expense");
            DropForeignKey("dbo.Expense", "ExpenseTypeID", "dbo.ExpenseType");
            DropForeignKey("dbo.Expense", "ConferenceFormID", "dbo.ConferenceForm");
            DropForeignKey("dbo.Expense", "BudgetCodeID", "dbo.BudgetCode");
            DropForeignKey("dbo.ConferenceForm", "ConferenceID", "dbo.Conference");
            DropForeignKey("dbo.Conference", "AddressID", "dbo.Address");
            DropForeignKey("dbo.Address", "CityID", "dbo.City");
            DropIndex("dbo.Trip", new[] { "ConferenceFormID" });
            DropIndex("dbo.Trip", new[] { "MileageFormID" });
            DropIndex("dbo.Trip", new[] { "EndAddressID" });
            DropIndex("dbo.Trip", new[] { "StartAddressID" });
            DropIndex("dbo.StaffMember", "IX_Unique_Email");
            DropIndex("dbo.MileageForm", new[] { "ConferenceFormID" });
            DropIndex("dbo.MileageForm", new[] { "StaffMemberID" });
            DropIndex("dbo.MileageForm", new[] { "FormStatusID" });
            DropIndex("dbo.FormStatus", "IX_Unique_StatusType");
            DropIndex("dbo.Receipt", new[] { "ExpenseReceiptID" });
            DropIndex("dbo.ExpenseType", "IX_Unique_TypeOfExpense");
            DropIndex("dbo.BudgetCode", "IX_Unique_CodeType");
            DropIndex("dbo.Expense", new[] { "BudgetCodeID" });
            DropIndex("dbo.Expense", new[] { "ExpenseTypeID" });
            DropIndex("dbo.Expense", new[] { "ConferenceFormID" });
            DropIndex("dbo.ConferenceForm", new[] { "StaffMemberID" });
            DropIndex("dbo.ConferenceForm", new[] { "FormStatusID" });
            DropIndex("dbo.ConferenceForm", new[] { "ConferenceID" });
            DropIndex("dbo.Conference", new[] { "AddressID" });
            DropIndex("dbo.Conference", "IX_Unique_Name_Date");
            DropIndex("dbo.City", "IX_Unique_City");
            DropIndex("dbo.Address", new[] { "CityID" });
            DropIndex("dbo.Address", "IX_Unique_SiteName");
            DropTable("dbo.Trip");
            DropTable("dbo.StaffMember");
            DropTable("dbo.MileageForm");
            DropTable("dbo.FormStatus");
            DropTable("dbo.Receipt");
            DropTable("dbo.ExpenseType");
            DropTable("dbo.BudgetCode");
            DropTable("dbo.Expense");
            DropTable("dbo.ConferenceForm");
            DropTable("dbo.Conference");
            DropTable("dbo.City");
            DropTable("dbo.Address");
        }
    }
}
