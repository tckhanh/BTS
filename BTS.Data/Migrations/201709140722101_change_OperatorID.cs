namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_OperatorID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Operators", "Telephone", c => c.String(maxLength: 30));
            AlterColumn("dbo.Operators", "Fax", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Operators", "Fax", c => c.String(maxLength: 50));
            AlterColumn("dbo.Operators", "Telephone", c => c.String(maxLength: 50));
        }
    }
}
