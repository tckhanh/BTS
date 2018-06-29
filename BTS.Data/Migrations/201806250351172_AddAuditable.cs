namespace BTS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuditable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationGroups", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationGroups", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ApplicationGroups", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationGroups", "UpdatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ApplicationRoleGroups", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationRoleGroups", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ApplicationRoleGroups", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationRoleGroups", "UpdatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ApplicationRoles", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationRoles", "CreatedBy", c => c.String());
            AddColumn("dbo.ApplicationRoles", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationRoles", "UpdatedBy", c => c.String());
            AddColumn("dbo.ApplicationUserGroups", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUserGroups", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ApplicationUserGroups", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUserGroups", "UpdatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ApplicationUsers", "OfficialDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUsers", "WorkingDuration", c => c.String(maxLength: 50));
            AddColumn("dbo.ApplicationUsers", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUsers", "CreatedBy", c => c.String());
            AddColumn("dbo.ApplicationUsers", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ApplicationUsers", "UpdatedBy", c => c.String());
            AddColumn("dbo.Btss", "ProFilesInProcess", c => c.String(maxLength: 255));
            AddColumn("dbo.Btss", "ReasonsNoCertificate", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Btss", "ReasonsNoCertificate");
            DropColumn("dbo.Btss", "ProFilesInProcess");
            DropColumn("dbo.ApplicationUsers", "UpdatedBy");
            DropColumn("dbo.ApplicationUsers", "UpdatedDate");
            DropColumn("dbo.ApplicationUsers", "CreatedBy");
            DropColumn("dbo.ApplicationUsers", "CreatedDate");
            DropColumn("dbo.ApplicationUsers", "WorkingDuration");
            DropColumn("dbo.ApplicationUsers", "OfficialDate");
            DropColumn("dbo.ApplicationUserGroups", "UpdatedBy");
            DropColumn("dbo.ApplicationUserGroups", "UpdatedDate");
            DropColumn("dbo.ApplicationUserGroups", "CreatedBy");
            DropColumn("dbo.ApplicationUserGroups", "CreatedDate");
            DropColumn("dbo.ApplicationRoles", "UpdatedBy");
            DropColumn("dbo.ApplicationRoles", "UpdatedDate");
            DropColumn("dbo.ApplicationRoles", "CreatedBy");
            DropColumn("dbo.ApplicationRoles", "CreatedDate");
            DropColumn("dbo.ApplicationRoleGroups", "UpdatedBy");
            DropColumn("dbo.ApplicationRoleGroups", "UpdatedDate");
            DropColumn("dbo.ApplicationRoleGroups", "CreatedBy");
            DropColumn("dbo.ApplicationRoleGroups", "CreatedDate");
            DropColumn("dbo.ApplicationGroups", "UpdatedBy");
            DropColumn("dbo.ApplicationGroups", "UpdatedDate");
            DropColumn("dbo.ApplicationGroups", "CreatedBy");
            DropColumn("dbo.ApplicationGroups", "CreatedDate");
        }
    }
}
