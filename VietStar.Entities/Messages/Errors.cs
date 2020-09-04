using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Messages
{
    public static class Errors
    {
        public const string customername_must_not_be_empty = "Tên khách hàng không được để trống";
        public const string email_cannot_be_null = "Email không được để trống";
        public const string error_login_expected = "Vui lòng đăng nhập";
        public const string freetext_length_max_lenght = "Chuỗi tìm kiếm không được nhiều hơn 30 ký tự";
        public const string invalid_data = "Dữ liệu không hợp lệ";
        public const string invalid_email = "Email không hợp lệ";
        public const string invalid_id = "Id không hợp lệ";
        public const string invalid_username_or_pass = "Tên đăng nhập hoặc mật khẩu không đúng";
        public const string mising_checkdate = "Vui lòng chọn ngày";
        public const string missing_birthday = "Vui lòng chọn ngày sinh";
        public const string missing_cmnd = "Vui lòng nhập cmnd";
        public const string missing_cmnd_day = "Vui lòng chọn ngày cấp cmnd";
        public const string missing_date_or_invalid = "Bạn chưa chọn ngày tháng hoặc ngày tháng không đúng định dạng(ngày/tháng/năm, vd: 30/12/2019)";
        public const string missing_address = "Vui lòng nhập địa chỉ";
        public const string missing_district = "Vui lòng chọn quận huyện";
        public const string missing_location_code = "Vui lòng chọn mã huyện";
        public const string missing_tema_manager = "Vui lòng chọn người quản lý";
        public const string missing_money = "Vui lòng chọn số tiền vay";
        public const string missing_must_have_files = "Vui lòng đính kèm các tập tin";
        public const string missing_fullname = "Vui lòng nhập họ tên";
        public const string missing_ngaynhandon = "Vui lòng nhập ngày nhận đơn";
        public const string missing_partner = "Vui lòng chọn đối tác";
        public const string missing_product = "Vui lòng chọn sản phẩm vay";
        public const string missing_province = "Vui lòng chọn tỉnh/thành phố";
        public const string missing_phone = "Vui lòng nhập số điện thoại";
        public const string missing_rootpath = "Không tìm thấy rootpath";
        public const string missing_shortname = "Vui lòng nhập tên ngắn";
        public const string missing_team = "Vui lỏng chọn nhóm";
        public const string missing_team_member = "Vui lòng chọn ít nhất 1 thành viên";
        public const string missing_team_name = "Vui lòng nhập tên";
        public const string missing_team_parent = "Vui lòng chọn nhóm cha";
        public const string missing_user_id = "Vui lòng chọn nhân viên";
        public const string not_found_user = "Không tìm thấy người dùng";
        public const string note_length_cannot_more_than_200 = "Nội dung ghi chú không được nhiều hơn 200 ký tự";
        public const string notfound = "Dữ liệu không tồn tại";
        public const string password_not_match = "Mật khẩu không khớp";
        public const string password_not_match_min_length = "Độ dài mật khẩu phải có ít nhất 6 ký tự";
        public const string product_code_inuse = "Mã sản phẩm đang được sử dung bởi hồ sơ khác, vui lòng chọn sản phẩm khác";
        public const string username_has_exist = "Tên đăng nhập đã tồn tại";
        public const string username_or_password_must_not_be_empty = "Tên đăng nhập hoặc mật khẩu không được để trống";
        public const string missing_diachi = "Vui lòng nhập địa chỉ";
        public const string profile_could_not_found_in_portal = "Không thể tìm thấy hồ sơ trong portal";
        public const string NoData = "Không có dữ liệu";
        public const string id_number_has_exist = "Số Cmnd/cccd này đã được tạo ở một hồ sơ khác";
        public const string file_cannot_be_null = "Vui lòng import file theo mẫu";
    }
}
