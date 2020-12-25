namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Btss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Certificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.NoCertificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.SubBtsInCerts", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles");
            DropIndex("dbo.Applicants", new[] { "OperatorID" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.Btss", new[] { "ProfileID" });
            DropIndex("dbo.Btss", new[] { "OperatorID" });
            DropIndex("dbo.Certificates", new[] { "ProfileID" });
            DropIndex("dbo.Certificates", new[] { "OperatorID" });
            DropIndex("dbo.NoCertificates", new[] { "ProfileID" });
            DropIndex("dbo.NoCertificates", new[] { "OperatorID" });
            DropIndex("dbo.SubBtsInCerts", new[] { "OperatorID" });
            DropPrimaryKey("dbo.Operators");
            DropPrimaryKey("dbo.ApplicationGroups");
            DropPrimaryKey("dbo.ApplicationRoleGroups");
            DropPrimaryKey("dbo.ApplicationUserGroups");
            DropPrimaryKey("dbo.Btss");
            DropPrimaryKey("dbo.Profiles");
            DropPrimaryKey("dbo.NoCertificates");
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionID = c.String(unicode: false),
                        IPAddress = c.String(unicode: false),
                        UserName = c.String(unicode: false),
                        URLAccessed = c.String(unicode: false),
                        TimeAccessed = c.DateTime(nullable: false, precision: 0),
                        Data = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        CityId = c.String(maxLength: 3, storeType: "nvarchar"),
                        CreatedDate = c.DateTime(precision: 0),
                        CreatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                        UpdatedDate = c.DateTime(precision: 0),
                        UpdatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        Tx = c.Int(nullable: false),
                        Band = c.String(maxLength: 30, storeType: "nvarchar"),
                        OperatorRootID = c.String(maxLength: 20, storeType: "nvarchar"),
                        MaxPower = c.Double(nullable: false),
                        CreatedDate = c.DateTime(precision: 0),
                        CreatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                        UpdatedDate = c.DateTime(precision: 0),
                        UpdatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Licences",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36, storeType: "nvarchar"),
                        key = c.String(nullable: false, unicode: false),
                        enable = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(precision: 0),
                        CreatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                        UpdatedDate = c.DateTime(precision: 0),
                        UpdatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoRequiredBtss",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36, storeType: "nvarchar"),
                        OperatorID = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        BtsCode = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Address = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        CityID = c.String(nullable: false, maxLength: 3, storeType: "nvarchar"),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        SubBtsQuantity = c.Int(nullable: false),
                        SubBtsCodes = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        SubBtsOperatorIDs = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        SubBtsEquipments = c.String(nullable: false, maxLength: 512, storeType: "nvarchar"),
                        SubBtsAntenNums = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        SubBtsConfigurations = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        SubBtsPowerSums = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        SubBtsBands = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        SubBtsAntenHeights = c.String(nullable: false, maxLength: 150, storeType: "nvarchar"),
                        AnnouncedDate = c.DateTime(storeType: "date"),
                        AnnouncedDoc = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsCanceled = c.Boolean(nullable: false),
                        CanceledDate = c.DateTime(storeType: "date"),
                        CanceledReason = c.String(maxLength: 150, storeType: "nvarchar"),
                        CreatedDate = c.DateTime(precision: 0),
                        CreatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                        UpdatedDate = c.DateTime(precision: 0),
                        UpdatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .Index(t => t.OperatorID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.SubBtsInNoRequiredBtss",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36, storeType: "nvarchar"),
                        NoRequiredBtsID = c.String(maxLength: 36, storeType: "nvarchar"),
                        BtsSerialNo = c.Int(nullable: false),
                        BtsCode = c.String(maxLength: 50, storeType: "nvarchar"),
                        OperatorID = c.String(maxLength: 20, storeType: "nvarchar"),
                        Manufactory = c.String(maxLength: 50, storeType: "nvarchar"),
                        Equipment = c.String(maxLength: 80, storeType: "nvarchar"),
                        AntenNum = c.Int(nullable: false),
                        Configuration = c.String(maxLength: 30, storeType: "nvarchar"),
                        PowerSum = c.String(maxLength: 30, storeType: "nvarchar"),
                        Band = c.String(maxLength: 60, storeType: "nvarchar"),
                        Technology = c.String(maxLength: 30, storeType: "nvarchar"),
                        AntenHeight = c.String(maxLength: 30, storeType: "nvarchar"),
                        CreatedDate = c.DateTime(precision: 0),
                        CreatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                        UpdatedDate = c.DateTime(precision: 0),
                        UpdatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NoRequiredBtss", t => t.NoRequiredBtsID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .Index(t => t.NoRequiredBtsID)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.Wards",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        DistrictId = c.String(maxLength: 5, storeType: "nvarchar"),
                        CreatedDate = c.DateTime(precision: 0),
                        CreatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                        UpdatedDate = c.DateTime(precision: 0),
                        UpdatedBy = c.String(maxLength: 256, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .Index(t => t.DistrictId);
            
            AddColumn("dbo.Operators", "RootId", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
            AddColumn("dbo.ApplicationUsers", "CityIDsScope", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.ApplicationUsers", "AreasScope", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AddColumn("dbo.Cities", "Area", c => c.String(maxLength: 20, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "SubBtsBandsOld", c => c.String(maxLength: 256, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "IsMeasuringExposure", c => c.Boolean(nullable: false));
            AddColumn("dbo.Certificates", "VerifyUnit", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "SignerRole", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "SignerSubRole", c => c.String(maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "Verifier1", c => c.String(maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "Verifier2", c => c.String(maxLength: 50, storeType: "nvarchar"));
            AddColumn("dbo.Certificates", "IsSigned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Certificates", "IsCanceled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Certificates", "CanceledDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Certificates", "CanceledReason", c => c.String(maxLength: 150, storeType: "nvarchar"));
            AddColumn("dbo.NoCertificates", "IsSigned", c => c.Boolean(nullable: false));
            AddColumn("dbo.NoCertificates", "IsCanceled", c => c.Boolean(nullable: false));
            AddColumn("dbo.NoCertificates", "CanceledDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.NoCertificates", "CanceledReason", c => c.String(maxLength: 150, storeType: "nvarchar"));
            AddColumn("dbo.SubBtsInCerts", "BtsSerialNo", c => c.Int(nullable: false));
            AddColumn("dbo.SubBtsInCerts", "Technology", c => c.String(maxLength: 30, storeType: "nvarchar"));
            AddColumn("dbo.SystemConfigs", "Description", c => c.String(nullable: false, maxLength: 256, storeType: "nvarchar"));
            AlterColumn("dbo.Applicants", "OperatorID", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.Operators", "Id", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.ApplicationGroups", "Id", c => c.String(nullable: false, maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.ApplicationRoleGroups", "GroupId", c => c.String(nullable: false, maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.ApplicationUserGroups", "GroupId", c => c.String(nullable: false, maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.Btss", "Id", c => c.String(nullable: false, maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.Btss", "ProfileID", c => c.String(maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.Btss", "OperatorID", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.Profiles", "Id", c => c.String(nullable: false, maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.Profiles", "ApplyDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Profiles", "Fee", c => c.Long(nullable: false));
            AlterColumn("dbo.Certificates", "ProfileID", c => c.String(maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "OperatorID", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "IssuedDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Certificates", "ExpiredDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Certificates", "Signer", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "SubBtsEquipments", c => c.String(nullable: false, maxLength: 512, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "SubBtsBands", c => c.String(nullable: false, maxLength: 256, storeType: "nvarchar"));
            AlterColumn("dbo.NoCertificates", "Id", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.NoCertificates", "ProfileID", c => c.String(maxLength: 36, storeType: "nvarchar"));
            AlterColumn("dbo.NoCertificates", "OperatorID", c => c.String(nullable: false, maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.SubBtsInCerts", "OperatorID", c => c.String(maxLength: 20, storeType: "nvarchar"));
            AlterColumn("dbo.SubBtsInCerts", "Equipment", c => c.String(maxLength: 80, storeType: "nvarchar"));
            AlterColumn("dbo.SubBtsInCerts", "AntenNum", c => c.Int(nullable: false));
            AlterColumn("dbo.SubBtsInCerts", "Band", c => c.String(maxLength: 60, storeType: "nvarchar"));
            AddPrimaryKey("dbo.Operators", "Id");
            AddPrimaryKey("dbo.ApplicationGroups", "Id");
            AddPrimaryKey("dbo.ApplicationRoleGroups", new[] { "GroupId", "RoleId" });
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "UserId", "GroupId" });
            AddPrimaryKey("dbo.Btss", "Id");
            AddPrimaryKey("dbo.Profiles", "Id");
            AddPrimaryKey("dbo.NoCertificates", "Id");
            CreateIndex("dbo.Applicants", "OperatorID");
            CreateIndex("dbo.ApplicationRoleGroups", "GroupId");
            CreateIndex("dbo.ApplicationUserGroups", "GroupId");
            CreateIndex("dbo.Btss", "ProfileID");
            CreateIndex("dbo.Btss", "OperatorID");
            CreateIndex("dbo.Certificates", "ProfileID");
            CreateIndex("dbo.Certificates", "OperatorID");
            CreateIndex("dbo.NoCertificates", "ProfileID");
            CreateIndex("dbo.NoCertificates", "OperatorID");
            CreateIndex("dbo.SubBtsInCerts", "OperatorID");
            AddForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Btss", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Certificates", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NoCertificates", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SubBtsInCerts", "OperatorID", "dbo.Operators", "Id");
            AddForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles", "Id");
            AddForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles", "Id");
            DropColumn("dbo.Certificates", "SharedAntens");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Certificates", "SharedAntens", c => c.String(maxLength: 50, storeType: "nvarchar"));
            DropForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.SubBtsInCerts", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.NoCertificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Certificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Btss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Wards", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.SubBtsInNoRequiredBtss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.SubBtsInNoRequiredBtss", "NoRequiredBtsID", "dbo.NoRequiredBtss");
            DropForeignKey("dbo.NoRequiredBtss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.NoRequiredBtss", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Districts", "CityId", "dbo.Cities");
            DropIndex("dbo.Wards", new[] { "DistrictId" });
            DropIndex("dbo.SubBtsInNoRequiredBtss", new[] { "OperatorID" });
            DropIndex("dbo.SubBtsInNoRequiredBtss", new[] { "NoRequiredBtsID" });
            DropIndex("dbo.SubBtsInCerts", new[] { "OperatorID" });
            DropIndex("dbo.NoRequiredBtss", new[] { "CityID" });
            DropIndex("dbo.NoRequiredBtss", new[] { "OperatorID" });
            DropIndex("dbo.NoCertificates", new[] { "OperatorID" });
            DropIndex("dbo.NoCertificates", new[] { "ProfileID" });
            DropIndex("dbo.Districts", new[] { "CityId" });
            DropIndex("dbo.Certificates", new[] { "OperatorID" });
            DropIndex("dbo.Certificates", new[] { "ProfileID" });
            DropIndex("dbo.Btss", new[] { "OperatorID" });
            DropIndex("dbo.Btss", new[] { "ProfileID" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.Applicants", new[] { "OperatorID" });
            DropPrimaryKey("dbo.NoCertificates");
            DropPrimaryKey("dbo.Profiles");
            DropPrimaryKey("dbo.Btss");
            DropPrimaryKey("dbo.ApplicationUserGroups");
            DropPrimaryKey("dbo.ApplicationRoleGroups");
            DropPrimaryKey("dbo.ApplicationGroups");
            DropPrimaryKey("dbo.Operators");
            AlterColumn("dbo.SubBtsInCerts", "Band", c => c.String(maxLength: 30, storeType: "nvarchar"));
            AlterColumn("dbo.SubBtsInCerts", "AntenNum", c => c.Int());
            AlterColumn("dbo.SubBtsInCerts", "Equipment", c => c.String(maxLength: 50, storeType: "nvarchar"));
            AlterColumn("dbo.SubBtsInCerts", "OperatorID", c => c.String(maxLength: 10, storeType: "nvarchar"));
            AlterColumn("dbo.NoCertificates", "OperatorID", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
            AlterColumn("dbo.NoCertificates", "ProfileID", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.NoCertificates", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Certificates", "SubBtsBands", c => c.String(nullable: false, maxLength: 150, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "SubBtsEquipments", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "Signer", c => c.String(nullable: false, maxLength: 30, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "ExpiredDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Certificates", "IssuedDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Certificates", "OperatorID", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
            AlterColumn("dbo.Certificates", "ProfileID", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.Profiles", "Fee", c => c.Int(nullable: false));
            AlterColumn("dbo.Profiles", "ApplyDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Profiles", "Id", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.Btss", "OperatorID", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
            AlterColumn("dbo.Btss", "ProfileID", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.Btss", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ApplicationUserGroups", "GroupId", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.ApplicationRoleGroups", "GroupId", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.ApplicationGroups", "Id", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.Operators", "Id", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
            AlterColumn("dbo.Applicants", "OperatorID", c => c.String(nullable: false, maxLength: 10, storeType: "nvarchar"));
            DropColumn("dbo.SystemConfigs", "Description");
            DropColumn("dbo.SubBtsInCerts", "Technology");
            DropColumn("dbo.SubBtsInCerts", "BtsSerialNo");
            DropColumn("dbo.NoCertificates", "CanceledReason");
            DropColumn("dbo.NoCertificates", "CanceledDate");
            DropColumn("dbo.NoCertificates", "IsCanceled");
            DropColumn("dbo.NoCertificates", "IsSigned");
            DropColumn("dbo.Certificates", "CanceledReason");
            DropColumn("dbo.Certificates", "CanceledDate");
            DropColumn("dbo.Certificates", "IsCanceled");
            DropColumn("dbo.Certificates", "IsSigned");
            DropColumn("dbo.Certificates", "Verifier2");
            DropColumn("dbo.Certificates", "Verifier1");
            DropColumn("dbo.Certificates", "SignerSubRole");
            DropColumn("dbo.Certificates", "SignerRole");
            DropColumn("dbo.Certificates", "VerifyUnit");
            DropColumn("dbo.Certificates", "IsMeasuringExposure");
            DropColumn("dbo.Certificates", "SubBtsBandsOld");
            DropColumn("dbo.Cities", "Area");
            DropColumn("dbo.ApplicationUsers", "AreasScope");
            DropColumn("dbo.ApplicationUsers", "CityIDsScope");
            DropColumn("dbo.Operators", "RootId");
            DropTable("dbo.Wards");
            DropTable("dbo.SubBtsInNoRequiredBtss");
            DropTable("dbo.NoRequiredBtss");
            DropTable("dbo.Licences");
            DropTable("dbo.Equipments");
            DropTable("dbo.Districts");
            DropTable("dbo.Audits");
            AddPrimaryKey("dbo.NoCertificates", "Id");
            AddPrimaryKey("dbo.Profiles", "Id");
            AddPrimaryKey("dbo.Btss", "Id");
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "UserId", "GroupId" });
            AddPrimaryKey("dbo.ApplicationRoleGroups", new[] { "GroupId", "RoleId" });
            AddPrimaryKey("dbo.ApplicationGroups", "Id");
            AddPrimaryKey("dbo.Operators", "Id");
            CreateIndex("dbo.SubBtsInCerts", "OperatorID");
            CreateIndex("dbo.NoCertificates", "OperatorID");
            CreateIndex("dbo.NoCertificates", "ProfileID");
            CreateIndex("dbo.Certificates", "OperatorID");
            CreateIndex("dbo.Certificates", "ProfileID");
            CreateIndex("dbo.Btss", "OperatorID");
            CreateIndex("dbo.Btss", "ProfileID");
            CreateIndex("dbo.ApplicationUserGroups", "GroupId");
            CreateIndex("dbo.ApplicationRoleGroups", "GroupId");
            CreateIndex("dbo.Applicants", "OperatorID");
            AddForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles", "Id");
            AddForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles", "Id");
            AddForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SubBtsInCerts", "OperatorID", "dbo.Operators", "Id");
            AddForeignKey("dbo.NoCertificates", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Certificates", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Btss", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators", "Id", cascadeDelete: true);
        }
    }
}
