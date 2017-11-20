namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Certificates", "SubBTSCodes", c => c.String(maxLength: 255));
            AddColumn("dbo.Certificates", "SubBTSOperatorIDs", c => c.String(maxLength: 150));
            AddColumn("dbo.Certificates", "SubBTSEquipments", c => c.String(maxLength: 255));
            AddColumn("dbo.Certificates", "SubBTSAntenNums", c => c.String(maxLength: 150));
            AddColumn("dbo.Certificates", "SubBTSConfigurations", c => c.String(maxLength: 150));
            AddColumn("dbo.Certificates", "SubBTSPowerSums", c => c.String(maxLength: 150));
            AddColumn("dbo.Certificates", "SubBTSBands", c => c.String(maxLength: 150));
            AddColumn("dbo.Certificates", "SubBTSAntenHeights", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Certificates", "SubBTSAntenHeights");
            DropColumn("dbo.Certificates", "SubBTSBands");
            DropColumn("dbo.Certificates", "SubBTSPowerSums");
            DropColumn("dbo.Certificates", "SubBTSConfigurations");
            DropColumn("dbo.Certificates", "SubBTSAntenNums");
            DropColumn("dbo.Certificates", "SubBTSEquipments");
            DropColumn("dbo.Certificates", "SubBTSOperatorIDs");
            DropColumn("dbo.Certificates", "SubBTSCodes");
        }
    }
}
