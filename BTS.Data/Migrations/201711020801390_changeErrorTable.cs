namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeErrorTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Errors", "Description", c => c.String());
            AddColumn("dbo.Errors", "Controller", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Errors", "Controller");
            DropColumn("dbo.Errors", "Description");
        }
    }
}
