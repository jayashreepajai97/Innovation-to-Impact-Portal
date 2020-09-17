using IdeaDatabase.DataContext;
using IdeaDatabase.Enums;
using IdeaDatabase.Utils.IImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIIdeaBasicDetailsInterchange
    {
        public int IdeaId { get; set; }
        public string Title { get; set; }

        public RESTAPIIdeaBasicDetailsInterchange()
        {
  
        }

    }
}