//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdeaDatabase.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class IdeaLog
    {
        public int IdeaLogsId { get; set; }
        public Nullable<int> IdeaId { get; set; }
        public string LogMessage { get; set; }
        public Nullable<int> Type { get; set; }
        public string OriginMethod { get; set; }
        public string OriginAPI { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Idea Idea { get; set; }
        public virtual User User { get; set; }
    }
}
