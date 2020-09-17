using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils
{
    public class StringUtils
    {
        public static List<string> GetTagList(string tags)
        {
            List<string> tagslist = new List<string>();
            if (tags.Contains(","))
                tagslist = tags.TrimEnd(',').Split(',').ToList();
            else
                tagslist.Add(tags);

            return tagslist;
        }
    }
}