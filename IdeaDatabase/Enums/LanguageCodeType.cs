namespace IdeaDatabase.Enums
{
    public enum LanguageCodeType
    {
        ar,
        bg,
        cs,
        da,
        de,
        el,
        en,
        es,
        et,
        fi,
        fr,
        he,
        hr,
        hu,
        id,
        it,
        ja,
        ko,
        lt,
        lv,
        nb,
        nl,
        no,
        pl,
        pt,
        ro,
        ru,
        sk,
        sl,
        sr,
        sv,
        th,
        tr,
        uk,
        zh,
        zf
    }

    public static class LanguageCodeExtension
    {
        public static string MapToHPPLangCode(this string lang)
        {
            if (lang == "zh")
                return "12";
            else if (lang == "zf")
                return "13";
            return lang;
        }
        public static string MapFromHPPLangCode(this string lang)
        {
            if (lang == "12")
                return "zh";
            else if (lang == "13")
                return "zf";
            return lang;
        }
    }
}