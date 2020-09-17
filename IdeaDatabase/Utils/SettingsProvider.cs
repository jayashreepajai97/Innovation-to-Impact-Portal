using IdeaDatabase.DataContext;
using Responses;
using SettingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils
{
    public static class SettingsProvider
    {
        public static List<AdmSettings> admSettings;

        public static List<SettingsRepository.AdmSettings> LoadSettingsFromDatabase()
        {
            List<AdmSetting> settingsfromDB = new List<AdmSetting>();
            admSettings = new List<AdmSettings>();
            DatabaseWrapper.databaseOperation(new ResponseBase(),
                (context, query) =>
                {
                    settingsfromDB = context.AdmSettings.ToList();
                },
                readOnly: true
            );

            settingsfromDB.ForEach(x => admSettings.Add(InterchangeAdmSetting(x)));

            return admSettings;
        }

        public static void AddUpdateSetting(IIdeaDatabaseDataContext ctx, AdmSetting NewAdmSetting)
        {
            AdmSetting admSetting = ctx.AdmSettings.Where(x => x.ParamName == NewAdmSetting.ParamName).FirstOrDefault();
            if (admSetting != null)
            {
                admSetting.StringValue = NewAdmSetting.StringValue;
            }
            else
            {
                admSetting = new AdmSetting()
                {
                    ParamName = NewAdmSetting.ParamName,
                    ParamType = NewAdmSetting.ParamType,
                    StringValue = NewAdmSetting.StringValue,
                    Version = string.IsNullOrEmpty(NewAdmSetting.Version) ? "" : NewAdmSetting.Version
                };
                ctx.AdmSettings.Add(admSetting);
            }
            ctx.SubmitChanges();
        }

        private static SettingsRepository.AdmSettings InterchangeAdmSetting(DataContext.AdmSetting dbSetting)
        {
            return new SettingsRepository.AdmSettings
            {
                ParamName = dbSetting.ParamName,
                ParamType = dbSetting.ParamType,
                StringValue = dbSetting.StringValue,
                Version = dbSetting.Version,
                NumValue = dbSetting.NumValue,
                DateValue = dbSetting.DateValue,
                UpdateDate = dbSetting.UpdateDate
            };
        }
    }
}