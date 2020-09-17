using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace IdeaDatabase.DataContext
{
    public interface IIdeaDatabaseDataContext : IDisposable
    {
        DbSet<IdeaAttachment> IdeaAttachments { get; set; }
        DbSet<IdeaCategory> IdeaCategories { get; set; }
        DbSet<IdeaComment> IdeaComments { get; set; }
        DbSet<Idea> Ideas { get; set; }         
        DbSet<RoleMapping> RoleMappings { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<S3AttachmentContainer> S3AttachmentContainer { get; set; }
        DbSet<UserAuthentication> UserAuthentications { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<AdmSetting> AdmSettings { get; set; }
        DbSet<IdeaSubscriber> IdeaSubscribers { get; set; }
        DbSet<Ideatag> Ideatags { get; set; }
        DbSet<IdeaStatusLog> IdeaStatusLogs { get; set; }
        DbSet<IdeaAssignment> IdeaAssignments { get; set; }
        DbSet<IdeaCommentDiscussion> IdeaCommentDiscussions { get; set; }
        DbSet<IdeaChallenge> IdeaChallenges { get; set; }
        DbSet<EmailTemplate> EmailTemplates { get; set; }
       
        DbSet<IdeaContributor> IdeaContributors { get; set; }
        DbSet<IdeaIntellectualProperty> IdeaIntellectualProperties { get; set; }
        DbSet<IdeaLog> IdeaLogs { get; set; }
        DbSet<IdeaArchiveHistory> IdeaArchiveHistories { get; set; }

        int ValidateHPPToken(Nullable<int> customerId, string token, string callerId);
        int SetHPPToken(Nullable<int> customerId, string token, string callerId);
        
        void SubmitChanges();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}