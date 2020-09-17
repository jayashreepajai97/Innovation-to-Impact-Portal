using Newtonsoft.Json;
using System;
using System.Globalization;

namespace IdeaDatabase.Utils
{
    public class CustomDateTime
    {
        [JsonIgnore]
        public bool IsValid;
        [JsonIgnore]
        public DateTime? Date;
        [JsonIgnore]
        public string Text;
        protected CustomDateTime(string s, string format = CustomDateTimeFormat.AllowedFormats)
        {
            Text = s;
            string[] allowedFormat = format.Split(new char[] { '|' });
            try
            {
                DateTime dt;
                IsValid = DateTime.TryParseExact(s, allowedFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dt);

                if (IsValid)
                {
                    if (dt.TimeOfDay == TimeSpan.Zero && dt.Kind != DateTimeKind.Utc)
                    {                                               
                        // if TimeSpan is required but is empty, then set it to current time
                        Date = DateTime.SpecifyKind(dt, DateTimeKind.Utc) + DateTime.Now.TimeOfDay;                       
                    }
                    else if (dt.Kind == DateTimeKind.Utc)
                    {
                        Date = dt;
                    }
                    else
                    {
                        Date = null;
                        IsValid = false;
                    }
                }
                else
                {
                    Date = null;
                }
            }
            catch (Exception)
            {
                IsValid = false;
                Date = null;
            }
        }
        
        protected CustomDateTime(DateTime? d, string format = CustomDateTimeFormat.Iso8601Format)
        {
            Date = d;
            Text = d.HasValue ? d.Value.ToString(format) : null;
        }

        public static implicit operator DateTime? (CustomDateTime d)
        {
            return d == null ? null : d.Date;
        }

        public static implicit operator CustomDateTime(DateTime? d)
        {
            return new CustomDateTime(d);
        }

        public static implicit operator string(CustomDateTime d)
        {
            return d == null ? null : d.Text;
        }

        public static implicit operator CustomDateTime(string s)
        {
            return new CustomDateTime(s);
        }

        public static implicit operator CustomDateTime(CustomDateTimeFormat o)
        {
            return new CustomDateTime(o.value, o.format);
        }
    }

    public class CustomDate : CustomDateTime
    {
        private CustomDate(DateTime? d) : base(d, CustomDateTimeFormat.DateOnlyFormat)
        {

        }

        private CustomDate(string d) : base(d, CustomDateTimeFormat.DateOnlyFormat)
        {

        }

        public static implicit operator DateTime? (CustomDate d)
        {
            return d == null ? null : d.Date;
        }

        public static implicit operator CustomDate(DateTime? d)
        {
            return new CustomDate(d);
        }

        public static implicit operator string(CustomDate d)
        {
            return d == null ? null : d.Text;
        }

        public static implicit operator CustomDate(string s)
        {
            return new CustomDate(s);
        }
    }
}