namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OperatorID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        Telephone = c.String(maxLength: 30),
                        Fax = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.Operators",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 10),
                        Name = c.String(maxLength: 255),
                        Address = c.String(maxLength: 255),
                        Telephone = c.String(maxLength: 50),
                        Fax = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BTSCertificates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        DistrictID = c.Int(),
                        SubBTSNum = c.Int(),
                        InCaseOf = c.String(maxLength: 10),
                        ReportNum = c.String(maxLength: 30),
                        ReportDate = c.DateTime(storeType: "date"),
                        CertificateNum = c.String(maxLength: 16),
                        SafeLimit = c.Double(),
                        IssuedPlace = c.String(maxLength: 30),
                        IssuedDate = c.DateTime(storeType: "date"),
                        ExpiredDate = c.DateTime(storeType: "date"),
                        Signer = c.String(maxLength: 30),
                        OperatorID = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.Dítricts", t => t.DistrictID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.CityID)
                .Index(t => t.DistrictID)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 3),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Dítricts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CityID = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.CityID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicantID = c.Int(),
                        ProfileNum = c.String(maxLength: 30),
                        ProfileDate = c.DateTime(storeType: "date"),
                        ApplyDate = c.DateTime(storeType: "date"),
                        BTSNum = c.Int(),
                        AdminNum = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Applicants", t => t.ApplicantID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.SubBTSs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OperatorID = c.String(maxLength: 10),
                        BTSCertificateID = c.Int(),
                        BTSCode = c.String(maxLength: 50),
                        Equipment = c.String(maxLength: 50),
                        AntenNum = c.Int(),
                        Configuration = c.String(maxLength: 30),
                        PowerSum = c.String(maxLength: 30),
                        Band = c.String(maxLength: 30),
                        HeightAnten = c.String(maxLength: 30),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.BTSCertificates", t => t.BTSCertificateID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .Index(t => t.OperatorID)
                .Index(t => t.BTSCertificateID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubBTSs", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.SubBTSs", "BTSCertificateID", "dbo.BTSCertificates");
            DropForeignKey("dbo.BTSCertificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "ApplicantID", "dbo.Applicants");
            DropForeignKey("dbo.BTSCertificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Dítricts", "CityID", "dbo.Cities");
            DropForeignKey("dbo.BTSCertificates", "DistrictID", "dbo.Dítricts");
            DropForeignKey("dbo.BTSCertificates", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators");
            DropIndex("dbo.SubBTSs", new[] { "BTSCertificateID" });
            DropIndex("dbo.SubBTSs", new[] { "OperatorID" });
            DropIndex("dbo.Profiles", new[] { "ApplicantID" });
            DropIndex("dbo.Dítricts", new[] { "CityID" });
            DropIndex("dbo.BTSCertificates", new[] { "OperatorID" });
            DropIndex("dbo.BTSCertificates", new[] { "DistrictID" });
            DropIndex("dbo.BTSCertificates", new[] { "CityID" });
            DropIndex("dbo.BTSCertificates", new[] { "ProfileID" });
            DropIndex("dbo.Applicants", new[] { "OperatorID" });
            DropTable("dbo.SubBTSs");
            DropTable("dbo.Profiles");
            DropTable("dbo.Dítricts");
            DropTable("dbo.Cities");
            DropTable("dbo.BTSCertificates");
            DropTable("dbo.Operators");
            DropTable("dbo.Applicants");
        }
    }
}
