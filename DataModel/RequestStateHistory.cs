//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestStateHistory
    {
        public int Id { get; set; }
        public int Id_Request { get; set; }
        public int Id_State { get; set; }
        public Nullable<System.DateTime> DateStatusChange { get; set; }
        public Nullable<int> Id_User { get; set; }
    
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual LicenseRequest LicenseRequest { get; set; }
        public virtual RequestState RequestState { get; set; }
    }
}
