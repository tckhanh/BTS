namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDatabase_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.Certificates", new[] { "ProfileID" });
            DropIndex("dbo.NoCertificates", new[] { "ProfileID" });
            AlterColumn("dbo.Certificates", "ProfileID", c => c.Int(nullable: false));
            AlterColumn("dbo.NoCertificates", "ProfileID", c => c.Int(nullable: false));
            CreateIndex("dbo.Certificates", "ProfileID");
            CreateIndex("dbo.NoCertificates", "ProfileID");
            AddForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.NoCertificates", new[] { "ProfileID" });
            DropIndex("dbo.Certificates", new[] { "ProfileID" });
            AlterColumn("dbo.NoCertificates", "ProfileID", c => c.Int());
            AlterColumn("dbo.Certificates", "ProfileID", c => c.Int());
            CreateIndex("dbo.NoCertificates", "ProfileID");
            CreateIndex("dbo.Certificates", "ProfileID");
            AddForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles", "ID");
            AddForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles", "ID");
        }
    }
}
