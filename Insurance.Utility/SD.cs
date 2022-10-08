using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Insurance.Utility
{
    public static class SD
    {
        //public const string Proc_CoverType_Create = "usp_CreateCoverType";
        //public const string Proc_CoverType_Get = "usp_GetCoverType";
        //public const string Proc_CoverType_GetAll = "usp_GetCoverTypes";
        //public const string Proc_CoverType_Update = "usp_UpdateCoverType";
        //public const string Proc_CoverType_Delete = "usp_DeleteCoverType";


        //-----------------------------------------------------------------
        //--------------------------Roles----------------------------------
        public const string Role_Super_Admin = "Super Admin";
        public const string Role_Company_User = "Company User";
        public const string Role_Admin = "System Admin";
        public const string Role_Managers = "Managers";
        public const string Role_Quality_Control = "Quality Control";
        public const string Role_Schedulers = "Schedulers";
        public const string Role_Team_Leader = "Team Leader";
        public const string Role_Inspector = "Inspector";
        public const string Role_1099 = "1099";
        public const string Role_Enterprise = "Enterprise";
        public const string Role_Client = "Client";
        public const string Role_Guest = "Guest";
        //-----------------------------------------------------------------
        //-----------------------------------------------------------------
        //-----------------------------------------------------------------
        //--------------------------Claims Status----------------------------------

        //-------------------------------------------------------------------
        //-----------------New Status


        public const string Claim_Unassigned = "Unassigned";
        public const string Claim_Assigned = "Assigned";
        public const string Claim_InProgress = "In Progress";
        public const string Claim_QUALITYAS = "QUALITY AS.";
        public const string Claim_Report_Approved_by_QA = "Approved";
        public const string Claim_Report_Rejected_by_QA = "Denied";
        public const string Claim_Report_Approved_by_Client = "Accepted";
        public const string Claim_Report_Rejected_by_Client = "Rejected";
        public const string Claim_Completed = "Completed";

        //---------------------------------------------------------------
        //---------------------------------------------------------------
        //--------------------------OTP Status----------------------------------
        public const string OTP_Active = "A";
        public const string OTP_InActive = "I";
        public const string OTP_Utlize = "U";

        //------------------------------------------------------------------------
        //-----------------------FORM TYPE---------------------------------------
        public const string FT_Client = "Client";
        public const string FT_Inspector = "Pickup";
        public const string FT_Enterprise = "Enterprise";
        public const string FT_Scheduler = "Scheduler";
        public const string FT_TeamLeader = "Team Leader";
        public const string FT_Manager = "Manager";
        public const string FT_QualityAssurance = "Quality Assurance";
        //------------------------------------------------------------------------
        //-----------------------Device---------------------------------------
        public const string Dev_Web = "Web";
        public const string Dev_API = "Api";
        //-----------------------------------------------------------------

        //----------------------------------------------------------------
        //--------------Reportstatus
        public const string Report_Done = "Done";
        public const string FW_QA_Done = "QUALITY AS.";
        //---------------------Company Details
        public static string CompanyName { get; set; } = "";
        public static string CompanyAddress { get; set; } = "";
        public static string CompanyAddress2 { get; set; } = "";
        public static string Phone { get; set; } = "";
        public static string Email { get; set; } = "";

        //----------------------------

        public static List<SelectListItem> getHours = new List<SelectListItem>()
        {

            new SelectListItem() {Text="01", Value="01"},
            new SelectListItem() { Text="02", Value="02"},
            new SelectListItem() { Text="03", Value="03"},
            new SelectListItem() { Text="04", Value="04"},
            new SelectListItem() { Text="05", Value="05"},
            new SelectListItem() { Text="06", Value="06"},
            new SelectListItem() { Text="07", Value="07"},
            new SelectListItem() { Text="08", Value="08"},
            new SelectListItem() { Text="09", Value="09"},
            new SelectListItem() { Text="10", Value="10"},
            new SelectListItem() { Text="11", Value="11"},
            new SelectListItem() { Text="12", Value="12"},

        };


        public static string GetFormType(string Rolecms)
        {
            if (Rolecms == SD.Role_Client.ToUpper())
            {
                return FT_Client;
            } else if (Rolecms == SD.Role_Enterprise.ToUpper())
            {
                return FT_Enterprise;
            } else if (Rolecms == SD.Role_Quality_Control.ToUpper())
            {
                return FT_QualityAssurance;
            } else if (Rolecms == SD.Role_Schedulers.ToUpper())
            {
                return FT_Scheduler;
            } else if (Rolecms == SD.Role_Team_Leader.ToUpper())
            {
                return FT_TeamLeader;
            } else if (Rolecms == SD.Role_Managers.ToUpper())
            {
                return FT_Manager;
            } else if (Rolecms == SD.Role_Managers.ToUpper())
            {
                return FT_Manager;
            } else if (Rolecms == SD.Role_Inspector.ToUpper())
            {
                return FT_Inspector;
            }

            return "";
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }
    }

    public class vari
    {

        public static string HeaderName = "";
        public static string HeaderDescription = "";


    }
    public class Conntact
    {

        public static string Contact = "0888888888";
        public static string Email = "abc@abc.com";

       


    }



  

   
}
