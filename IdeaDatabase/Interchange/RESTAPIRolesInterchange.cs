using IdeaDatabase.DataContext;
using System;

namespace IdeaDatabase.Interchange
{
    public class RESTAPIRolesInterchange
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public RESTAPIRolesInterchange(Role role)
        {
            if (role != null)
            {
                RoleId = role.RoleId;
                RoleName = role.RoleName;
                CreatedDate = role.CreatedDate;
                IsActive = role.IsActive;
            }
        }
    }
}