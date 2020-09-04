using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Profile
{
    public class ProfileAddSql : SqlBaseModel
    {
        public int ID { get; set; }

        public string Ma_Ho_So { get; set; }

        public string Ten_Khach_Hang { get; set; }

        public string CMND { get; set; }

        public string Dia_Chi { get; set; }

        public int Ma_Khu_Vuc { get; set; }

        public string SDT { get; set; }

        public string SDT2 { get; set; }

        public int Gioi_Tinh { get; set; }
        public int Ho_So_Cua_Ai { get; set; }

        public DateTime? Ngay_Nhan_Don { get; set; }

        public int Ma_Trang_Thai { get; set; }

        public int San_Pham_Vay { get; set; }

       
        public bool Co_Bao_Hiem { get; set; }

        public decimal So_Tien_Vay { get; set; }

        public string Han_Vay { get; set; }

        public string Ghi_Chu { get; set; }

        public int Courier_Code { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? BirthDay { get; set; }

        public DateTime? CMNDDay { get; set; }
    }
}
