namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBtsTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Btss", "LastSelfCertificateID", c => c.String(maxLength: 16));
            AddColumn("dbo.Btss", "LastSelfOperatorID", c => c.String(maxLength: 10));
            AddColumn("dbo.Btss", "LastNoSelfCertificateID", c => c.String(maxLength: 16));
            AddColumn("dbo.Btss", "LastNoSelfOperatorID", c => c.String(maxLength: 10));
            AddColumn("dbo.SubBtsInCerts", "Manufactory", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubBtsInCerts", "Manufactory");
            DropColumn("dbo.Btss", "LastNoSelfOperatorID");
            DropColumn("dbo.Btss", "LastNoSelfCertificateID");
            DropColumn("dbo.Btss", "LastSelfOperatorID");
            DropColumn("dbo.Btss", "LastSelfCertificateID");
        }
    }
}
