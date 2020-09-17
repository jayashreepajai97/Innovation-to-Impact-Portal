﻿using IdeaDatabase.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Requests
{
    public class RestAPIAddIdeaStateRequest : ValidableObject
    {
        [NumberValidation(1, required: true)]
        public int IdeaID { get; set; }
        //public string IdeaStatus { get; set; }
    }
}