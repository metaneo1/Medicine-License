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
    
    public partial class LocalGoverment2
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocalGoverment2()
        {
            this.LocalGoverment21 = new HashSet<LocalGoverment2>();
        }
    
        public int Id { get; set; }
        public Nullable<int> Id_Parent { get; set; }
        public string Name { get; set; }
        public string SOATE { get; set; }
        public Nullable<int> Id_SettlementType { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
    
        public virtual SettlementType SettlementType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalGoverment2> LocalGoverment21 { get; set; }
        public virtual LocalGoverment2 LocalGoverment22 { get; set; }
    }
}
