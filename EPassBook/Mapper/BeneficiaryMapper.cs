using EPassBook.DAL.DBModel;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Mapper
{
    public static class BeneficiaryMapper
    {

        public static BenificiaryMaster Attach(BeneficiaryViewModel beneficiaryViewModel)
        {
            BenificiaryMaster beneficiaryMaster = new BenificiaryMaster();
            return beneficiaryMaster;
        }

       
        public static BeneficiaryViewModel Detach(BenificiaryMaster beneficiaryMaster)
        {
            BeneficiaryViewModel beneficiaryViewModel = new BeneficiaryViewModel();            

            return beneficiaryViewModel;
        }
    }
}