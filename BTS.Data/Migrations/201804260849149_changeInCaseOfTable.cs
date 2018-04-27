namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInCaseOfTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InCaseOfs", "Code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InCaseOfs", "Code");
        }
    }
}
