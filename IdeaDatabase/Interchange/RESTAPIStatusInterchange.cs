using IdeaDatabase.DataContext;
using System;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIStatusInterchange
    {
        public int IdeaStatusId { get; set; }
        public int Status { get; set; }
        public int ModifiedByUserID { get; set; }      
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public RESTAPIStatusInterchange(IdeaStatusLog ideaState)
        {
            if (ideaState != null)
            {
                IdeaStatusId = ideaState.IdeaStatusLogId;
                Status = ideaState.IdeaState;
                CreatedDate = ideaState.CreatedDate;
                ModifiedByUserID = ideaState.ModifiedByUserId;
            }
          
        }
    }
}