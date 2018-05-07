namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.IdentityUsers", newName: "ApplicationUsers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ApplicationUsers", newName: "IdentityUsers");
        }
    }
}
