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
    
    public partial class Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document()
        {
            this.DocTemplForReqElemType = new HashSet<DocTemplForReqElemType>();
            this.DocumentInRequest = new HashSet<DocumentInRequest>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string PathToFile { get; set; }
        public int Id_DocumentFormat { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocTemplForReqElemType> DocTemplForReqElemType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentInRequest> DocumentInRequest { get; set; }
        public virtual Document_Format Document_Format { get; set; }
    }
}
