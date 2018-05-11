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
        public const string VERTIFICATION_GROUP_NAME = "Chuyên viên kiểm định";
        public const string LAB_GROUP_NAME = "Chuyên viên đo kiểm";
        public const string STTT_GROUP_NAME = "Sở TTTT";

        public const string SuperAdmin_Name = "admin";
        public const string SuperAdmin_FullName = "Trần Công Khanh";
        public const string SuperAdmin_Email = "tckhanh.p@gmail.com";
        public const bool SuperAdmin_EmailConfirmed = true;
        public const string SuperAdmin_Password = "P@ssword";

        public const string System_Admin_Role = "System_Admin";
        public const string Data_CanView_Role = "Data_CanView";
        public const string Data_CanViewDetail_Role = "Data_CanViewDetail";
        public const string Data_CanViewChart_Role = "Data_CanViewChart";
        public const string Data_CanViewStatitics_Role = "Data_CanViewStatitics";
        public const string Data_CanAdd_Role = "Data_CanAdd";
        public const string Data_CanImport_Role = "Data_CanImport";
        public const string Data_CanExport_Role = "Data_CanExport";
        public const string Data_CanEdit_Role = "Data_CanEdit";
        public const string Data_CanDisable_Role = "Data_CanDisable";
        public const string Data_CanDelete_Role = "Data_CanDelete";

        public const string System_Admin_Role_Description = "Quản trị hệ thống";
        public const string Data_CanView_Role_Description = "Xem dữ liệu";
        public const string Data_CanViewDetail_Role_Description = "Xem chi tiết dữ liệu";
        public const string Data_CanViewChart_Role_Description = "Xem biểu đồ";
        public const string Data_CanViewStatitics_Role_Description = "Xem dữ liệu thống kê";
        public const string Data_CanAdd_Role_Description = "Thêm dữ liệu";
        public const string Data_CanImport_Role_Description = "Nhập dữ liệu";
        public const string Data_CanExport_Role_Description = "Xuất dữ liệu";
        public const string Data_CanEdit_Role_Description = "Sửa dữ liệu";
        public const string Data_CanDisable_Role_Description = "Xóa tạm dữ liệu";
        public const string Data_CanDelete_Role_Description = "Xóa bỏ dữ liệu";

        public static string USER_SESSION = "USER_SESSION";
        public static string SESSION_CREDENTIALS = "SESSION_CREDENTIALS";
        public static string CartSession = "CartSession";
        public static string CurrentCulture { set; get; }

        public const string Status_Error = "Error";
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

        public const string Sheet_Bts = "Bts";
        public const string Sheet_Bts_OperatorID = "OperatorID";
        public const string Sheet_Bts_BtsCode = "BtsCode";
        public const string Sheet_Bts_Address = "Address";
        public const string Sheet_Bts_CityID = "CityID";
        public const string Sheet_Bts_Longtitude = "Longtitude";
        public const string Sheet_Bts_Latitude = "Latitude";
        public const string Sheet_Bts_InCaseOfID = "InCaseOfID";
        public const string Sheet_Bts_IsCertificated = "IsCertificated";
        public const string Sheet_Bts_LastCertificateNo = "LastCertificateNo";

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
        public const string Sheet_Certificate_SubBtsConfigurations = "SubBtsConfigurations";
        public const string Sheet_Certificate_SubBtsPowerSums = "SubBtsPowerSums";
        public const string Sheet_Certificate_SubBtsBands = "SubBtsBands";
        public const string Sheet_Certificate_SubBtsAntenHeights = "SubBtsAntenHeights";
        public const string Sheet_Certificate_MinAntenHeight = "MinAntenHeight";
        public const string Sheet_Certificate_MaxHeightIn100m = "MaxHeightIn100m";
        public const string Sheet_Certificate_OffsetHeight = "OffsetHeight";
        public const string Sheet_Certificate_SafeLimit = "SafeLimit";

        public const string IssuePalce = "Tp. Hồ Chí Minh";
        public const string Signer = "Trần Công Khanh";

        public const string ImportCER = "ImportCER";
    }
}