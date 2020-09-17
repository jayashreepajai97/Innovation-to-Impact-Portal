using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InnovationPortalService.Idea
{
    public class RESTAPIDeviceWithDbContext
    {
        public IdeaDatabase.DataContext.IdeaDatabaseDataContext DbContext { get; set; }
    }
}