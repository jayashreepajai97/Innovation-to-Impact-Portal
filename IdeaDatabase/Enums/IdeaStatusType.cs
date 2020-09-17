using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Enums
{
    public enum IdeaStatusTypes
    {
        Default = 0,
        [Description("Review Pending")]
        ReviewPending = 1,   // Submitted     &&   Ready for Review
        [Description("Sponsor Pending")]
        SponsorPending = 2,    // Reviewed      &&   Ready for Sponsor Review
        [Description("Sponsored")]
        Sponsored = 3,    // Sponsored
        [Description("Sales")]
        Sales = 4,
        [Description("Submit Pending")]
        SubmitPending = 5   // Draft && Submit pending
    }
}