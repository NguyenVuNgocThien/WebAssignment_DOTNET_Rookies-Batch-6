﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SharedModel.Models
{
    public partial class TaiKhoanDangNhap
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string MaKh { get; set; }
        public int? MaNv { get; set; }
        public int? MaQuyen { get; set; }

        public virtual KhachHang MaKhNavigation { get; set; }
    }
}