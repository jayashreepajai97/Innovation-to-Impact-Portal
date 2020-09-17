using IdeaDatabase.Responses;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IdeaDatabase.Utils.Interface
{
    public interface IideaCategoryUtils
    {
        void InsertCategory(RestAPIAddIdeaCatrgoryResponse response, string CategoriesName, int AddedByUserId);
        void DeleteCategory(ResponseBase response, int CategoryId, int AddedByUserId);
        bool CheckIfIdeaExistsForCategory(ResponseBase response, int CategoryId);
        RestAPIGetIdeaCategoryResponse GetCategory(RestAPIGetIdeaCategoryResponse response);
        void UpdateCategory(RestAPIUpdateCategoryResponse response, string CategoriesName, int IdeaCategorieID);

    }
}
