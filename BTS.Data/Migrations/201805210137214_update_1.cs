namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "FatherLand", c => c.String(maxLength: 50));
            AddColumn("dbo.ApplicationUsers", "Level", c => c.String(maxLength: 50));
            AddColumn("dbo.ApplicationUsers", "EducationalField", c => c.String(maxLength: 150));
            AddColumn("dbo.ApplicationUsers", "EntryDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUsers", "EndDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUsers", "JobPositions", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "JobPositions");
            DropColumn("dbo.ApplicationUsers", "EndDate");
            DropColumn("dbo.ApplicationUsers", "EntryDate");
            DropColumn("dbo.ApplicationUsers", "EducationalField");
            DropColumn("dbo.ApplicationUsers", "Level");
            DropColumn("dbo.ApplicationUsers", "FatherLand");
        }
    }
}
