using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Helper
{
    public static class Common
    {
      public enum WorkFlowStages
        {
            DataEntry = 1,
            UserRequest = 2,
            SiteEngineer = 3,
            ProjectManager = 4,
            CityEngineer = 5,
            ChiefOfficer = 6,
            Accountant = 7,
            LastChiefOfficer = 8
        }
    }
}