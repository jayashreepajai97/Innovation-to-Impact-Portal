using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIAddIdeaCatrgoryRequest : ValidableObject
    {

        public string CategoriesName { get; set; }
        public int AddedByUserId { get; set; }


    }
}