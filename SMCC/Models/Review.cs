//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMCC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Review()
        {
            this.Articles = new HashSet<Article>();
        }
    
        public int ReviewId { get; set; }
        public string Text { get; set; }
        public string ReviewUserId { get; set; }
        public bool Accepted { get; set; }
        public System.DateTime DateReviewed { get; set; }
        public Nullable<System.DateTime> DateEdited { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Article> Articles { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
