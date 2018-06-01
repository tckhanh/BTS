namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        Phone = c.String(maxLength: 30),
                        Fax = c.String(maxLength: 30),
                        ContactName = c.String(maxLength: 100),
                        OperatorID = c.String(nullable: false, maxLength: 10),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
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
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationGroups",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        GroupId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GroupId, t.RoleId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(maxLength: 250),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationRoles", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.GroupId })
                .ForeignKey("dbo.ApplicationGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                        FullName = c.String(maxLength: 255),
                        Address = c.String(maxLength: 255),
                        BirthDay = c.DateTime(),
                        FatherLand = c.String(maxLength: 50),
                        Level = c.String(maxLength: 50),
                        EducationalField = c.String(maxLength: 150),
                        EntryDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        JobPositions = c.String(maxLength: 255),
                        ImagePath = c.String(maxLength: 555),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserClaims",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        IdentityUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.IdentityUser_Id)
                .Index(t => t.IdentityUser_Id);
            
            CreateTable(
                "dbo.Btss",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(nullable: false),
                        OperatorID = c.String(nullable: false, maxLength: 10),
                        BtsCode = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        InCaseOfID = c.Int(nullable: false),
                        IssuedCertificateID = c.String(maxLength: 16),
                        LastOwnCertificateID = c.String(maxLength: 16),
                        LastOwnOperatorID = c.String(maxLength: 10),
                        LastNoOwnCertificateID = c.String(maxLength: 16),
                        LastNoOwnOperatorID = c.String(maxLength: 10),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.InCaseOfs", t => t.InCaseOfID, cascadeDelete: true)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID, cascadeDelete: true)
                .Index(t => t.ProfileID)
                .Index(t => t.OperatorID)
                .Index(t => t.CityID)
                .Index(t => t.InCaseOfID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 3),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.InCaseOfs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplicantID = c.String(maxLength: 50),
                        ProfileNum = c.String(maxLength: 30),
                        ProfileDate = c.DateTime(nullable: false, storeType: "date"),
                        BtsQuantity = c.Int(),
                        ApplyDate = c.DateTime(nullable: false, storeType: "date"),
                        Fee = c.Int(),
                        FeeAnnounceNum = c.String(maxLength: 30),
                        FeeAnnounceDate = c.DateTime(storeType: "date"),
                        FeeReceiptDate = c.DateTime(storeType: "date"),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Applicants", t => t.ApplicantID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 16),
                        ProfileID = c.Int(),
                        OperatorID = c.String(nullable: false, maxLength: 10),
                        BtsCode = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        InCaseOfID = c.Int(nullable: false),
                        LabID = c.String(maxLength: 20),
                        TestReportNo = c.String(maxLength: 30),
                        TestReportDate = c.DateTime(nullable: false, storeType: "date"),
                        IssuedDate = c.DateTime(storeType: "date"),
                        ExpiredDate = c.DateTime(storeType: "date"),
                        IssuedPlace = c.String(nullable: false, maxLength: 30),
                        Signer = c.String(nullable: false, maxLength: 30),
                        SubBtsQuantity = c.Int(nullable: false),
                        SubBtsCodes = c.String(nullable: false, maxLength: 255),
                        SubBtsOperatorIDs = c.String(nullable: false, maxLength: 150),
                        SubBtsEquipments = c.String(nullable: false, maxLength: 255),
                        SubBtsAntenNums = c.String(nullable: false, maxLength: 150),
                        SubBtsConfigurations = c.String(nullable: false, maxLength: 150),
                        SubBtsPowerSums = c.String(nullable: false, maxLength: 150),
                        SubBtsBands = c.String(nullable: false, maxLength: 150),
                        SubBtsAntenHeights = c.String(nullable: false, maxLength: 150),
                        MinAntenHeight = c.Double(),
                        MaxHeightIn100m = c.Double(),
                        OffsetHeight = c.Double(),
                        SafeLimit = c.Double(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.InCaseOfs", t => t.InCaseOfID, cascadeDelete: true)
                .ForeignKey("dbo.Labs", t => t.LabID)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.OperatorID)
                .Index(t => t.CityID)
                .Index(t => t.InCaseOfID)
                .Index(t => t.LabID);
            
            CreateTable(
                "dbo.Labs",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(maxLength: 255),
                        Phone = c.String(maxLength: 30),
                        Fax = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ContactDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Phone = c.String(maxLength: 50),
                        Email = c.String(maxLength: 250),
                        Website = c.String(maxLength: 250),
                        Address = c.String(maxLength: 250),
                        Other = c.String(),
                        Lat = c.Double(),
                        Lng = c.Double(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Description = c.String(),
                        Controller = c.String(maxLength: 255),
                        StackTrace = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Email = c.String(maxLength: 250),
                        Message = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Footers",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MenuGroups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 256),
                        DisplayOrder = c.Int(),
                        GroupID = c.Int(nullable: false),
                        Target = c.String(maxLength: 10),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuGroups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.NoCertificates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProfileID = c.Int(),
                        OperatorID = c.String(nullable: false, maxLength: 10),
                        BtsCode = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        InCaseOfID = c.Int(nullable: false),
                        LabID = c.String(maxLength: 20),
                        TestReportNo = c.String(maxLength: 30),
                        TestReportDate = c.DateTime(nullable: false, storeType: "date"),
                        Reason = c.String(nullable: false, maxLength: 255),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.InCaseOfs", t => t.InCaseOfID, cascadeDelete: true)
                .ForeignKey("dbo.Labs", t => t.LabID)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.OperatorID)
                .Index(t => t.CityID)
                .Index(t => t.InCaseOfID)
                .Index(t => t.LabID);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Alias = c.String(nullable: false, maxLength: 256, unicode: false),
                        Content = c.String(),
                        MetaKeyword = c.String(maxLength: 256),
                        MetaDescription = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                        Url = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SubBtsInCerts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CertificateID = c.String(maxLength: 16),
                        BtsCode = c.String(maxLength: 50),
                        OperatorID = c.String(maxLength: 10),
                        Manufactory = c.String(maxLength: 50),
                        Equipment = c.String(maxLength: 50),
                        AntenNum = c.Int(),
                        Configuration = c.String(maxLength: 30),
                        PowerSum = c.String(maxLength: 30),
                        Band = c.String(maxLength: 30),
                        AntenHeight = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Certificates", t => t.CertificateID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .Index(t => t.CertificateID)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.SupportOnlines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Department = c.String(maxLength: 50),
                        Skype = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Yahoo = c.String(maxLength: 50),
                        Facebook = c.String(maxLength: 50),
                        Status = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        ValueString = c.String(maxLength: 50),
                        ValueInt = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VisitorStatistics",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        VisitedDate = c.DateTime(nullable: false),
                        IPAddress = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "IdentityUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "IdentityUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.SubBtsInCerts", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.SubBtsInCerts", "CertificateID", "dbo.Certificates");
            DropForeignKey("dbo.ApplicationUserRoles", "IdentityRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.NoCertificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.NoCertificates", "LabID", "dbo.Labs");
            DropForeignKey("dbo.NoCertificates", "InCaseOfID", "dbo.InCaseOfs");
            DropForeignKey("dbo.NoCertificates", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Menus", "GroupID", "dbo.MenuGroups");
            DropForeignKey("dbo.Certificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Certificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Certificates", "LabID", "dbo.Labs");
            DropForeignKey("dbo.Certificates", "InCaseOfID", "dbo.InCaseOfs");
            DropForeignKey("dbo.Certificates", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Btss", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "ApplicantID", "dbo.Applicants");
            DropForeignKey("dbo.Btss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.Btss", "InCaseOfID", "dbo.InCaseOfs");
            DropForeignKey("dbo.Btss", "CityID", "dbo.Cities");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators");
            DropIndex("dbo.SubBtsInCerts", new[] { "OperatorID" });
            DropIndex("dbo.SubBtsInCerts", new[] { "CertificateID" });
            DropIndex("dbo.NoCertificates", new[] { "LabID" });
            DropIndex("dbo.NoCertificates", new[] { "InCaseOfID" });
            DropIndex("dbo.NoCertificates", new[] { "CityID" });
            DropIndex("dbo.NoCertificates", new[] { "OperatorID" });
            DropIndex("dbo.NoCertificates", new[] { "ProfileID" });
            DropIndex("dbo.Menus", new[] { "GroupID" });
            DropIndex("dbo.Certificates", new[] { "LabID" });
            DropIndex("dbo.Certificates", new[] { "InCaseOfID" });
            DropIndex("dbo.Certificates", new[] { "CityID" });
            DropIndex("dbo.Certificates", new[] { "OperatorID" });
            DropIndex("dbo.Certificates", new[] { "ProfileID" });
            DropIndex("dbo.Profiles", new[] { "ApplicantID" });
            DropIndex("dbo.Btss", new[] { "InCaseOfID" });
            DropIndex("dbo.Btss", new[] { "CityID" });
            DropIndex("dbo.Btss", new[] { "OperatorID" });
            DropIndex("dbo.Btss", new[] { "ProfileID" });
            DropIndex("dbo.ApplicationUserLogins", new[] { "IdentityUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "IdentityUser_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityUser_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.Applicants", new[] { "OperatorID" });
            DropTable("dbo.VisitorStatistics");
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.SupportOnlines");
            DropTable("dbo.SubBtsInCerts");
            DropTable("dbo.Slides");
            DropTable("dbo.Pages");
            DropTable("dbo.NoCertificates");
            DropTable("dbo.Menus");
            DropTable("dbo.MenuGroups");
            DropTable("dbo.Footers");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Errors");
            DropTable("dbo.ContactDetails");
            DropTable("dbo.Labs");
            DropTable("dbo.Certificates");
            DropTable("dbo.Profiles");
            DropTable("dbo.InCaseOfs");
            DropTable("dbo.Cities");
            DropTable("dbo.Btss");
            DropTable("dbo.ApplicationUserLogins");
            DropTable("dbo.ApplicationUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.ApplicationUserGroups");
            DropTable("dbo.ApplicationUserRoles");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ApplicationRoleGroups");
            DropTable("dbo.ApplicationGroups");
            DropTable("dbo.Operators");
            DropTable("dbo.Applicants");
        }
    }
}