namespace IdeaDatabase.Utils
{
    public class CustomDateTimeFormat
    {
        /*
        The date fields in request are accepted only in the input format mentioned below. All other
        formats are rejected. The date and datetime values are stored in DB in UTC only. The date 
        and datetime fields are always reported back in UTC. The following tables summarizes the 
        logic. In short, if only date is provided (scenario #1), time part is assumed to be UTC 
        midnight. But if time is also provided, it has to be either the client local time with the 
        time zone offset (scenario #2) or the equivalent UTC time (scenario #3).
        ===========================================================================================
        |    Input (In Request)     |            Saved in DB               | Output (In Response) |
        |=========================================================================================|
        |2018-09-15                 | 2018-09-15T00:00:00 (treat as UTC)   | 2018-09-15T00:00:00Z |
        |2018-09-15T18:30:00+05:30  | 2018-09-15T13:00:00 (convert to UTC) | 2018-09-15T13:00:00Z |
        |2018-09-15T13:00:00Z       | 2018-09-15T13:00:00 (already in UTC) | 2018-09-15T13:00:00Z |
        ===========================================================================================
        */

        #region Custom Formats

        // Input Formats
        public const string DateOnlyFormat = @"yyyy-MM-dd";
        public const string DateTimeFormat = @"yyyy-MM-ddTHH:mm:ssK";
        public const string AllowedFormats = @"yyyy-MM-dd|yyyy-MM-ddTHH:mm:ssK";
        //Exceptional case useded only for warranty.
        public const string YYYYMMDDFormat = @"yyyyMMdd"; 
        // Output Format
        public const string Iso8601Format  = @"yyyy-MM-ddTHH:mm:ssZ";

        #endregion Custom Formats

        public string value;
        public string format;

        public CustomDateTimeFormat(string value, string format)
        {
            this.value = value;
            this.format = format;
        }
    }
}