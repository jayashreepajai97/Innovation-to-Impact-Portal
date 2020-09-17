using System;
using System.Globalization;

namespace IdeaDatabase.Utils
{
    public class DateTimeFormatUtils
    {
        public const string Iso8601FormatString = @"yyyy-MM-ddTHH:mm:ssZ";

        /// <summary>
        /// Returns date in formate adequate to given localization
        /// </summary>
        /// <param name="baseDateValue">Raw date format</param>
        /// <param name="localeId">Localization</param>
        /// <returns>Formatted date if raw Date is proper and format was found for given localization. Otherwise returns empty string.</returns>
        public static string FormatDate(string baseDateValue, string localeId = null)
        {
            if (string.IsNullOrEmpty(baseDateValue))
            {
                return string.Empty;
            }

            try
            {
                DateTime? rawDate = new DateTime(DateTime.ParseExact(baseDateValue, "yyyy-MM-ddTHH:mm:ssZ", null).Ticks, DateTimeKind.Utc);
                if (rawDate == null)
                {
                    return string.Empty;
                }
                if (!rawDate.HasValue)
                {
                    return string.Empty;
                }
                string format = SelectDateFormat(localeId);
                return rawDate.Value.ToString(format, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns date in formate adequate to given localization
        /// </summary>
        /// <param name="baseDateValue">Raw date format</param>
        /// <returns>Formatted date if baseDateValue is proper and was successfully formated to date and time stamp. Otherwise returns null.</returns>
        public static string FormatToDateWithTimestamp(DateTime? baseDateValue)
        {
            if (baseDateValue == null)
            {
                return null;
            }
            if (!baseDateValue.HasValue)
            {
                return null;
            }
            try
            {
                return baseDateValue.Value.ToString(Iso8601FormatString, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// Method verifies locale and selects adequate Date format
        /// </summary>
        /// <param name="localeId">Localization</param>
        /// <returns>Date format adequate for given localization</returns>
        public static string SelectDateFormat(string localeId)
        {
            if (string.IsNullOrEmpty(localeId))
            {
                return Iso8601FormatString;
            }

            // list of allowed formats and localizations should be expanded
            // if (localeId.Equals("de-DE"))
            //    return "dd.MM.yyyy";

            return Iso8601FormatString;
        }

        /// <summary>
        /// Converts the nullable DateTime to ISO8601 format datetime stamp
        /// </summary>
        /// <param name="dateTimeStamp">The nullable DateTime object</param>
        /// <returns>The datetime representation in ISO8601 format</returns>
        public static string GetIso8601String(DateTime? dateTimeStamp)
        {
            string returnValue = null;
            if (dateTimeStamp != null && dateTimeStamp.HasValue)
            {
                try
                {
                    // All incming dates as treated as UTC. The Iso8601FormatString
                    // will make ToString(...) to append "Z" suffix at the end of 
                    // the string. Later on when we add support for time stamps 
                    // in database and for other interacting services, conversion into
                    // the appropriate time zone will be required. Right now all dates 
                    // are assumed to be in UTC which may not be valid in future.
                    returnValue = dateTimeStamp.Value.ToString(Iso8601FormatString);
                }
                catch (FormatException)
                {
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts DateTime string to nullable DateTime object
        /// </summary>
        /// <param name="dateTimeStamp">Input string to parse into DateTime</param>
        /// <returns>DateTime object representing the time given in string</returns>
        public static DateTime? GetDateTime(string dateTimeStamp)
        {
            DateTime? returnValue = null;
            if (!string.IsNullOrEmpty(dateTimeStamp))
            {
                try
                {
                    returnValue = Convert.ToDateTime(dateTimeStamp, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts the nullable DateTime to ISO8601 format datetime stamp
        /// </summary>
        /// <param name="dateTimeStamp">The nullable DateTime object</param>
        /// <returns>The datetime representation in ISO8601 format in DateTime</returns>
        public static DateTime? GetIso8601DateTime(string dateTimeStamp)
        {
            DateTime? returnValue = null;
            if (!string.IsNullOrEmpty(dateTimeStamp))
            {
                try
                {
                    returnValue = new DateTime(DateTime.ParseExact(dateTimeStamp, Iso8601FormatString, null).Ticks, DateTimeKind.Utc);
                }
                catch (FormatException)
                {
                }
            }
            return returnValue;
        }

        public static DateTime? GetDateTime(string dateTimeStamp, string specifyFormat = Iso8601FormatString)
        {
            DateTime? returnValue = null;
            if (!string.IsNullOrEmpty(dateTimeStamp))
            {
                try
                {
                    returnValue = new DateTime(DateTime.ParseExact(dateTimeStamp, specifyFormat, null).Ticks, DateTimeKind.Utc);
                }
                catch (FormatException )
                {
                  
                }
            }
            return returnValue;
        }
        public static TimeSpan GetOffSet(string offSetString)
        {
            TimeSpan offSet = new TimeSpan(0, 0, 0);

            if (string.IsNullOrEmpty(offSetString) || offSetString == "0")
                return offSet;

            if (offSetString.Length > 0)
            {
                string offSetType = (offSetString[0] == 'M') ? "-" : "+";

                if (offSetString.IndexOf(':') > 0)
                {
                    string[] time = offSetString.Split(':');
                    string hour = time[0].Substring(1, time[0].Length - 1);
                    string sec = time[0].Substring(1, time[0].Length - 1);

                    if (offSetType == "M")
                        offSet = new TimeSpan(-int.Parse(hour), int.Parse(sec), 0);
                    else
                        offSet = new TimeSpan(int.Parse(hour), int.Parse(sec), 0);
                }
                else if (offSetString.Length == 5)
                {
                    string hour = offSetString.Substring(1, 2);
                    string mins = offSetString.Substring(offSetString.Length - 2);

                    if (offSetType == "M")
                        offSet = new TimeSpan(-int.Parse(hour), int.Parse(mins), 0);
                    else
                        offSet = new TimeSpan(int.Parse(hour), int.Parse(mins), 0);

                }
                else
                {
                    string hour = offSetString.Substring(1, offSetString.Length - 1);

                    if (offSetType == "M")
                        offSet = new TimeSpan(-int.Parse(hour), 0, 0);
                    else
                        offSet = new TimeSpan(int.Parse(hour), 0, 0);
                }
            }
            return offSet;
        }

       
    }
}