using CallingScore.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CallingScore.Prism.Helpers
{
    public static class CombosHelper
    {
        public static List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Supervisor" },
                new Role { Id = 2, Name = "Call Adviser" }
            };
        }
    }

}
