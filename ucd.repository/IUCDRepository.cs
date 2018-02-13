using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UCD.Model.V1;

namespace UCD.Repository
{
    public interface IUCDRepository
    {
        Task<object> UploadClaimTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool validated, string request);

        string GetClaimTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool logClaimRequest);

        List<SearchClaimResultClass> SearchClaims(string transacionID, string apiKey, string pirCode, string claimID, string includeNullClaim, int minTier, string requestJSON);


        List<ExceptionClass> GetOpenException(string ctpExceptionType, string pirCode, string claimID);
        void UpsertException(string transactionID, string ctpExceptionType, string pirCode, string claimID, int tier, string exceptionType, string rule, string description, string node, string element, int? index, string uniqueID, int? exceptionID, bool closed, DateTime createdDate, string category, bool isDroolException);

        List<SLAReferenceDataClass> GetSLAReferenceData();

        bool IsSiraRefNumberUnique(string pirCode, string claimID, string siraRefNumber);
        bool IsNomDefRefNumUnique(string pirCode, string claimId, string nomDefRefNum);

        bool ValidateNullClaimIndGrossAmount(string pirCode, string claimId, string nullClaimInd);

        bool ValidateNullClaimIndNetAmount(string pirCode, string claimId, string nullClaimInd);

        bool IsAccidentEventNumUnique(string pirCode, string accidentID, string eventNum);

        bool IsTransactionNumberUnique(string pirCode, string transactionID);

        bool IsAPIKeyInsurerCodeValid(string apiKey, string pirCode);

        bool IsOriginalTransactionIDExists(string pirCode, string claimID, string originalTransactionID);

        void UploadPaymentTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool validated, string request);

        string GetPaymentTransaction(string apiKey, string pirCode, string transactionID, string claimID);

        void WriteClaimTransactionAsync(string transactionID, string pirCode);

        void WritePaymentTransactionAsync(string transactionID, string pirCode);

        void CloseExceptionbySuppression(string transactionID, string pirCode, int exceptionID);
    }
}
