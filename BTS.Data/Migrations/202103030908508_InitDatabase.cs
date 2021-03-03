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
                        Id = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        Phone = c.String(maxLength: 30),
                        Fax = c.String(maxLength: 30),
                        ContactName = c.String(maxLength: 100),
                        OperatorID = c.String(nullable: false, maxLength: 20),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.Operators",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 20),
                        RootId = c.String(nullable: false, maxLength: 20),
                        Name = c.String(maxLength: 255),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationGroups",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        Name = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationRoleGroups",
                c => new
                    {
                        GroupId = c.String(nullable: false, maxLength: 36),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
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
                        Description = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ApplicationRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationRoles", t => t.ApplicationRole_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.String(nullable: false, maxLength: 36),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
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
                        FullName = c.String(nullable: false, maxLength: 255),
                        Address = c.String(maxLength: 255),
                        BirthDay = c.DateTime(),
                        FatherLand = c.String(maxLength: 50),
                        Level = c.String(maxLength: 50),
                        EducationalField = c.String(maxLength: 150),
                        EntryDate = c.DateTime(),
                        OfficialDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        WorkingDuration = c.String(maxLength: 50),
                        JobPositions = c.String(maxLength: 255),
                        ImagePath = c.String(maxLength: 555),
                        Locked = c.Boolean(nullable: false),
                        CityIDsScope = c.String(maxLength: 255),
                        AreasScope = c.String(maxLength: 255),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
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
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionID = c.String(),
                        IPAddress = c.String(),
                        UserName = c.String(),
                        URLAccessed = c.String(),
                        TimeAccessed = c.DateTime(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Btss",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ProfileID = c.String(maxLength: 36),
                        OperatorID = c.String(nullable: false, maxLength: 20),
                        BtsCode = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        InCaseOfID = c.Int(nullable: false),
                        LastOwnCertificateIDs = c.String(maxLength: 255),
                        LastNoOwnCertificateIDs = c.String(maxLength: 255),
                        ProfilesInProcess = c.String(maxLength: 255),
                        ReasonsNoCertificate = c.String(maxLength: 255),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.InCaseOfs", t => t.InCaseOfID, cascadeDelete: true)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileID)
                .Index(t => t.ProfileID)
                .Index(t => t.OperatorID)
                .Index(t => t.CityID)
                .Index(t => t.InCaseOfID);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 3),
                        Name = c.String(nullable: false, maxLength: 50),
                        Area = c.String(maxLength: 20),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InCaseOfs",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        ApplicantID = c.String(maxLength: 50),
                        ProfileNum = c.String(maxLength: 30),
                        ProfileDate = c.DateTime(nullable: false, storeType: "date"),
                        BtsQuantity = c.Int(nullable: false),
                        AcceptedBtsQuantity = c.Int(nullable: false),
                        ApplyDate = c.DateTime(storeType: "date"),
                        Fee = c.Long(nullable: false),
                        FeeAnnounceNum = c.String(maxLength: 30),
                        FeeAnnounceDate = c.DateTime(storeType: "date"),
                        FeeReceiptDate = c.DateTime(storeType: "date"),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.ApplicantID)
                .Index(t => t.ApplicantID);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 16),
                        ProfileID = c.String(maxLength: 36),
                        OperatorID = c.String(nullable: false, maxLength: 20),
                        BtsCode = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        InCaseOfID = c.Int(nullable: false),
                        LabID = c.String(maxLength: 20),
                        TestReportNo = c.String(maxLength: 30),
                        TestReportDate = c.DateTime(nullable: false, storeType: "date"),
                        IssuedDate = c.DateTime(nullable: false, storeType: "date"),
                        ExpiredDate = c.DateTime(nullable: false, storeType: "date"),
                        IssuedPlace = c.String(nullable: false, maxLength: 30),
                        Signer = c.String(nullable: false, maxLength: 50),
                        SubBtsQuantity = c.Int(nullable: false),
                        SubBtsCodes = c.String(nullable: false, maxLength: 255),
                        SubBtsOperatorIDs = c.String(nullable: false, maxLength: 150),
                        SubBtsEquipments = c.String(nullable: false, maxLength: 512),
                        SubBtsAntenNums = c.String(nullable: false, maxLength: 150),
                        SubBtsConfigurations = c.String(nullable: false, maxLength: 150),
                        SubBtsPowerSums = c.String(nullable: false, maxLength: 150),
                        SubBtsBands = c.String(nullable: false, maxLength: 256),
                        SubBtsBandsOld = c.String(maxLength: 256),
                        SubBtsAntenHeights = c.String(nullable: false, maxLength: 150),
                        IsPoleOnGround = c.Boolean(nullable: false),
                        IsSafeLimit = c.Boolean(nullable: false),
                        SafeLimitHeight = c.Double(),
                        IsHouseIn100m = c.Boolean(nullable: false),
                        MaxHeightIn100m = c.Double(),
                        MaxPowerSum = c.Double(),
                        IsMeasuringExposure = c.Boolean(nullable: false),
                        MinAntenHeight = c.Double(),
                        OffsetHeight = c.Double(),
                        VerifyUnit = c.String(nullable: false, maxLength: 50),
                        SignerRole = c.String(nullable: false, maxLength: 50),
                        SignerSubRole = c.String(maxLength: 50),
                        Verifier1 = c.String(maxLength: 50),
                        Verifier2 = c.String(maxLength: 50),
                        IsSigned = c.Boolean(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                        CanceledDate = c.DateTime(storeType: "date"),
                        CanceledReason = c.String(maxLength: 150),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
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
                        Id = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(maxLength: 255),
                        Phone = c.String(maxLength: 30),
                        Fax = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 50),
                        CityId = c.String(maxLength: 3),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Tx = c.Int(nullable: false),
                        Band = c.String(maxLength: 30),
                        OperatorRootID = c.String(maxLength: 20),
                        MaxPower = c.Double(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Description = c.String(),
                        Controller = c.String(maxLength: 255),
                        StackTrace = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Email = c.String(maxLength: 250),
                        Message = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Footers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Licences",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        key = c.String(nullable: false),
                        enable = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        URL = c.String(nullable: false, maxLength: 256),
                        DisplayOrder = c.Int(),
                        GroupID = c.Int(nullable: false),
                        Target = c.String(maxLength: 10),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuGroups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.NoCertificates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ProfileID = c.String(maxLength: 36),
                        OperatorID = c.String(nullable: false, maxLength: 20),
                        BtsCode = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        InCaseOfID = c.Int(nullable: false),
                        LabID = c.String(maxLength: 20),
                        TestReportNo = c.String(maxLength: 30),
                        TestReportDate = c.DateTime(nullable: false, storeType: "date"),
                        ReasonNoCertificate = c.String(nullable: false, maxLength: 255),
                        IsSigned = c.Boolean(nullable: false),
                        IsCanceled = c.Boolean(nullable: false),
                        CanceledDate = c.DateTime(storeType: "date"),
                        CanceledReason = c.String(maxLength: 150),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
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
                "dbo.NoRequiredBtss",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        OperatorID = c.String(nullable: false, maxLength: 20),
                        BtsCode = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 255),
                        CityID = c.String(nullable: false, maxLength: 3),
                        Longtitude = c.Double(),
                        Latitude = c.Double(),
                        SubBtsQuantity = c.Int(nullable: false),
                        SubBtsCodes = c.String(nullable: false, maxLength: 255),
                        SubBtsOperatorIDs = c.String(nullable: false, maxLength: 150),
                        SubBtsEquipments = c.String(nullable: false, maxLength: 512),
                        SubBtsAntenNums = c.String(nullable: false, maxLength: 150),
                        SubBtsConfigurations = c.String(nullable: false, maxLength: 150),
                        SubBtsPowerSums = c.String(nullable: false, maxLength: 150),
                        SubBtsBands = c.String(nullable: false, maxLength: 256),
                        SubBtsAntenHeights = c.String(nullable: false, maxLength: 150),
                        AnnouncedDate = c.DateTime(storeType: "date"),
                        AnnouncedDoc = c.String(maxLength: 50),
                        IsCanceled = c.Boolean(nullable: false),
                        CanceledDate = c.DateTime(storeType: "date"),
                        CanceledReason = c.String(maxLength: 150),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityID, cascadeDelete: true)
                .ForeignKey("dbo.Operators", t => t.OperatorID, cascadeDelete: true)
                .Index(t => t.OperatorID)
                .Index(t => t.CityID);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        Image = c.String(maxLength: 256),
                        Url = c.String(maxLength: 256),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubBtsInCerts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CertificateID = c.String(maxLength: 16),
                        BtsSerialNo = c.Int(nullable: false),
                        BtsCode = c.String(maxLength: 50),
                        OperatorID = c.String(maxLength: 20),
                        Manufactory = c.String(maxLength: 50),
                        Equipment = c.String(maxLength: 80),
                        AntenNum = c.Int(nullable: false),
                        Configuration = c.String(maxLength: 30),
                        PowerSum = c.String(maxLength: 30),
                        Band = c.String(maxLength: 60),
                        Technology = c.String(maxLength: 30),
                        AntenHeight = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Certificates", t => t.CertificateID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .Index(t => t.CertificateID)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.SubBtsInNoRequiredBtss",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 36),
                        NoRequiredBtsID = c.String(maxLength: 36),
                        BtsSerialNo = c.Int(nullable: false),
                        BtsCode = c.String(maxLength: 50),
                        OperatorID = c.String(maxLength: 20),
                        Manufactory = c.String(maxLength: 50),
                        Equipment = c.String(maxLength: 80),
                        AntenNum = c.Int(nullable: false),
                        Configuration = c.String(maxLength: 30),
                        PowerSum = c.String(maxLength: 30),
                        Band = c.String(maxLength: 60),
                        Technology = c.String(maxLength: 30),
                        AntenHeight = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NoRequiredBtss", t => t.NoRequiredBtsID)
                .ForeignKey("dbo.Operators", t => t.OperatorID)
                .Index(t => t.NoRequiredBtsID)
                .Index(t => t.OperatorID);
            
            CreateTable(
                "dbo.SupportOnlines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 256),
                        ValueString = c.String(maxLength: 50),
                        ValueInt = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisitorStatistics",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VisitedDate = c.DateTime(nullable: false),
                        IPAddress = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Wards",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 50),
                        DistrictId = c.String(maxLength: 5),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wards", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.SubBtsInNoRequiredBtss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.SubBtsInNoRequiredBtss", "NoRequiredBtsID", "dbo.NoRequiredBtss");
            DropForeignKey("dbo.SubBtsInCerts", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.SubBtsInCerts", "CertificateID", "dbo.Certificates");
            DropForeignKey("dbo.NoRequiredBtss", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.NoRequiredBtss", "CityID", "dbo.Cities");
            DropForeignKey("dbo.NoCertificates", "ProfileID", "dbo.Profiles");
            DropForeignKey("dbo.NoCertificates", "OperatorID", "dbo.Operators");
            DropForeignKey("dbo.NoCertificates", "LabID", "dbo.Labs");
            DropForeignKey("dbo.NoCertificates", "InCaseOfID", "dbo.InCaseOfs");
            DropForeignKey("dbo.NoCertificates", "CityID", "dbo.Cities");
            DropForeignKey("dbo.Menus", "GroupID", "dbo.MenuGroups");
            DropForeignKey("dbo.Districts", "CityId", "dbo.Cities");
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
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationUserRoles", "ApplicationRole_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.Applicants", "OperatorID", "dbo.Operators");
            DropIndex("dbo.Wards", new[] { "DistrictId" });
            DropIndex("dbo.SubBtsInNoRequiredBtss", new[] { "OperatorID" });
            DropIndex("dbo.SubBtsInNoRequiredBtss", new[] { "NoRequiredBtsID" });
            DropIndex("dbo.SubBtsInCerts", new[] { "OperatorID" });
            DropIndex("dbo.SubBtsInCerts", new[] { "CertificateID" });
            DropIndex("dbo.NoRequiredBtss", new[] { "CityID" });
            DropIndex("dbo.NoRequiredBtss", new[] { "OperatorID" });
            DropIndex("dbo.NoCertificates", new[] { "LabID" });
            DropIndex("dbo.NoCertificates", new[] { "InCaseOfID" });
            DropIndex("dbo.NoCertificates", new[] { "CityID" });
            DropIndex("dbo.NoCertificates", new[] { "OperatorID" });
            DropIndex("dbo.NoCertificates", new[] { "ProfileID" });
            DropIndex("dbo.Menus", new[] { "GroupID" });
            DropIndex("dbo.Districts", new[] { "CityId" });
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
            DropIndex("dbo.ApplicationUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserRoles", new[] { "ApplicationRole_Id" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropIndex("dbo.ApplicationRoleGroups", new[] { "GroupId" });
            DropIndex("dbo.Applicants", new[] { "OperatorID" });
            DropTable("dbo.Wards");
            DropTable("dbo.VisitorStatistics");
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.SupportOnlines");
            DropTable("dbo.SubBtsInNoRequiredBtss");
            DropTable("dbo.SubBtsInCerts");
            DropTable("dbo.Slides");
            DropTable("dbo.Pages");
            DropTable("dbo.NoRequiredBtss");
            DropTable("dbo.NoCertificates");
            DropTable("dbo.Menus");
            DropTable("dbo.MenuGroups");
            DropTable("dbo.Licences");
            DropTable("dbo.Footers");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Errors");
            DropTable("dbo.Equipments");
            DropTable("dbo.Districts");
            DropTable("dbo.ContactDetails");
            DropTable("dbo.Labs");
            DropTable("dbo.Certificates");
            DropTable("dbo.Profiles");
            DropTable("dbo.InCaseOfs");
            DropTable("dbo.Cities");
            DropTable("dbo.Btss");
            DropTable("dbo.Audits");
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
