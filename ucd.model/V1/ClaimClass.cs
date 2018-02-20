using Comlib.Common.Framework.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UCD.Model.Base;
using static Comlib.Common.Framework.Attributes.ValidDateFormat;

namespace UCD.Model.V1
{
    /// <summary>
    /// 
    /// </summary>
    public class ClaimClass: BaseSiraClass
    {
        [Display(Name = @"claim\claimType")]
        [ValidLengthLimit(50)]
        public string claimType { get; set; }

        [Display(Name = @"claim\claimantEmail")]
        [ValidLengthLimit(150)]
        public string claimantEmail { get; set; }

        [Display(Name = @"claim\claimantLang")]
        [ValidLengthLimit(50)]
        public string claimantLang { get; set; }

        [Display(Name = @"claim\compToRelativesInd")]
        [ValidLengthLimit(50)]
        public string compToRelativesInd { get; set; }

        [Display(Name = @"claim\ceaseWorkDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string ceaseWorkDate { get; set; }

        [Display(Name = @"claim\sysEnteredDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string sysEnteredDate { get; set; }

        [Display(Name = @"claim\finalisedDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string finalisedDate { get; set; }

        [Display(Name = @"claim\injuryCodeReviewDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string injuryCodeReviewDate { get; set; }


        [Display(Name = @"claim\claimReviewDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string claimReviewDate { get; set; }


        [Display(Name = @"claim\deathDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string deathDate { get; set; }

        [Display(Name = @"claim\earlyNotificationDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string earlyNotificationDate { get; set; }

        [Display(Name = @"claim\recoveryPlanReviewDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string recoveryPlanReviewDate { get; set; }

        //public string liabilityStatusDate { get; set; }

        [Display(Name = @"claim\hospitalStayDays")]
        [ValidLengthLimit(10)]
        [EnsureDouble]
        public string hospitalStayDays { get; set; }

        [Display(Name = @"claim\drugAlcoholInd")]
        [ValidLengthLimit(50)]
        public string drugAlcoholInd { get; set; }

        [Display(Name = @"claim\preAccAverageWeeklyEarnings")]
        [ValidPrecision (10,2)]
        public string preAccAverageWeeklyEarnings { get; set; }

        [Display(Name = @"claim\fatality")]
        [ValidLengthLimit(50)]
        public string fatality { get; set; }

        [Display(Name = @"claim\faultStatusCode")]
        [ValidLengthLimit(50)]
        public string faultStatusCode { get; set; }

        [Display(Name = @"claim\faultStatusDate")]
        [ValidDateFormat(ValidateModeEnum.Date, "yyyyMMdd")]
        public string faultStatusDate { get; set; }

        [Display(Name = @"claim\finalisationCode")]
        [ValidLengthLimit(50)]
        public string finalisationCode { get; set; }
        [Display(Name = @"claim\hospitalName")]
        [ValidLengthLimit(200)]
        public string hospitalName  { get; set; }

        [Display(Name = @"claim\hospitalStatusCode")]
        [ValidLengthLimit(50)]
        public string hospitalStatusCode { get; set; }

        [Display(Name = @"claim\injuryStatusCode")]
        [ValidLengthLimit(50)]
        public string injuryStatusCode { get; set; }

        [Display(Name = @"claim\interpreterReqInd")]
        [ValidLengthLimit(50)]
        public string interpreterReqInd { get; set; }

        [Display(Name = @"claim\interstateInd")]
        [ValidLengthLimit(50)]
        public string interstateInd       { get; set; }

        [Display(Name = @"claim\managInsBranch")]
        [ValidLengthLimit(50)]
        public string   managInsBranch { get; set; }

        [Display(Name = @"claim\claimID")]
        [Required]
        [ValidLengthLimit(50)]
        public string claimID { get; set; }


        [Display(Name = @"claim\nomDefPercentage")]
        [ValidPrecision(3, 4)]
        public string nomDefPercentage { get; set; }

        [Display(Name = @"claim\nomDefReason")]
        [ValidLengthLimit(50)]
        public string nomDefReason { get; set; }

        [Display(Name = @"claim\nomDefRefNum")]
        [ValidLengthLimit(50)]
        public string nomDefRefNum { get; set; }

        [Display(Name = @"claim\nullClaimInd")]
        [ValidLengthLimit(50)]
        public string nullClaimInd { get; set; }

        [Display(Name = @"claim\shareingVehiclesNum")]
        [ValidLengthLimit(2)]
        [EnsureDouble]
        public string  sharingVehiclesNum { get; set; }

        [Display(Name = @"claim\preAccWorkHour")]
        [ValidPrecision(3, 2)]
        public string  preAccWorkHour { get; set; }

        [Display(Name = @"claim\preAccWorkStatusCode")]
        [ValidLengthLimit(50)]
        public string preAccWorkStatusCode { get; set; }


        [Display(Name = @"claim\priorCTPClaimInd")]
        [ValidLengthLimit(50)]
        public string priorCTPClaimInd { get; set; }

        //[Display(Name = @"claim\statBDeclReasonCode")]
        //[ValidLength(1)]
        //public string statBDeclReasonCode { get; set; }

        [Display(Name = @"claim\recoveryPlanActionCode")]
        [ValidLengthLimit(50)]
        public string recoveryPlanActionCode { get; set; }

        [Display(Name = @"claim\ambulanceRoleCode")]
        [ValidLengthLimit(50)]
        public string ambulanceRoleCode { get; set; }

        [Display(Name = @"claim\seriousOffenceInd")]
        [ValidLengthLimit(50)]
        public string seriousOffenceInd        { get; set; }

        [Display(Name = @"claim\seriousOffenceType")]
        [ValidLengthLimit(50)]
        public string seriousOffenceType { get; set; }


        [Display(Name = @"claim\siraRefNum")]
        [ValidLengthLimit(50)]
        public string siraRefNum { get; set; }

        //public string liabilityStatusCode        { get; set; }

        //public string seriousOffenceType { get; set; }
        [Display(Name = @"claim\wcClaimID")]
        [ValidLengthLimit(50)]
        public string wcClaimID        { get; set; }

        [Display(Name = @"claim\helmetInd")]
        [ValidLengthLimit(50)]
        public string helmetInd        { get; set; }

        [Display(Name = @"claim\seatbeltInd")]
        [ValidLengthLimit(50)]
        public string seatbeltInd { get; set; }

        [Display(Name = @"claim\wcRecovInd")]
        [ValidLengthLimit(50)]
        public string wcRecovInd { get; set; }

        [Display(Name = @"claim\managingInsCode")]
        [ValidLengthLimit(50)]
        public string managingInsCode { get; set; }

        [Display(Name = @"claim\legalRepEmail")]
        [ValidLengthLimit(150)]
        public string legalRepEmail  { get; set; }

        [Display(Name = @"claim\legalRepFirstName")]
        [ValidLengthLimit(150)]
        public string legalRepFirstName        { get; set; }

        [Display(Name = @"claim\legalRepSurname")]
        [ValidLengthLimit(150)]
        public string legalRepSurname { get; set; }

        [Display(Name = @"claim\legalRepPhoneNum")]
        [ValidLengthLimit(50)]
        public string legalRepPhoneNum { get; set; }

        [Display(Name = @"claim\legalRepABN")]
        [ValidLengthLimit(11)]
        public string legalRepABN { get; set; }

        [Display(Name = @"claim\legalRepBranch")]
        [ValidLengthLimit(150)]
        public string legalRepBranch { get; set; }

        [Display(Name = @"claim\legalRepFirmName")]
        [ValidLengthLimit(150)]
        public string legalRepFirmName { get; set; }

        public AccidentClass accident { get; set; } 

        public ContribNegClass contribNeg { get; set; }

        public MinorInjuryClass  minorInjury { get; set; }

        public InjurySeverityClass injurySeverity { get; set; }

        public ReturnToWorkClass returnToWork { get; set; }

        public WPIClass  wpi { get; set; }

   

        public CommonLawClass commonLaw { get; set; }

        public LTCSClass ltcs { get; set; }

        public RiskScreeningClass riskScreening { get; set; }


        public List<InjuryClass> injury { get; set; }

        public StatutoryBenefitsClass statutoryBenefits { get; set; }

        public List<VehicleClass> vehicle { get; set; } = new List<VehicleClass>();


        public List<PersonClass> person { get; set; } = new List<PersonClass>();

        public List<CaseEstimateClass> caseEstimate { get; set; }

        public List<CertificateOfFitnessClass> certificateOfFitness        { get; set; }

        public List<EmploymentClass> employment        { get; set; }
        public List<SharingClass> sharing        { get; set; }

        public List<CommonLawSettlementClass> commonLawSettlement        { get; set; }


        public List<InternalReviewClass> internalReview        { get; set; }

        public List<RecoveryClass > recovery { get; set; }

        public List<EarningCapacityClass> earningCapacity { get; set; }




























    }
}