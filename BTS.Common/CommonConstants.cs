namespace BTS.Common
{
    public class CommonConstants
    {
        public const string SUPERADMIN_GROUP = "SUPERADMIN";
        public const string DIRECTOR_GROUP = "DIRECTOR";
        public const string VERTIFICATIONHEADER_GROUP = "HEADER";
        public const string VERTIFICATION_GROUP = "VERTIFICATION";
        public const string LAB_GROUP = "LAB";
        public const string STTT_GROUP = "STTT";

        public const string SUPERADMIN_GROUP_NAME = "Quản trị hệ thống";
        public const string DIRECTOR_GROUP_NAME = "Lãnh đạo Trung tâm";
        public const string VERTIFICATIONHEADER_GROUP_NAME = "Quản lý Kiểm định";
        public const string VERTIFICATION_GROUP_NAME = "Phòng Kiểm định";
        public const string LAB_GROUP_NAME = "Phòng Đo kiểm";
        public const string STTT_GROUP_NAME = "Sở TTTT";

        public const string Action_Add = "Add";
        public const string Action_Detail = "Detail";
        public const string Action_Edit = "Edit";
        public const string Action_Reset = "Reset";
        public const string Action_ChangePassword = "ChangePassword";
        public const string Action_Delete = "Delete";

        public const string SuperAdmin_Name = "admin";
        public const string SuperAdmin_FullName = "Trần Công Khanh";
        public const string SuperAdmin_Email = "tckhanh.p@gmail.com";
        public const bool SuperAdmin_EmailConfirmed = true;
        public const string SuperAdmin_Password = "P@ssword";

        public const string System_CanView_Role = "System_CanView";
        public const string System_CanViewDetail_Role = "System_CanViewDetail";
        public const string System_CanViewChart_Role = "System_CanViewChart";
        public const string System_CanViewStatitics_Role = "System_CanViewStatitics";
        public const string System_CanAdd_Role = "System_CanAdd";
        public const string System_CanImport_Role = "System_CanImport";
        public const string System_CanExport_Role = "System_CanExport";
        public const string System_CanEdit_Role = "System_CanEdit";
        public const string System_CanReset_Role = "System_CanReset";
        public const string System_CanLock_Role = "System_CanLock";
        public const string System_CanDelete_Role = "System_CanDelete";

        public const string Data_CanView_Role = "Data_CanView";
        public const string Data_CanViewDetail_Role = "Data_CanViewDetail";
        public const string Data_CanViewChart_Role = "Data_CanViewChart";
        public const string Data_CanViewStatitics_Role = "Data_CanViewStatitics";
        public const string Data_CanAdd_Role = "Data_CanAdd";
        public const string Data_CanImport_Role = "Data_CanImport";
        public const string Data_CanExport_Role = "Data_CanExport";
        public const string Data_CanEdit_Role = "Data_CanEdit";
        public const string Data_CanReset_Role = "Data_CanReset";
        public const string Data_CanLock_Role = "Data_CanLock";
        public const string Data_CanDelete_Role = "Data_CanDelete";

        public const string System_CanView_Description = "Xem hệ thống";
        public const string System_CanViewDetail_Description = "Xem chi tiết hệ thống";
        public const string System_CanViewChart_Description = "Xem biểu đồ hệ thống";
        public const string System_CanViewStatitics_Description = "Xem thống kê hệ thống";
        public const string System_CanAdd_Description = "Thêm hệ thống";
        public const string System_CanImport_Description = "Nhập hệ thống";
        public const string System_CanExport_Description = "Xuất hệ thống";
        public const string System_CanEdit_Description = "Sửa hệ thống";
        public const string System_CanReset_Description = "Tạo lại mật khẩu";
        public const string System_CanLock_Description = "Khóa người dùng";
        public const string System_CanDelete_Description = "Xóa bỏ hệ thống";

        public const string Data_CanView_Description = "Xem dữ liệu";
        public const string Data_CanViewDetail_Description = "Xem chi tiết dữ liệu";
        public const string Data_CanViewChart_Description = "Xem biểu đồ dữ liệu";
        public const string Data_CanViewStatitics_Description = "Xem thống kê dữ liệu";
        public const string Data_CanAdd_Description = "Thêm dữ liệu";
        public const string Data_CanImport_Description = "Nhập dữ liệu";
        public const string Data_CanExport_Description = "Xuất dữ liệu";
        public const string Data_CanEdit_Description = "Sửa dữ liệu";
        public const string Data_CanReset_Description = "Thay đổi mật khẩu";
        public const string Data_CanLock_Description = "Khóa dữ liệu";
        public const string Data_CanDelete_Description = "Xóa bỏ dữ liệu";

        public static string USER_SESSION = "USER_SESSION";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CartSession = "CartSession";
        public static string CurrentCulture { set; get; }

        public const string Status_Error = "Error";
        public const string Status_TimeOut = "TimeOut";
        public const string Status_Success = "Success";

        public const string ProductTag = "product";
        public const string PostTag = "post";
        public const string DefaultFooterId = "default";

        public const string SessionCart = "SessionCart";

        public const string HomeTitle = "HomeTitle";
        public const string HomeMetaKeyword = "HomeMetaKeyword";
        public const string HomeMetaDescription = "HomeMetaDescription";

        public const string Sheet_InCaseOf = "InCaseOf";
        public const string Sheet_InCaseOf_ID = "ID";
        public const string Sheet_InCaseOf_Name = "Name";

        public const string Sheet_Lab = "Lab";
        public const string Sheet_Lab_ID = "ID";
        public const string Sheet_Lab_Name = "Name";
        public const string Sheet_Lab_Address = "Address";
        public const string Sheet_Lab_Phone = "Phone";
        public const string Sheet_Lab_Fax = "Fax";

        public const string Sheet_City = "City";
        public const string Sheet_City_ID = "ID";
        public const string Sheet_City_Name = "Name";

        public const string Sheet_Operator = "Operator";
        public const string Sheet_Operator_ID = "ID";
        public const string Sheet_Operator_Name = "Name";

        public const string Sheet_Applicant = "Applicant";
        public const string Sheet_Applicant_ID = "ID";
        public const string Sheet_Applicant_Name = "Name";
        public const string Sheet_Applicant_Address = "Address";
        public const string Sheet_Applicant_Phone = "Phone";
        public const string Sheet_Applicant_Fax = "Fax";
        public const string Sheet_Applicant_ContactName = "ContactName";
        public const string Sheet_Applicant_OperatorID = "OperatorID";

        public const string Sheet_Profile = "Profile";
        public const string Sheet_Profile_ApplicantID = "ApplicantID";
        public const string Sheet_Profile_ProfileNum = "ProfileNum";
        public const string Sheet_Profile_ProfileDate = "ProfileDate";
        public const string Sheet_Profile_BtsQuantity = "BtsQuantity";
        public const string Sheet_Profile_ApplyDate = "ApplyDate";
        public const string Sheet_Profile_ValidDate = "ValidDate";
        public const string Sheet_Profile_Fee = "Fee";
        public const string Sheet_Profile_FeeAnnounceNum = "FeeAnnounceNum";
        public const string Sheet_Profile_FeeAnnounceDate = "FeeAnnounceDate";
        public const string Sheet_Profile_FeeReceiptDate = "FeeReceiptDate";

        public const string Sheet_Bts = "BTS";
        public const string Sheet_Bts_OperatorID = "OperatorID";
        public const string Sheet_Bts_BtsCode = "BtsCode";
        public const string Sheet_Bts_Address = "Address";
        public const string Sheet_Bts_CityID = "CityID";
        public const string Sheet_Bts_Longtitude = "Longtitude";
        public const string Sheet_Bts_Latitude = "Latitude";
        public const string Sheet_Bts_InCaseOfID = "InCaseOfID";
        public const string Sheet_Bts_IsCertificated = "IsCertificated";
        public const string Sheet_Bts_LastOwnCertificateIDs = "LastOwnCertificateIDs";
        public const string Sheet_Bts_LastNoOwnCertificateIDs = "LastNoOwnCertificateIDs";
        public const string Sheet_Bts_ProfileInProcess = "ProFileInProcess";
        public const string Sheet_Bts_ReasonNoCertificate = "ReasonNoCertificate";

        public const string Sheet_Certificate = "Certificate";
        public const string Sheet_Certificate_BtsCode = "BtsCode";
        public const string Sheet_Certificate_Address = "Address";
        public const string Sheet_Certificate_CityID = "CityID";
        public const string Sheet_Certificate_Longtitude = "Longtitude";
        public const string Sheet_Certificate_Latitude = "Latitude";
        public const string Sheet_Certificate_InCaseOfID = "InCaseOfID";
        public const string Sheet_Certificate_LabID = "LabID";
        public const string Sheet_Certificate_TestReportNo = "TestReportNo";
        public const string Sheet_Certificate_TestReportDate = "TestReportDate";
        public const string Sheet_Certificate_CertificateNum = "CertificateNum";
        public const string Sheet_Certificate_IssuedDate = "IssuedDate";
        public const string Sheet_Certificate_ExpiredDate = "ExpiredDate";
        public const string Sheet_Certificate_SubBtsQuantity = "SubBtsQuantity";
        public const string Sheet_Certificate_SubBtsCodes = "SubBtsCodes";
        public const string Sheet_Certificate_SubBtsOperatorIDs = "SubBtsOperatorIDs";
        public const string Sheet_Certificate_SubBtsEquipments = "SubBtsEquipments";
        public const string Sheet_Certificate_SubBtsAntenNums = "SubBtsAntenNums";
        public const string Sheet_Certificate_SharedAntens = "SharedAntens";
        public const string Sheet_Certificate_SubBtsConfigurations = "SubBtsConfigurations";
        public const string Sheet_Certificate_SubBtsPowerSums = "SubBtsPowerSums";
        public const string Sheet_Certificate_SubBtsBands = "SubBtsBands";
        public const string Sheet_Certificate_SubBtsAntenHeights = "SubBtsAntenHeights";
        public const string Sheet_Certificate_MinAntenHeight = "MinAntenHeight";
        public const string Sheet_Certificate_MaxHeightIn100m = "MaxHeightIn100m";
        public const string Sheet_Certificate_OffsetHeight = "OffsetHeight";
        public const string Sheet_Certificate_MeasuringExposure = "MeasuringExposure";
        public const string Sheet_Certificate_SafeLimit = "SafeLimit";

        public const string Sheet_NoCertificate = "NoCertificate";
        public const string Sheet_NoCertificate_BtsCode = "BtsCode";
        public const string Sheet_NoCertificate_Address = "Address";
        public const string Sheet_NoCertificate_CityID = "CityID";
        public const string Sheet_NoCertificate_Longtitude = "Longtitude";
        public const string Sheet_NoCertificate_Latitude = "Latitude";
        public const string Sheet_NoCertificate_InCaseOfID = "InCaseOfID";
        public const string Sheet_NoCertificate_LabID = "LabID";
        public const string Sheet_NoCertificate_TestReportNo = "TestReportNo";
        public const string Sheet_NoCertificate_TestReportDate = "TestReportDate";
        public const string Sheet_NoCertificate_Reason = "Reason";

        public const string Sheet_User = "User";
        public const string Sheet_User_FullName = "FullName";
        public const string Sheet_User_BirthDay = "BirthDay";
        public const string Sheet_User_FatherLand = "FatherLand";
        public const string Sheet_User_Level = "Level";
        public const string Sheet_User_EducationalField = "EducationalField";
        public const string Sheet_User_EntryDate = "EntryDate";
        public const string Sheet_User_OfficialDate = "OfficialDate";
        public const string Sheet_User_EndDate = "EndDate";
        public const string Sheet_User_WorkingDuration = "WorkingDuration";
        public const string Sheet_User_JobPositions = "JobPositions";
        public const string Sheet_User_Department = "Department";
        public const string Sheet_User_Telephone = "Telephone";
        public const string Sheet_User_Image = "Image";
        public const string Sheet_User_UserName = "UserName";
        public const string Sheet_User_Password = "Password";
        public const string Sheet_User_Email = "Email";

        public const string IssuePalce = "Tp. Hồ Chí Minh";
        public const string Signer = "Trần Công Khanh";

        public const string ImportCER = "ImportCER";
        public const string ImportBTS = "ImportBTS";
        public const string SelectAll = "ALL";
    }
}