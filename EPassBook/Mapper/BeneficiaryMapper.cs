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

            beneficiaryMaster.BeneficiaryId = beneficiaryViewModel.BeneficiaryId;
            beneficiaryMaster.Hasband_Photo = beneficiaryViewModel.Hasband_Photo;
            beneficiaryMaster.Wife_Photo = beneficiaryViewModel.Wife_Photo;
            beneficiaryMaster.BeneficairyName = beneficiaryViewModel.BeneficairyName;
            beneficiaryMaster.FatherName = beneficiaryViewModel.FatherName;
            beneficiaryMaster.Mother = beneficiaryViewModel.Mother;
            beneficiaryMaster.MobileNo = beneficiaryViewModel.MobileNo;
            beneficiaryMaster.CityId = beneficiaryViewModel.CityId;
            beneficiaryMaster.DTRNo = beneficiaryViewModel.DTRNo;
            beneficiaryMaster.RecordNo = beneficiaryViewModel.RecordNo;
            beneficiaryMaster.Class = beneficiaryViewModel.Class;
            beneficiaryMaster.General = beneficiaryViewModel.General;
            beneficiaryMaster.Single = beneficiaryViewModel.Single;
            beneficiaryMaster.Disabled = beneficiaryViewModel.Disabled;
            beneficiaryMaster.Password = beneficiaryViewModel.Password;
            beneficiaryMaster.AdharNo = beneficiaryViewModel.AdharNo;
            beneficiaryMaster.VoterID = beneficiaryViewModel.VoterID;
            beneficiaryMaster.Area = beneficiaryViewModel.Area;
            beneficiaryMaster.MojaNo = beneficiaryViewModel.MojaNo;
            beneficiaryMaster.KhataNo = beneficiaryViewModel.KhataNo;
            beneficiaryMaster.KhasraNo = beneficiaryViewModel.KhasraNo;
            beneficiaryMaster.PlotNo = beneficiaryViewModel.PlotNo;
            beneficiaryMaster.PoliceStation = beneficiaryViewModel.PoliceStation;
            beneficiaryMaster.WardNo = beneficiaryViewModel.WardNo;
            beneficiaryMaster.District = beneficiaryViewModel.District;
            beneficiaryMaster.BankName = beneficiaryViewModel.BankName;
            beneficiaryMaster.BranchName = beneficiaryViewModel.BranchName;
            beneficiaryMaster.IFSCCode = beneficiaryViewModel.IFSCCode;
            beneficiaryMaster.AccountNo = beneficiaryViewModel.AccountNo;
            beneficiaryMaster.CreatedBy = beneficiaryViewModel.CreatedBy;
            beneficiaryMaster.CreatedDate = DateTime.Now;
            beneficiaryMaster.CompanyID = beneficiaryViewModel.CompanyID;

            //beneficiaryMaster.InstallmentDetails = beneficiaryViewModel.InstallmentDetails.Select(s => new InstallmentDetail { BeneficiaryId = s.BeneficiaryId, StageID = s.StageID, CreatedDate = DateTime.Now, CreatedBy= s.CreatedBy }).ToList();           
            return beneficiaryMaster;
        }

       
        public static BeneficiaryViewModel Detach(BenificiaryMaster beneficiaryMaster)
        {
            BeneficiaryViewModel beneficiaryViewModel = new BeneficiaryViewModel();
            beneficiaryViewModel.Hasband_Photo = beneficiaryMaster.Hasband_Photo;
            beneficiaryViewModel.Wife_Photo = beneficiaryMaster.Wife_Photo;
            beneficiaryViewModel.BeneficairyName = beneficiaryMaster.BeneficairyName;
            beneficiaryViewModel.FatherName = beneficiaryMaster.FatherName;
            beneficiaryViewModel.Mother = beneficiaryMaster.Mother;
            beneficiaryViewModel.MobileNo = beneficiaryMaster.MobileNo;
            beneficiaryViewModel.CityId = beneficiaryMaster.CityId;
            beneficiaryViewModel.DTRNo = beneficiaryMaster.DTRNo;
            beneficiaryViewModel.RecordNo = beneficiaryMaster.RecordNo;
            beneficiaryViewModel.Class = beneficiaryMaster.Class;
            beneficiaryViewModel.General = beneficiaryMaster.General;
            beneficiaryViewModel.Single = beneficiaryMaster.Single;
            beneficiaryViewModel.Disabled = beneficiaryMaster.Disabled;
            beneficiaryViewModel.Password = beneficiaryMaster.Password;
            beneficiaryViewModel.AdharNo = beneficiaryMaster.AdharNo;
            beneficiaryViewModel.VoterID = beneficiaryMaster.VoterID;
            beneficiaryViewModel.Area = beneficiaryMaster.Area;
            beneficiaryViewModel.MojaNo = beneficiaryMaster.MojaNo;
            beneficiaryViewModel.KhataNo = beneficiaryMaster.KhataNo;
            beneficiaryViewModel.KhasraNo = beneficiaryMaster.KhasraNo;
            beneficiaryViewModel.PlotNo = beneficiaryMaster.PlotNo;
            beneficiaryViewModel.PoliceStation = beneficiaryMaster.PoliceStation;
            beneficiaryViewModel.WardNo = beneficiaryMaster.WardNo;
            beneficiaryViewModel.District = beneficiaryMaster.District;
            beneficiaryViewModel.BankName = beneficiaryMaster.BankName;
            beneficiaryViewModel.BranchName = beneficiaryMaster.BranchName;
            beneficiaryViewModel.IFSCCode = beneficiaryMaster.IFSCCode;
            beneficiaryViewModel.AccountNo = beneficiaryMaster.AccountNo;
            beneficiaryViewModel.CreatedBy = beneficiaryMaster.CreatedBy;
            beneficiaryViewModel.CreatedDate = DateTime.Now;
            beneficiaryViewModel.CompanyID = beneficiaryMaster.CompanyID;
            beneficiaryViewModel.BeneficiaryId = beneficiaryMaster.BeneficiaryId;
            return beneficiaryViewModel;
        }
    }
}