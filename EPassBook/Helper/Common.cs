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
            ProjectEngineer = 4,
            CityEngineer = 5,
            ChiefOfficer = 6,
            Accountant = 7,
            LastChiefOfficer = 8,
            Rejected = 9,
            Recommended = 10
        }
      public enum Roles
      {
          Admin = 1,
          DataEntry = 2,
          SiteEngineer = 3,
          ProjectManager = 4,
          CityEngineer = 5,
          ChiefOfficer = 6,
          Accountant = 7,
          Beneficiary = 8
      }

        public const string Admin = "Admin";
        public const string DataEntry = "Data Entry";
        public const string SiteEngineer = "Site Engineer";
        public const string ProjectEngineer = "Project Engineer";
        public const string CityEngineer = "City Engineer";
        public const string ChiefOfficer = "Chief Officer";
        public const string Accountant = "Accountant";
        public const string Beneficiary = "Beneficiary";

    }
}