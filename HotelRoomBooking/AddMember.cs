//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelRoomBooking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class AddMember
    {
        public int MemId { get; set; }
        [Required]
        public string Member_Name { get; set; }
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Age shold be in digits only")]
        public Nullable<int> member_Age { get; set; }
    }
}
