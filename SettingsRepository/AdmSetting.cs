using System;

namespace SettingsRepository
{
    public class AdmSettings
    {
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public string StringValue { get; set; }
        public Nullable<decimal> NumValue { get; set; }
        public Nullable<System.DateTime> DateValue { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string Version { get; set; }
    }
}
