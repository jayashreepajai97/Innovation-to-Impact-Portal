using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Responses;
using IdeaDatabase.Utils.Interface;
using Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Utils.IImplementation
{
    public class IdeaCategoryUtils : IideaCategoryUtils
    {


        //void
        public void DeleteCategory(ResponseBase response, int CategoryId, int AddedByUserId)
        {
            if (this.CheckIfIdeaExistsForCategory(response, CategoryId)) {
                DatabaseWrapper.databaseOperation(response,
                 (context, query) =>
                 {

                     IdeaCategory ideaCategory = query.GetCategoreById(context, CategoryId);
                     if (ideaCategory != null)
                     {
                         query.DeleteIdeaCategory(context, ideaCategory);
                         response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                     }
                     else
                     {
                         response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                         response.ErrorList.Add(Faults.IdeaCategoriesIDNotExists);
                         return;
                     }

                     context.SubmitChanges();
                 }
               , readOnly: false

               );

                if (response == null && response.ErrorList.Count != 0)
                {
                    response.ErrorList.Add(Faults.ServerIsBusy);
                    return;
                }
            }
            return;
        }

        public bool CheckIfIdeaExistsForCategory(ResponseBase response, int CategoryId)
        {
            bool ideaExists = false;
            DatabaseWrapper.databaseOperation(response,
             (context, query) =>
             {

                 if (query.CheckIfIdeaExistsForCategory(context, CategoryId))
                 {
                     ideaExists = false;
                     response.ErrorList.Add(Faults.IdeaExistsForCategory);
                 }
                 else
                 {
                     ideaExists = true;
                 }

                 context.SubmitChanges();
             }
           , readOnly: false

           );

            return ideaExists;
        }
        public RestAPIGetIdeaCategoryResponse GetCategory(RestAPIGetIdeaCategoryResponse response)
        {

            DatabaseWrapper.databaseOperation(response,
           (context, query) =>
           {
               List<IdeaCategory> ideaCategories = query.GetCategories(context);

               if (ideaCategories.Count != 0)
               {
                   response.categories = ideaCategories.Select(s => new IdeaCatrgoryResponse() { Category = s.CategoriesName, Id = s.IdeaCategorieId }).ToList();
                   response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
               }
               else
               {
                   response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure); ;
               }

           }, readOnly: true);

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
            }
            return response;
        }

        public void InsertCategory(RestAPIAddIdeaCatrgoryResponse response, string Category, int AddedByUserId)
        {

            IdeaCategory ideacategory;
            DatabaseWrapper.databaseOperation(response,
            (context, query) =>
            {
                IdeaCategory ideaCategory = query.GetCategoreByName(context, Category);
                if (ideaCategory == null)
                {
                    ideacategory = new IdeaCategory() { CategoriesName = Category, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, AddedByUserId = AddedByUserId };
                    query.AddIdeaCategory(context, ideacategory);
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                }
                else
                {
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                    response.ErrorList.Add(Faults.IdeaCategoriesNameExists);
                    return;
                }
                context.SubmitChanges();
            }
            , readOnly: false
            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                return;
            }

        }

        public void UpdateCategory(RestAPIUpdateCategoryResponse response, string Category, int IdeaCategorieID)
        {
            DatabaseWrapper.databaseOperation(response,
            (context, query) =>
            {
                IdeaCategory ideacategory;
                ideacategory = query.GetIdeaFromCategoryID(context, IdeaCategorieID);

                if (ideacategory != null)
                {
                    ideacategory.CategoriesName = Category;
                    ideacategory.ModifiedDate = DateTime.UtcNow;
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Success);
                }
                else
                {
                    response.Status = Enum.GetName(typeof(ResponseStatusType), ResponseStatusType.Failure);
                    response.ErrorList.Add(Faults.IdeaCategoriesIDNotExists);
                    return;
                }
                context.SubmitChanges();
            }
            , readOnly: false

            );

            if (response == null && response.ErrorList.Count != 0)
            {
                response.ErrorList.Add(Faults.ServerIsBusy);
                return;
            }
        }

    }
}