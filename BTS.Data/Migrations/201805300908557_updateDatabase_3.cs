namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDatabase_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Certificates", "SharedAntens", c => c.String(maxLength: 50));
            AddColumn("dbo.Certificates", "MeasuringExposure", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Certificates", "MeasuringExposure");
            DropColumn("dbo.Certificates", "SharedAntens");
        }
    }
}
