using IdeaDatabase.DataContext;
using IdeaDatabase.Interchange;

namespace IdeaDatabase.Utils.Interface
{
    public interface IIdeaUtils
    {
        int GetIdeaState(Idea idea);
        string getStatus(Idea idea);
        string getImagePath(Idea idea);
        string getDefaultImagePath();
        RESTAPIIdeaDetailsInterchange GetIdeasDetails(IIdeaDatabaseDataContext context, int ideaId);
    }
}
