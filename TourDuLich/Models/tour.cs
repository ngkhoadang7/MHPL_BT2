﻿//------------------------------------------------------------------------------
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


    public partial class tour
    {
        public int tour_id { get; set; }

        [Required]
        [Display(Name = "Tên tour")]
        public string tour_ten { get; set; }

        [Required]
        [Display(Name = "Mô tả tour")]
        public string tour_mota { get; set; }

        [Required]
        [Display(Name = "Loại tour")]
        public int loai_id { get; set; }

        public string tour_chitiet { get; set; }
    }
}