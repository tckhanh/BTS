private void CreateRolesandUsers1(BTSDbContext context)
{
    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
    var role = new ApplicationRole();
    var newGroup = new ApplicationGroup();

    //    var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //    var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

    if (!roleManager.RoleExists(CommonConstants.System_Admin_Role))
    {
        role = new ApplicationRole(CommonConstants.System_Admin_Role, CommonConstants.System_Admin_Role_Description);
        roleManager.Create(role);
    }

    var user = userManager.FindByName(CommonConstants.SuperAdmin_Name);
    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = CommonConstants.SuperAdmin_Name,
            FullName = CommonConstants.SuperAdmin_FullName,
            Email = CommonConstants.SuperAdmin_Email,
            EmailConfirmed = CommonConstants.SuperAdmin_EmailConfirmed,
        };
        var result = userManager.Create(user, CommonConstants.SuperAdmin_Password);
        result = userManager.SetLockoutEnabled(user.Id, false);
    }

    newGroup = new ApplicationGroup(CommonConstants.SUPERADMIN_GROUP, CommonConstants.SUPERADMIN_GROUP_NAME);
    groupManager.CreateGroup(newGroup);
    groupManager.SetUserGroups(user.Id, new string[] { newGroup.ID });
    groupManager.SetGroupRoles(newGroup.ID, new string[] { CommonConstants.System_Admin_Role });

    //********************
    if (!roleManager.RoleExists(CommonConstants.Data_CanView_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanView_Role, CommonConstants.Data_CanView_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanViewDetail_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanViewDetail_Role, CommonConstants.Data_CanViewDetail_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanViewChart_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanViewChart_Role, CommonConstants.Data_CanViewChart_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanViewStatitics_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanViewStatitics_Role, CommonConstants.Data_CanViewStatitics_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanAdd_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanAdd_Role, CommonConstants.Data_CanAdd_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanImport_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanImport_Role, CommonConstants.Data_CanImport_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanExport_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanExport_Role, CommonConstants.Data_CanExport_Role_Description);
        roleManager.Create(role);
    }
    if (!roleManager.RoleExists(CommonConstants.Data_CanEdit_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanEdit_Role, CommonConstants.Data_CanEdit_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanDisable_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanDisable_Role, CommonConstants.Data_CanDisable_Role_Description);
        roleManager.Create(role);
    }

    if (!roleManager.RoleExists(CommonConstants.Data_CanDelete_Role))
    {
        role = new ApplicationRole(CommonConstants.Data_CanDelete_Role, CommonConstants.Data_CanDelete_Role_Description);
        roleManager.Create(role);
    }

    newGroup = new ApplicationGroup(CommonConstants.DIRECTOR_GROUP, CommonConstants.DIRECTOR_GROUP_NAME);
    groupManager.CreateGroup(newGroup);
    groupManager.SetGroupRoles(newGroup.ID, new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanExport_Role,
            });

    newGroup = new ApplicationGroup(CommonConstants.VERTIFICATIONHEADER_GROUP, CommonConstants.VERTIFICATIONHEADER_GROUP_NAME);
    groupManager.CreateGroup(newGroup);
    groupManager.SetGroupRoles(newGroup.ID, new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanViewStatitics_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanExport_Role,
                CommonConstants.Data_CanEdit_Role,
                CommonConstants.Data_CanDisable_Role
            });

    newGroup = new ApplicationGroup(CommonConstants.VERTIFICATION_GROUP, CommonConstants.VERTIFICATION_GROUP_NAME);
    groupManager.CreateGroup(newGroup);
    groupManager.SetGroupRoles(newGroup.ID, new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
                CommonConstants.Data_CanAdd_Role,
                CommonConstants.Data_CanImport_Role,
                CommonConstants.Data_CanEdit_Role,
            });

    newGroup = new ApplicationGroup(CommonConstants.LAB_GROUP, CommonConstants.LAB_GROUP_NAME);
    groupManager.CreateGroup(newGroup);
    groupManager.SetGroupRoles(newGroup.ID, new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
            });

    newGroup = new ApplicationGroup(CommonConstants.STTT_GROUP, CommonConstants.STTT_GROUP_NAME);
    groupManager.CreateGroup(newGroup);
    groupManager.SetGroupRoles(newGroup.ID, new string[] {
                CommonConstants.Data_CanView_Role,
                CommonConstants.Data_CanViewDetail_Role,
                CommonConstants.Data_CanViewChart_Role,
            });
}

private void CreateRolesandUsers2(BTSDbContext context)
{
    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

    // In Startup iam creating first Admin Role and creating a default Admin User
    if (!roleManager.RoleExists(CommonConstants.System_Admin_Role))
    {
        // first we create Admin rool
        var role = new ApplicationRole();
        role.Name = CommonConstants.System_Admin_Role;
        role.Description = CommonConstants.System_Admin_Role_Description;
        roleManager.Create(role);

        //Here we create a Admin super user who will maintain the website

        var user = new ApplicationUser
        {
            UserName = CommonConstants.SuperAdmin_Name,
            FullName = CommonConstants.SuperAdmin_FullName,
            Email = CommonConstants.SuperAdmin_Email,
            EmailConfirmed = CommonConstants.SuperAdmin_EmailConfirmed,
        };

        if (userManager.Users.Count(x => x.UserName == user.UserName) != 0)
        {
            var chkUser = userManager.Create(user, CommonConstants.SuperAdmin_Password);
            chkUser = userManager.SetLockoutEnabled(user.Id, false);

            //Add default User to Role Admin
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "Admin");
            }
        }
    }

    // creating Creating Manager role
    if (!roleManager.RoleExists("Manager"))
    {
        var role = new IdentityRole();
        role.Name = "Manager";
        roleManager.Create(role);
    }

    // creating Creating Employee role
    if (!roleManager.RoleExists("Employee"))
    {
        var role = new IdentityRole();
        role.Name = "Employee";
        roleManager.Create(role);
    }

    // creating Creating Guest role
    if (!roleManager.RoleExists("Guest"))
    {
        var role = new IdentityRole();
        role.Name = "Guest";
        roleManager.Create(role);
    }
}