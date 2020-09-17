using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdeaDatabase.Credentials
{
    public class ApplicationVersion : IComparable<ApplicationVersion>, IEquatable<ApplicationVersion>
    {
        private static char Delimiter = '.';

        private List<byte> versionTokens;

        public ApplicationVersion(string version)
        {
            versionTokens = version.Split(Delimiter).Select(x => Convert.ToByte(x)).ToList();
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (int token in versionTokens)
            {
                hashCode ^= token;
            }
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ApplicationVersion);
        }

        public bool Equals(ApplicationVersion v)
        {
            if (ReferenceEquals(v, null))
            {
                return false;
            }

            if (versionTokens.Count != v.versionTokens.Count)
            {
                return false;
            }

            for (int i = 0; i < versionTokens.Count; i++)
            {
                if (versionTokens[i] != v.versionTokens[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int CompareTo(ApplicationVersion inputVersion)
        {
            int comparisonResult = 0;
            int i = 0;
            for (i = 0; i < versionTokens.Count; i++)
            {
                if (i >= inputVersion.versionTokens.Count)
                {
                    comparisonResult = 1;
                    break;
                }
                if (versionTokens[i] < inputVersion.versionTokens[i])
                {
                    comparisonResult = -1;
                    break;
                }
                if (versionTokens[i] > inputVersion.versionTokens[i])
                {
                    comparisonResult = 1;
                    break;
                }
            }

            if (i == versionTokens.Count)
            {
                if (inputVersion.versionTokens.Count == versionTokens.Count)
                {
                    comparisonResult = 0;
                }
                else if (inputVersion.versionTokens.Count > versionTokens.Count)
                {
                    comparisonResult = -1;
                }
            }

            return comparisonResult;
        }

        public static bool operator ==(ApplicationVersion v1, ApplicationVersion v2)
        {
            if (ReferenceEquals(v1, null))
            {
                return ReferenceEquals(v2, null) ? true : false;
            }
            return v1.Equals(v2);
        }

        public static bool operator !=(ApplicationVersion v1, ApplicationVersion v2)
        {
            return !(v1 == v2);
        }

        public static bool operator <(ApplicationVersion v1, ApplicationVersion v2)
        {
            if (ReferenceEquals(v1, null))
            {
                return false;
            }
            else
            {
                return (v1.CompareTo(v2) < 0) ? true : false;
            }
        }

        public static bool operator >(ApplicationVersion v1, ApplicationVersion v2)
        {
            if (ReferenceEquals(v1, null))
            {
                return false;
            }
            else
            {
                return (v1.CompareTo(v2) > 0) ? true : false;
            }
        }

        public static bool operator <=(ApplicationVersion v1, ApplicationVersion v2)
        {
            return (v1 < v2) || (v1 == v2);
        }

        public static bool operator >=(ApplicationVersion v1, ApplicationVersion v2)
        {
            return (v1 > v2) || (v1 == v2);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (int token in versionTokens)
            {
                result.Append(token.ToString());
                result.Append(".");
            }
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }
    }
}