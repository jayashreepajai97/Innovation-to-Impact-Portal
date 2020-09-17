using Responses;
using Responses.Enums;
using System.Collections.Generic;

namespace InnovationPortalService.Responses
{
    /// <summary>
    /// This class should contain all possible faults, that this service can report. Everywhere when response is created
    /// developer should use Fault from this list instead of creating one on their own. 
    /// </summary>
    public class Faults
    {
        /// <summary>
        /// Fault returned when some error message, rule, or other fail sent by other service was not mapped to 
        /// unique fault in our service - should be avoided
        /// </summary>
        public static Fault UnMappedError { get; } = new Fault("Unknown", "UnMappedError", "Cause and origin of this error is unknown");
        public static Fault TokenExpired { get; } = new Fault("ISAAC", "TokenExpired", "Provided token is no longer valid");

        public static Fault ISAACConnectionError { get; } = new Fault("ISAAC", "RemoteServiceError", "Connection to remote service failed");

        public static Fault CDAXConnectionError { get; } = new Fault("CDAX", "RemoteServiceError", "Connection to remote service failed");
        public static Fault CDAXTimeoutError { get; } = new Fault("CDAX", "RemoteServiceError", "The service operation timed out");
        public static Fault CDAXServiceError { get; } = new Fault("CDAX", "RemoteServiceError", "Error encountered during case creation");

        public static Fault CDAXExtInternalError { get; } = new Fault("CDAXExt", "RemoteServiceError", "Invalid response from service");
        public static Fault CDAXExtTransactionError { get; } = new Fault("CDAXExt", "RemoteServiceError", "Error response from service");

        public static Fault OBSServiceError { get; } = new Fault("OBS", "RemoteServiceError", "Error encountered during interaction creation");
        public static Fault OBSTimeoutError { get; } = new Fault("OBS", "RemoteServiceError", "The service operation timed out");
        public static Fault OBSMissingCountryCode { get; } = new Fault("OBS", "RemoteServiceError", "Missing Country attribute in OBS node (cannot select OBS region)");
        public static Fault MissingOBSProxyURL { get; } = new Fault("InnovationPortal", "MissingOBSProxyURL", "Missing OBS proxy URL");

        public static Fault HPPInternalError { get; } = new Fault("HPP", "RemoteServiceError", "Remote service internal error");

        public static Fault HPPTimeoutError { get; } = new Fault("HPP", "RemoteServiceError", "The service operation timed out");
        public static Fault ISAACInternalError { get; } = new Fault("ISAAC", "RemoteServiceError", "Remote service internal error");

        /* Problems related to HP Passport */
        /// <summary>
        /// Fault returned when user with this mail already exist
        /// </summary>
        public static Fault EmailExists { get; } = new Fault("HPP", "EmailExists", "Customer profile with this EmailAddress already exists in database");
        /// <summary>
        /// Fault returned when user when we try to create new profile and user with this id already exists
        /// </summary>
        public static Fault UserIdExists { get; } = new Fault("HPP", "UserNameExists", "Customer profile with this UserName already exists");
        /// <summary>
        /// Fault returned when email address, that represents customer id is not found
        /// </summary>
        public static Fault EmailAddressNotFound { get; } = new Fault("HPP", "EmailAddressNotFound", "UserID field is missing or empty");

        public static Fault MustUseDiffPasswordOrUserName { get; } = new Fault("HPP", "MustUseDiffPasswordOrUserName", "Password cannot be the same as UserName");
        /// <summary>
        /// Fault returned when password is too short
        /// </summary>
        public static Fault PasswordTooShort { get; } = new Fault("HPP", "PasswordTooShort", "Password is too short");

        /// <summary>
        /// Fault returned when HPP is not available
        /// </summary>
        public static Fault AuthServiceNotAvailable { get; } = new Fault("HPP", "AuthServiceNotAvailable", "HPP Authentication Service not available");
        /// <summary>
        /// THIS SHOULD BE CLARIFIED - I don't know what this fault (and related field) is used for - Kuba 
        /// </summary>
        public static Fault ResetGuidNotFound { get; } = new Fault("HPP", "ResetGuidNotFound", "Current password is invalid");
        /// <summary>
        /// Fault returned when change id is requested, but new and old ids are same
        /// </summary>
        public static Fault UserNameIsTheSame { get; } = new Fault("HPP", "UserNameIsTheSame", "Given UserName is the same as current");

        public static Fault PasswordIsTheSame { get; } = new Fault("InnovationPortal", "PasswordIsTheSame", "New Password is the same as current password");

        public static Fault RequestError { get; } = new Fault("InnovationPortal", "RequestError", "Cannot create request: missing settings or invalid input data");
        public static Fault InvalidCaseId { get; } = new Fault("InnovationPortal", "InvalidCaseId", "Invalid case Id");

        /// <summary>
        /// Fault returned when device serial number is invalid
        /// </summary>
        public static Fault InvalidSerialNumber { get; } = new Fault("InvalidSerialNumber", "SerialNumber field is invalid");
         public static Fault EmptyOrNullDevicetoken { get; } = new Fault("Required", "EmptyOrNullDevicetoken", "Mandatory devicetoken is missing or empty.");

