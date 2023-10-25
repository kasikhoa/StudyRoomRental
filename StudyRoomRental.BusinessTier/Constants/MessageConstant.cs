using System.Data;
using System.Net.NetworkInformation;

namespace StudyRoomRental.BusinessTier.Constants;

public static class MessageConstant
{
    public static class LoginMessage
    {
        public const string InvalidUsernameOrPassword = "Tên đăng nhập hoặc mật khẩu không chính xác";
        public const string DeactivatedAccount = "Tài khoản đang bị vô hiệu hoá";
    }

    public static class Account
    {
        public const string AccountExisted = "Tài khoản đã tồn tại";
        public const string CreateAccountFailed = "Tạo tài khoản thất bại";
        public const string RenterRoleMessage = "Bạn không có quyền tạo phòng";

        public const string UpdateAccountStatusRequestWrongFormatMessage = "Cập nhật status tài khoản request sai format";

        public const string AccountNotFoundMessage = "Không tìm thấy tài khoản";
        public const string UpdateAccountSuccessfulMessage = "Cập nhật status tài khoản thành công";
        public const string UpdateAccountFailedMessage = "Cập nhật thông tin tài khoản thất bại";
        public const string UpdateAccountStatusFailedMessage = "Vô hiệu hóa tài khoản thất bại";
        public const string UpdateAccountStatusSuccessfulMessage = "Vô hiệu hóa tài khoản thành công";
        public const string EmptyAccountIdMessage = "Account Id không hợp lệ";
     
        public const string StaffNotFoundMessage = "Không tìm thấy nhân viên";
        public const string UpdateAccountRoleFailedMessage = "Cập nhật vai trò người cho thuê thất bại";
        public const string UpdateAccountRoleSuccessfulMessage = "Cập nhật vai trò người cho thuê thành công";
    }

    public static class RoomType
    {
        public const string DuplicatedNameMessage = "Loại phòng đã tồn tại";
        public const string CreateRoomTypeFailedMessage = "Tạo mới kiểu phòng thất bại";
        public const string EmptyIdMessage = "Id không hợp lệ";
        public const string NotFoundMessage = "Room Type không có trong hệ thống";
        public const string UpdateFailedMessage = "Cập nhật Room Type thất bại";
    }

    public static class Room
    {
        public const string EmptyIdMessage = "Id không hợp lệ";
        public const string CreateRoomFailedMessage = "Tạo mới phòng thất bại";
        public const string NotFoundMessage = "Room không có trong hệ thống";
        public const string UpdateFailedMessage = "Cập nhật Room thất bại";
    }

    public static class RoomSchedule
    {
        public const string CreateFailedMessage = "Tạo lịch thuê phòng thất bại";
    }


}