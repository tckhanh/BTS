namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProfileTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Profiles", "ProfileDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Profiles", "ApplyDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Profiles", "ApplyDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Profiles", "ProfileDate", c => c.DateTime(storeType: "date"));
        }
    }
}
