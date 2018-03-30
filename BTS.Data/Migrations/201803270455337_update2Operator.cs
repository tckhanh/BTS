namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update2Operator : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Operators", "Address");
            DropColumn("dbo.Operators", "Telephone");
            DropColumn("dbo.Operators", "Fax");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operators", "Fax", c => c.String(maxLength: 30));
            AddColumn("dbo.Operators", "Telephone", c => c.String(maxLength: 30));
            AddColumn("dbo.Operators", "Address", c => c.String(maxLength: 255));
        }
    }
}
