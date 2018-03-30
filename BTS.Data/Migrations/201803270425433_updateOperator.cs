namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOperator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operators", "Address", c => c.String(maxLength: 255));
            AddColumn("dbo.Operators", "Telephone", c => c.String(maxLength: 30));
            AddColumn("dbo.Operators", "Fax", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operators", "Fax");
            DropColumn("dbo.Operators", "Telephone");
            DropColumn("dbo.Operators", "Address");
        }
    }
}
