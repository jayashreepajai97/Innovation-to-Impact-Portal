using System;
using Responses;
using Responses.Enums;
using IdeaDatabase.Enums;

namespace IdeaDatabase.Responses
{
    /// <summary>
    /// This class should contain all possible faults, that this service can report. Everywhere when response is created
    /// developer should use Fault from this list instead of creating one on their own. 
    /// </summary>
    public class Faults
    {
        public static Fault UnknownError { get; } = new Fault("Unknown", "UnknownError", "Cause and origin of this error is unknown");
        public static Fault AWSS3UploadError { get; } = new Fault("AWSS3Upload", "AWSError", "Something went wrong !");

        public static Fault UserRoleExists { get; } = new Fault("RoleMappings", "RoleMappingsError", "Roles exists for the userID");
        public static Fault RoleIDNotExists { get; } = new Fault("UserRoles", "UserRolesError", "Role does not exists");
        public static Fault ReviewerNotExists { get; } = new Fault("Reviewer", "ReviewerNotExists", "No reviewer found.");
        public static Fault IdeaStateExists { get; } = new Fault("IdeaState", "IdeaStatesError", "Status already exists");
        public static Fault RoleNameExists { get; } = new Fault("RoleName", "RoleNameExists", "Role Name Exists");
        public static Fault IdeaCategoriesNameExists { get; } = new Fault("IdeaCategory", "CategoriesNameExists", "Category Name Exists");
        public static Fault IdeaChallengeNameExists { get; } = new Fault("IdeaChallenge", "ChallengeNameExists", "Challenge Name Exists");



        public static Fault IdeaCategoriesIDNotExists { get; } = new Fault("IdeaCategory", "CategoriesNameNotExists", "Category Name Not Exists");
        public static Fault IdeaNotExists { get; } = new Fault("IdeaDetails", "IdeaNotExists", "Idea Not Exists");
        public static Fault IdeaExistsForCategory { get; } = new Fault("IdeaCategory", "IdeaExistsForCategory", "Cannot delete given category as one or more Ideas exist corresponding to this category");

        
        public static Fault IdeaStateNotExists { get; } = new Fault("IdeaStatusHistoryLog", "IdeaStatesError", "Idea status does not exists");
        public static Fault IdeaCommentNotExists { get; } = new Fault("IdeaComment", "IdeaCommentsError", "No comments found for this Idea ID");


        public static Fault InvalidCredentials { get; } = new Fault("InvalidCredentials", "Given credentials are invalid");
        public static Fault InvalidSerialNumber { get; } = new Fault("InvalidSerialNumber", "SerialNumber field is invalid");
        public static Fault InvalidIdeaStatus { get; } = new Fault("InvalidIdeaStatus", "IdeaStatusError", "Idea status cannot be updated");
        public static Fault InvalidGitRepo { get; } = new Fault("InvalidGitRepo", "InvalidRepoError", "Git Repo cannot be updated");
        public static Fault InvalidTag { get; } = new Fault("InvalidTag", "InvalidTagError", "Tag cannot be updated");


        public static Fault MissingDataInUpdates { get; } = new Fault("MissingDataInUpdates", "Missing data in Updates node");
        public static Fault MissingDataInMessages { get; } = new Fault("MissingDataInMessages", "Missing data in Messages node");
        public static Fault MissingDataInSoftware { get; } = new Fault("MissingDataInSoftware", "Missing data in Software node");
        public static Fault MissingDataInAccessories { get; } = new Fault("MissingDataInAccessories", "Missing data in Accessories node");

        public static Fault AlertNotFound { get; } = new Fault("AlertNotFound", "No alert found for this alert id");
        public static Fault ContractNotFound { get; } = new Fault("ContractNotFound", "Contract not found");
        public static Fault CarePackNotFound { get; } = new Fault("CarePackNotFound", "Care Pack data not found");
        public static Fault IdeaCommentNotFound { get; } = new Fault("IdeaCommentNotFound", "Idea comment not found");
        public static Fault IdeaNotFound { get; } = new Fault("IdeaNotFound", "Idea not found");
        public static Fault IdeaIntellectNotFound { get; } = new Fault("IdeaIntellectNotFound", "Idea Intellectual Property not found");
        public static Fault NoActivityFound { get; } = new Fault("NoActivityFound", "No activity found for this idea");


        public static Fault UserNotFound { get; } = new Fault("UserNotFound", "No matching user found");
    
        public static Fault InvalidSearch { get; } = new Fault("InvalidSearch", "Search field is invalid");

        public static Fault MissingEmailAddress { get; } = new Fault("MissingEmailAddress", "Email address is missing");
        public static Fault MissingSubjectAddress { get; } = new Fault("MissingSubjectAddress", "Email subject is missing");
        public static Fault MissingBodyAddress { get; } = new Fault("MissingBodyAddress", "Email body is missing");
        public static Fault SendEmailError { get; } = new Fault("SendEmailError", "Sending email failed");
        public static Fault InsufficientPermissions { get; } = new Fault("InsufficientPermissions", "You do not have requierd permissions to perform this activity");

        /// <summary>
        /// Fault returned when server is unable to perform an operation
        /// </summary>
        public static Fault ServerIsBusy { get; } = new Fault("ServerIsBusy", "Server currently cannot process the request");

        public static Fault InvalidContentType { get; } = new Fault("InvalidContentType", "Content-Type must be set to application/json");

        /// <summary>
        /// Fault returned when invalid storage, drive or partition is provided
        /// </summary>
            public static Fault MissingRequestContent { get; } = new Fault("MissingRequestContent", "Request content is missing, null or empty");



        public static Fault NotHPProduct { get; } = new Fault("NotHPProduct", "Method not allowed for not-HP product");
     
      
     
        public static Fault LowerAppVersion { get; } = new Fault("AppUpgrade", "Mandatory app upgrade required");
        public static Fault HigherAppVersion { get; } = new Fault("AppDowngrade", "Mandatory app downgrade required");

        /// <summary>
        /// Fault returned when error, while encoding failure info.
        /// </summary>
          public static Fault EmptyFailureCode { get; } = new Fault("FailureIdEncode", "EmptyFailureCode", "FailureCode is empty or null.");
        public static Fault FailedToGenerateId { get; } = new Fault("FailureIdEncode", "FailedToGenerateId", "Failed to generate failure id.");
        public static Fault InvalidTestDate { get; } = new Fault("FailureIdEncode", "InvalidTestDate", "TestDate not valid or not in valid format(yyyy/MM/dd).");
        public static Fault InvalidStartDate { get; } = new Fault("FailureIdEncode", "InvalidStartDate", "StartDate not valid or not in valid format(yyyy/MM/dd).");

        /// <summary>
        /// Errors from EAS service
        /// </summary>
        public static Fault EASServiceError { get; } = new Fault("EAS", "RemoteServiceError", "Cannot connect to remote service");
        public static Fault EASConnectionError { get; } = new Fault("EAS", "RemoteServiceError", "Connection to remote service failed");
        public static Fault EASTimeoutError { get; } = new Fault("EAS", "RemoteServiceError", "The service operation timed out");
        public static Fault LocaleMappingNotfound { get; } = new Fault("LocaleMappingNotFound", "No Locale Mapping found in database");
     
        public static Fault IdeaAttachmentNotFound { get; } = new Fault("IdeaAttachmentNotFound", "Idea Attachment not found");

    }
}