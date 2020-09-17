using Newtonsoft.Json.Converters;

namespace IdeaDatabase.Responses
{
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter(string customFormat)
        {
            base.DateTimeFormat = customFormat;
        }
    }
}
