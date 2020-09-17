using IdeaDatabase.DataContext;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaDatabase.Utils.Interface
{
    public interface ITagsUtils
    {
        void InsertTags(ResponseBase response, Idea idea, string Tags, int UserId);
    }
}
