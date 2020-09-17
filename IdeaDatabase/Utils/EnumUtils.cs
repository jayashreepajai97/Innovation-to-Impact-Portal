using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace IdeaDatabase.Utils
{
    public class EnumUtils
    {



        public static bool Validate(Type e, string value, bool byDescription = false)
        {
            string[] types = Enum.GetNames(e);
            foreach (string s in types)
            {
                if (byDescription)
                {
                    var memInfo = e.GetMember(s);
                    var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptionAttributes.Length > 0)
                    {
                        string description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        if (description.Equals(value.ToString()))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (string.Compare(s, value) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //Parse string value to generic ENUM type.
        public static D ParseEnum<EnumType, D>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(D);
            }
            EnumType t = (EnumType)Enum.Parse(typeof(EnumType), value);
            return ConvertValue<D>(t);
        }

        //TryParse string value to generic ENUM type. If fails or null return false
        public static bool TryParseEnum<D>(string value, out D enumVal) where D : struct, IConvertible
        {
            if (string.IsNullOrEmpty(value))
            {
                enumVal = default(D);
                return false;
            }
            return Enum.TryParse(value, out enumVal);
        }

        //explicit datatype conversion
        public static T ConvertValue<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string GetDescriptionFromEnum(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

    }

    public static class EnumDescriptor
    {
        public static string GetEnumDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return null; // could also return string.Empty
        }
    }
}