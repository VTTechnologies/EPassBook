using EPassBook.DAL.DBModel;
using EPassBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPassBook.Mapper
{
    public static class InstallmentDetailsMapper
    {

        public static InstallmentDetail Attach(InstallmentDetailsViewModel installmentDetailsViewModel)
        {
            InstallmentDetail installmentDetail = new InstallmentDetail();              
        
            return installmentDetail;
        }
        public static InstallmentDetailsViewModel Detach(InstallmentDetail installmentDetail)
        {
            InstallmentDetailsViewModel installmentDetailsViewModel = new InstallmentDetailsViewModel();            

            return installmentDetailsViewModel;
        }
    }
}