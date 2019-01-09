//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPassBook.DAL.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserMaster()
        {
            this.GeoTaggingDetails = new HashSet<GeoTaggingDetail>();
            this.InstallmentSignings = new HashSet<InstallmentSigning>();
            this.UserInRoles = new HashSet<UserInRole>();
        }
    
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> Dob { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsLoggedIn { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<bool> IsReset { get; set; }
    
        public virtual CityMaster CityMaster { get; set; }
        public virtual CompanyMaster CompanyMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GeoTaggingDetail> GeoTaggingDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstallmentSigning> InstallmentSignings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}
