namespace NCDSB_ConferenceForm_Submit.DAL.ConferenceFormMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutocompleteWorkAround : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConferenceForm", "TempName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConferenceForm", "TempName");
        }
    }
}
