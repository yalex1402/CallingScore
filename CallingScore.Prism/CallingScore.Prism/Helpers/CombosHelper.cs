using CallingScore.Common.Enums;
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

        public static List<Campaign> GetCampaigns()
        {
            return new List<Campaign>
            {
                new Campaign { Id = 1, Name = "Linea Entrada" },
                new Campaign { Id = 2, Name = "Linea Salida" }
            };
        }

        public static List<StatisticsType> GetStatisticsTypes()
        {
            return new List<StatisticsType>
            {
                new StatisticsType{ Id = 1, Name = "Contact" },
                new StatisticsType{ Id = 2, Name = "Effectivity" }
            };
        }

        public static List<Month> GetMonths()
        {
            return new List<Month>
            {
                new Month { Id = 1, Name = "January" },
                new Month { Id = 2, Name = "February" },
                new Month { Id = 3, Name = "March" },
                new Month { Id = 4, Name = "April" },
                new Month { Id = 5, Name = "May" },
                new Month { Id = 6, Name = "June" },
                new Month { Id = 7, Name = "July" },
                new Month { Id = 8, Name = "August" },
                new Month { Id = 9, Name = "September" },
                new Month { Id = 10, Name = "Octuber" },
                new Month { Id = 11, Name = "November" },
                new Month { Id = 12, Name = "December" }
            };
        }

    }
}
