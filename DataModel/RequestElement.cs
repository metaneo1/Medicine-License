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
    
    public partial class RequestElement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestElement()
        {
            this.DocumentInRequest = new HashSet<DocumentInRequest>();
            this.RequestElement1 = new HashSet<RequestElement>();
        }
    
        public int Id { get; set; }
        public Nullable<int> Id_Parent { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public int Id_RequestElemType { get; set; }
        public Nullable<int> Id_TemplateDocument { get; set; }
        public int Id_RequestType { get; set; }
        public int Id_StructureType { get; set; }
        public Nullable<bool> IsRequired { get; set; }
    
        public virtual DocTemplForReqElemType DocTemplForReqElemType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentInRequest> DocumentInRequest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestElement> RequestElement1 { get; set; }
        public virtual RequestElement RequestElement2 { get; set; }
        public virtual RequestElementType RequestElementType { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual RequestElemStructureType RequestElemStructureType { get; set; }
    }
}