        /* ISAC Errors */
        public static Fault ProductNotFound { get; } = new Fault("ISAC", "ProductNotFound", "Product with this ID cannot be found");

        public static Fault InvalidCredentials { get; } = new Fault("InvalidCredentials", "Given credentials are invalid");

        public static Fault AccountLocked { get; } = new Fault("AccountLocked", "Account has been locked for approximately one hour");

        public static Fault HPPPasswordReusedError { get; } = new Fault("HPP", "PasswordReusedError", "Password cannot be the same as one of your last 3 passwords.");

        public static Fault PasswordExpiredError { get; } = new Fault("HPP", "PasswordExpiredError", "Password has expired");

        public static Fault StateDataNotFound { get; } = new Fault("StateDataNotFound", "The given session id was not found.");

        public static Fault StateDataExpired { get; } = new Fault("StateDataExpired", "Requested data are expired.");
        public static Fault SessionTimeout { get; } = new Fault("SessionTimeout", "Token has expired", ErrorCategory.SessionTimeout);

        public static Fault IncorrectExistingPassword { get; } = new Fault("IncorrectExistingPassword", "Current password is incorrect");
        public static Fault InvalidPassword { get; } = new Fault("HPP", "InvalidPassword", "Password must contain a combination of three of the following categories: uppercase letters, lowercase letters, numbers, special characters.");


        public static Dictionary<int, Fault> HppErrorMap { get; } = new Dictionary<int, Fault>
        {
            {228, InvalidCredentials },
            {235, SessionTimeout },
            {236, InvalidCredentials },
            {370, InvalidCredentials },
            {372, InvalidCredentials },
            {371, AccountLocked },
            {376, InvalidCredentials },
            {237, HPPInternalError },
            {242, InvalidCredentials },
            {250, UserIdExists },
            {422, IncorrectExistingPassword },
            {474, PasswordIsTheSame },
            {666, UserNameIsTheSame },
            {254, MustUseDiffPasswordOrUserName },
            {964, InvalidCredentials },
            {885, UserIdExists },
            {271, EmailExists },
            {419, HPPPasswordReusedError },
            {373, PasswordExpiredError},
            {377, PasswordExpiredError},
            {415, InvalidPassword}
        };
        public static Fault ReCaptchaValidationFailed { get; } = new Fault("ReCaptchaValidationFailed", "ReCaptcha token is invalid");
        public static Fault MissingReCaptchaToken { get; } = new Fault("MissingReCaptchaToken", "Missing ReCaptcha token");

        public static Fault GenericError { get; } = new Fault("GenericError", "GenericError");
        public static Fault GenesisError { get; } = new Fault("Genesis", "RemoteServiceError", "Cannot get user subscribed agreements");
        public static Fault GenesisConnectionError { get; } = new Fault("Genesis", "RemoteServiceError", "Connection to remote service failed");
        public static Fault GenesisEndpointError { get; } = new Fault("Genesis", "RemoteServiceError", "Server Unavailable");
        public static Fault GenesisTimeoutError { get; } = new Fault("Genesis", "RemoteServiceError", "The service operation timed out");
        public static Fault GNewtonConnectionError { get; } = new Fault("GNewton", "RemoteServiceError", "Connection to remote service failed");
        public static Fault GNewtonEndpointError { get; } = new Fault("GNewton", "RemoteServiceError", "Server Unavailable");
        public static Fault GNewtonServiceOrderError { get; } = new Fault("GNewton", "RemoteServiceError", "Cannot get user service order.");
        public static Fault ServiceOrderExistError { get; } = new Fault("ServiceOrderError", "ServiceOrderExistError", "Service order is already linked to profile.");
        public static Fault ServiceOrderSearchValidationError { get; } = new Fault("ServiceOrderSearchError", "MissingEmailAddressOrSerialNumber", "Either Email Address or Serial Number is required in the request.");
        public static Fault TranslationServiceConnectionError { get; } = new Fault("SamsungTranslation", "RemoteServiceError", "Connection to remote service failed");
        public static Fault TranslationServiceTimeout { get; } = new Fault("SamsungTranslation", "RemoteServiceError", "The service operation timed out");
        public static Fault InvalidDeviceToken { get; } = new Fault("InvalidDeviceToken", "Unable to find mobile device with requested token");
        public static Fault MobileDeviceAlreadyExist { get; } = new Fault("MobileDeviceAlreadyExist", "Device already registered");
        public static Fault AccessDenied { get; } = new Fault("AccessDenied", "Access denied");
        public static Fault MissingEmailAddress { get; } = new Fault("MissingEmailAddress", "Email address is missing");
        public static Fault SendEmailError { get; } = new Fault("SendEmailError", "Sending email failed");
        public static Fault MissingEmailTemplate { get; } = new Fault("MissingEmailTemplate", "Email template is missing");
        public static Fault EmailTemplateNotFound { get; } = new Fault("EmailTemplateNotFound", "Email template not found");
        public static Fault LegacyProductError { get; } = new Fault("SamsungTranslation", "LegacyProduct", "Product not recognized");
        public static Fault SerialNumberNotRecognized { get; } = new Fault("SamsungTranslation", "SerialNumberNotRecognized", "Serial number not recognized");
        public static Fault SerialNumberNotMatchToProductNumber { get; } = new Fault("SamsungTranslation", "SerialNumberNotMatchToProductNumber", "Serial number found with different Product Number");

