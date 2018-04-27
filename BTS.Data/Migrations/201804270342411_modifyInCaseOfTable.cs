namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyInCaseOfTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Btss", "InCaseOfID", "dbo.InCaseOfs");
            DropForeignKey("dbo.Certificates", "InCaseOfID", "dbo.InCaseOfs");
            DropPrimaryKey("dbo.InCaseOfs");
            AlterColumn("dbo.InCaseOfs", "ID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.InCaseOfs", "ID");
            AddForeignKey("dbo.Btss", "InCaseOfID", "dbo.InCaseOfs", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Certificates", "InCaseOfID", "dbo.InCaseOfs", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Certificates", "InCaseOfID", "dbo.InCaseOfs");
            DropForeignKey("dbo.Btss", "InCaseOfID", "dbo.InCaseOfs");
            DropPrimaryKey("dbo.InCaseOfs");
            AlterColumn("dbo.InCaseOfs", "ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.InCaseOfs", "ID");
            AddForeignKey("dbo.Certificates", "InCaseOfID", "dbo.InCaseOfs", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Btss", "InCaseOfID", "dbo.InCaseOfs", "ID", cascadeDelete: true);
        }
    }
}
