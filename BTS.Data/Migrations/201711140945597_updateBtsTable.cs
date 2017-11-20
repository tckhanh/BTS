namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBtsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.Btss", new[] { "ProfileID" });
            AlterColumn("dbo.Btss", "ProfileID", c => c.Int(nullable: false));
            CreateIndex("dbo.Btss", "ProfileID");
            AddForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.Btss", new[] { "ProfileID" });
            AlterColumn("dbo.Btss", "ProfileID", c => c.Int());
            CreateIndex("dbo.Btss", "ProfileID");
            AddForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles", "ID");
        }
    }
}
