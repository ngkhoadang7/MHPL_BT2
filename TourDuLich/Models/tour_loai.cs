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

    public partial class tour_loai
    {
        
        public int loai_id { get; set; }

        [Required]
        [Display(Name = "Tên loại Tour")]
        public string loai_ten { get; set; }

        [Required]
        [Display(Name = "Mô tả loại Tour")]
        public string loai_mota { get; set; }
    }
}
