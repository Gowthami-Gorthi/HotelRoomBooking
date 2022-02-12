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

    public partial class HMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HMap()
        {
            this.Bookings = new HashSet<Booking>();
            this.BookingHistories = new HashSet<BookingHistory>();
        }
    
        public int Rid { get; set; }
        [Required]
        public string HotelName { get; set; }
        [Required]
        public string RoomType { get; set; }
        [Required]
        public string images { get; set; }
        [Required][RegularExpression("^[0-9]+$",ErrorMessage ="Accept only digits[0-9]")]
        public Nullable<int> price { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual Room Room { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingHistory> BookingHistories { get; set; }
    }
}
