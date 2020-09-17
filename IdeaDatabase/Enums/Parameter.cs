using SettingsRepository;

namespace IdeaDatabase.Enums
{
    public class Parameter
    {
        public readonly string Name;
        public object DefaultValue;

        public Parameter(string name, string defaultValue)
        {
            Name = name;
            DefaultValue = (string)defaultValue;
        }

        public static readonly Parameter ResourceFileCheckSumConst = new Parameter("CheckSumKey", "");
        public static readonly int PushNoficationsMinTimeDifference = SettingRepository.Get<int>("PushNoficationsMinTimeDifference", 0);
    }
}