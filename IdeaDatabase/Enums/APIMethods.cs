using System;

namespace IdeaDatabase.Enums
{
    public enum APIMethods
    {
        None                    = -1,
        POSTGetProfileByToken   = 1 << 1,
        POSTAuthenticate        = 1 << 2,
        GETProfile              = 1 << 3,
        PUTProfile              = 1 << 4,
        POSTGetProfile          = 1 << 5,
        POSTUpdateProfile       = 1 << 6
    }

    public class APIMethodsUtils
    {
        public const APIMethods APIMethodsAllowedForLocale =    APIMethods.POSTAuthenticate | 
                                                                APIMethods.POSTGetProfileByToken | 
                                                                APIMethods.GETProfile | 
                                                                APIMethods.POSTGetProfile | 
                                                                APIMethods.PUTProfile | 
                                                                APIMethods.POSTUpdateProfile; 
    }    
}