        public static Fault ObligationConnectionError { get; } = new Fault("Obligation", "RemoteServiceError", "Connection to remote service failed");
        public static Fault ObligationTimeoutError { get; } = new Fault("Obligation", "RemoteServiceError", "The service operation timed out");

         public static Fault HPIDInternalError { get; } = new Fault("HPID", "RemoteServiceError", "Remote service internal error");
        public static Fault HPIDTimeoutError { get; } = new Fault("HPID", "RemoteServiceError", "The service operation timed out");
        public static Fault HPIDInvalidResponse { get; } = new Fault("HPID", "RemoteServiceError", "Remote service returned invalid response");
        public static Fault HPIDUserExists { get; } = new Fault("HPID", "EmailExists", "Customer profile already exists");
        public static Fault HPIDInvalidToken { get; } = new Fault("HPID", "InvalidToken", "Access token is expired or otherwise invalid");
        public static Fault HPIDInvalidSyntax { get; } = new Fault("HPID", "InvalidSyntax", "Request has invalid syntax");
        public static Fault MissingHPIDidentification { get; } = new Fault("ISAAC", "MissingHPIDn", "Missing HPID profile ID");
        public static Fault ISAACUpdateProfileError { get; } = new Fault("ISAAC", "RemoteServiceError", "Update Online Customer failed");
        public static Fault HPIDAccountLocked { get; } = new Fault("HPID", "account_locked", "Your account has been locked out after too many incorrect attempts. Please use the “Forgot your username or password?” link to reset your password.");
        public static Fault HPIDSessionTimeout { get; } = new Fault("HPID", "SessionTimeout", "Token has expired", ErrorCategory.SessionTimeout);
        public static Fault HPIDEmailAddressNotFound { get; } = new Fault("HPID", "EmailNotFound", "Email address is empty in HPID response");

        /// <summary>
        /// CDAX service related errors
        /// </summary>
        public static Fault MissingCdaxRequestField { get; } = new Fault("MissingCdaxRequestField", "Required field missing in CDAX request");
        public static Fault EmailInvalidSyntax { get; } = new Fault("CDAX", "EmailInvalidSyntax", "Email address has invalid syntax");
        public static Fault InvalidCustomerIdInRequest { get; } = new Fault("CDAX", "InvalidCustomerIdInRequest", "UserID in request does not match with authenticated user UserID");
        public static Fault InValidAttachmentType { get; } = new Fault("CDAX", "CDAXRequestAttachmentError", "Attachment file name is not matching with the specified file type");
        public static Fault InvalidAttachmentContents { get; } = new Fault("CDAX", "CDAXRequestAttachmentError", "Attachment file content is not matching with the specified file type");
        public static Fault UnsupportedAttachmentType { get; } = new Fault("CDAX", "CDAXRequestAttachmentError", "Unsupported attachment type found in request");
        public static Fault MissingAttachmentDetails { get; } = new Fault("CDAX", "CDAXRequestAttachmentError", "Required attachment details are missing");
 

        public static Fault SNRConnectionError { get; } = new Fault("SNR", "SNGenError", "An error occurred. Please try again later.");
        public static Fault InvalidSNFormat { get; } = new Fault("InvalidSNFormat", "This Serial Number is not a valid HP Serial Number. Please verify and try again.", ErrorCategory.InvalidSNFormat);
        public static Fault SNNotFound { get; } = new Fault("SNNotFound", "This Serial Number could not be found. Please verify and try again.", ErrorCategory.SNNotFound);
         
        public static Fault InvalidJsonRequestField { get; } = new Fault("InvalidJson", "Request has invalid json.");
        public static Fault EmptyOrNullJsonRequest { get; } = new Fault("EmptyOrNullJsonRequest", "Request json is empty or null.");
        public static Fault InternalErrorWhileSetSessionJson { get; } = new Fault("SetSessionJson", "Internal error occured while setting cache.");
        public static Fault InternalErrorWhileGetSessionJson { get; } = new Fault("GetSessionJson", "Internal error occured while getting data from cache.");
        public static Fault EmptyOrNullCacheToken { get; } = new Fault("EmptyOrNullCacheToken", "CacheToken is empty or null.");
        public static Fault InvalidCacheToken { get; } = new Fault("InvalidCacheToken", "Invalid cache token.");
        public static Fault AuthExpired { get; } = new Fault("AuthExpired", "The authorization exchange period of 30 seconds has been reached.");
        public static Fault AuthClaimed { get; } = new Fault("AuthClaimed", "The authorization exchange has already taken place.");
        public static Fault ServerIsBusy { get; } = new Fault("ServerIsBusy", "Server currently cannot process the request");
    }
}