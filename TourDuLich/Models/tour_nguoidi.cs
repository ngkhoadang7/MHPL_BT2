//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourDuLich.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tour_nguoidi
    {
        public int nguoidi_id { get; set; }
        [Required]
        [Display(Name = "Đoàn")]
        public int doan_id { get; set; }

        [Display(Name = "Danh sách nhân viên")]
        public string nguoidi_dsnhanvien { get; set; }

        [Display(Name = "Danh sách khách hàng")]
        public string nguoidi_dskhach { get; set; }
    }
}
