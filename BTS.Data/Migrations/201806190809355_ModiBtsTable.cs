namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModiBtsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Btss", "LastOwnCertificateIDs", c => c.String(maxLength: 255));
            AddColumn("dbo.Btss", "LastNoOwnCertificateIDs", c => c.String(maxLength: 255));
            DropColumn("dbo.Btss", "IssuedCertificateID");
            DropColumn("dbo.Btss", "LastOwnCertificateID");
            DropColumn("dbo.Btss", "LastOwnOperatorID");
            DropColumn("dbo.Btss", "LastNoOwnCertificateID");
            DropColumn("dbo.Btss", "LastNoOwnOperatorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Btss", "LastNoOwnOperatorID", c => c.String(maxLength: 10));
            AddColumn("dbo.Btss", "LastNoOwnCertificateID", c => c.String(maxLength: 16));
            AddColumn("dbo.Btss", "LastOwnOperatorID", c => c.String(maxLength: 10));
            AddColumn("dbo.Btss", "LastOwnCertificateID", c => c.String(maxLength: 16));
            AddColumn("dbo.Btss", "IssuedCertificateID", c => c.String(maxLength: 16));
            DropColumn("dbo.Btss", "LastNoOwnCertificateIDs");
            DropColumn("dbo.Btss", "LastOwnCertificateIDs");
        }
    }
}
