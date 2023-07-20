using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Domain.Models.Admin
{
    public class ChangeUserRoleRequestModel
    {
        public string UserId { get; set; }

        public string RoleName { get; set; }
    }
}
