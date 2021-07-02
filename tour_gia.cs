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

    public partial class tour_gia
    {
        public int gia_id { get; set; }

        [Required]
        [Display(Name = "Giá tiền")]
        public double gia_sotien { get; set; }
        [Required]
        [Display(Name = "Từ ngày"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime gia_tungay { get; set; }
        [Required]
        [Display(Name = "Đến ngày"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime gia_denngay { get; set; }

        [Required]
        [Display(Name = "Tour")]
        public int tour_id { get; set; }
    }
}
