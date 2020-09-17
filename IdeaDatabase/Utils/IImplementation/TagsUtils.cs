using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class TagsUtils : ITagsUtils
    {

        string Failure = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
        string Success = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);

        public void InsertTags(ResponseBase response, Idea idea, string Tags, int UserId)
        {
            DatabaseWrapper.databaseOperation(response,
          (context, query) =>
          {
              try
              {
                  List<string> tags = GetTagList(Tags);
                  if (idea.Ideatags.Count > 0)
                  {
                      query.DeleteIdeaTags(context, idea.IdeaId);
                      context.SubmitChanges();
                  }

                  tags.ForEach((t) =>
                  {
                      if (!string.IsNullOrWhiteSpace(t))
                          idea.Ideatags.Add(new Ideatag() { IdeaId = idea.IdeaId, AddedByUserId = UserId, CreatedDate = DateTime.UtcNow, Tags = t });
                  });
                  response.Status = Success;
              }
              catch (Exception)
              {
                  response.Status = Failure;
                  return;
              }
          }
          , readOnly: false
             );
        }

        private List<string> GetTagList(string tags)
        {
            List<string> tagslist = new List<string>();
            if (tags != null)
            {
                if (tags.Contains(","))
                    tagslist = tags.TrimEnd(',').Split(',').ToList();
                else
                    tagslist.Add(tags);
            }

            return tagslist;
        }

    }
}