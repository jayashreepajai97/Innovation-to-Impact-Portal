using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaDatabase.Validation;
using IdeaDatabase.Responses;
using System.ComponentModel.DataAnnotations;
using IdeaDatabase.Utils;

namespace IdeaDatabase.Interchange 
{
    public class CredentialsInterchange 
    {
        public int UserID { get; set; }
        [RequiredValidation]
        public string SessionToken { get; set; }

        //[DbFieldValidation("UserAuthentications")]
        public string CallerId { get; set; }
    }
}