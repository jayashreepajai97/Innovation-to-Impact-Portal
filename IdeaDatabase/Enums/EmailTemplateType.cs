using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Enums
{
    public enum EmailTemplateType
    {
        // Step 1: Idea created
        IdeaCreatedSubmitter,
        IdeaAssignedReviewer,

        // Step 2: Idea Reviewed
        IdeaReviewedSubmitter,
        IdeaReviewedReviewer,
        IdeaAssignedSponsor,

        // Step 2: Idea Sponsored
        IdeaSponsoredSubmitter,
        IdeaSponsoredReviewer,
        IdeaSponsoredSponsor

    }
}