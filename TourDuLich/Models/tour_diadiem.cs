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


    public partial class tour_diadiem
    {
        public int dd_id { get; set; }

        [Required]
        [Display(Name = "Thành phố")]
        public string dd_thanhpho { get; set; }

        [Required]
        [Display(Name = "Tên địa điểm")]
        public string dd_ten { get; set; }

        [Required]
        [Display(Name = "Mô tả")]
        public string dd_mota { get; set; }
    }
}