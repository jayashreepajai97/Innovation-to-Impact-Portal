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
    
    public partial class Idea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Idea()
        {
            this.IdeaAssignments = new HashSet<IdeaAssignment>();
            this.IdeaAttachments = new HashSet<IdeaAttachment>();
            this.IdeaComments = new HashSet<IdeaComment>();
            this.IdeaContributors = new HashSet<IdeaContributor>();
            this.IdeaStatusLogs = new HashSet<IdeaStatusLog>();
            this.IdeaSubscribers = new HashSet<IdeaSubscriber>();
            this.Ideatags = new HashSet<Ideatag>();
            this.S3AttachmentContainer = new HashSet<S3AttachmentContainer>();
            this.IdeaIntellectualProperties = new HashSet<IdeaIntellectualProperty>();
            this.IdeaArchiveHistories = new HashSet<IdeaArchiveHistory>();
            this.IdeaLogs = new HashSet<IdeaLog>();
        }
    
        public int IdeaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAttachment { get; set; }
        public Nullable<bool> IsSensitive { get; set; }
        public Nullable<int> AttachmentCount { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string BusinessImpact { get; set; }
        public string Solution { get; set; }
        public Nullable<int> ChallengeId { get; set; }
        public string GitRepo { get; set; }
        public Nullable<bool> IsDraft { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaAssignment> IdeaAssignments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaAttachment> IdeaAttachments { get; set; }
        public virtual IdeaCategory IdeaCategory { get; set; }
        public virtual IdeaChallenge IdeaChallenge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaComment> IdeaComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaContributor> IdeaContributors { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaStatusLog> IdeaStatusLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaSubscriber> IdeaSubscribers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ideatag> Ideatags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<S3AttachmentContainer> S3AttachmentContainer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaIntellectualProperty> IdeaIntellectualProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaArchiveHistory> IdeaArchiveHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdeaLog> IdeaLogs { get; set; }
    }
}