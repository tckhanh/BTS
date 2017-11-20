namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Certificates", "IssuedPlace", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Certificates", "Signer", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Certificates", "SubBtsCodes", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Certificates", "SubBtsOperatorIDs", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsEquipments", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Certificates", "SubBtsAntenNums", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsConfigurations", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsPowerSums", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsBands", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsAntenHeights", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Certificates", "MinAntenHeight", c => c.Double());
            AlterColumn("dbo.Certificates", "MaxHeightIn100m", c => c.Double());
            AlterColumn("dbo.Certificates", "OffsetHeight", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Certificates", "OffsetHeight", c => c.Int(nullable: false));
            AlterColumn("dbo.Certificates", "MaxHeightIn100m", c => c.Int(nullable: false));
            AlterColumn("dbo.Certificates", "MinAntenHeight", c => c.Int(nullable: false));
            AlterColumn("dbo.Certificates", "SubBtsAntenHeights", c => c.String(maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsBands", c => c.String(maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsPowerSums", c => c.String(maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsConfigurations", c => c.String(maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsAntenNums", c => c.String(maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsEquipments", c => c.String(maxLength: 255));
            AlterColumn("dbo.Certificates", "SubBtsOperatorIDs", c => c.String(maxLength: 150));
            AlterColumn("dbo.Certificates", "SubBtsCodes", c => c.String(maxLength: 255));
            AlterColumn("dbo.Certificates", "Signer", c => c.String(maxLength: 30));
            AlterColumn("dbo.Certificates", "IssuedPlace", c => c.String(maxLength: 30));
        }
    }
}
