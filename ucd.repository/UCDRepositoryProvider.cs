using Comlib.Common.Framework.DataAccess;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using UCD.Model.V1;

namespace UCD.Repository
{

    public class UCDRepositoryProvider : BaseDataAccessService, IUCDRepository
    {
        private readonly string _glsOnlineConnection;
        private readonly JsonSerializerSettings _ignoreDateFormat = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };
        private readonly IUCDConnectionProvider _ucdConnectionProvider;

        public UCDRepositoryProvider(IUCDConnectionProvider ucdConnectionProvider)
        {

            this._glsOnlineConnection = ucdConnectionProvider.ConnectionString;
        }
        public void CloseExceptionbySuppression(string transactionID, string pirCode, int exceptionID)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(_glsOnlineConnection))
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@RequestTransactionId", transactionID);
                    param.Add("@ExceptionId", exceptionID);


                    connection.Execute("[ctp].[CloseExceptionbySuppression]", param, commandType: CommandType.StoredProcedure);



                    // return isUnqiue.HasValue?isUnqiue.Value: false;
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public string GetClaimTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool logClaimRequest)
        {
            throw new NotImplementedException();
        }

        public List<ExceptionClass> GetOpenException(string ctpExceptionType, string pirCode, string claimID)
        {
            throw new NotImplementedException();
        }

        public string GetPaymentTransaction(string apiKey, string pirCode, string transactionID, string claimID)
        {
            throw new NotImplementedException();
        }

        public List<SLAReferenceDataClass> GetSLAReferenceData()
        {
            throw new NotImplementedException();
        }

        public bool IsAccidentEventNumUnique(string pirCode, string accidentID, string eventNum)
        {
            throw new NotImplementedException();
        }

        public bool IsAPIKeyInsurerCodeValid(string apiKey, string pirCode)
        {
            throw new NotImplementedException();
        }

        public bool IsNomDefRefNumUnique(string pirCode, string claimId, string nomDefRefNum)
        {
            throw new NotImplementedException();
        }

        public bool IsOriginalTransactionIDExists(string pirCode, string claimID, string originalTransactionID)
        {
            throw new NotImplementedException();
        }

        public bool IsSiraRefNumberUnique(string pirCode, string claimID, string siraRefNumber)
        {
            throw new NotImplementedException();
        }

        public bool IsTransactionNumberUnique(string pirCode, string transactionID)
        {
            throw new NotImplementedException();
        }

        public List<SearchClaimResultClass> SearchClaims(string transacionID, string apiKey, string pirCode, string claimID, string includeNullClaim, int minTier, string requestJSON)
        {
            throw new NotImplementedException();
        }

        public Task<object> UploadClaimTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool validated, string request)
        {
            throw new NotImplementedException();
        }

        public void UploadPaymentTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool validated, string request)
        {
            throw new NotImplementedException();
        }

        public void UpsertException(string transactionID, string ctpExceptionType, string pirCode, string claimID, int tier, string exceptionType, string rule, string description, string node, string element, int? index, string uniqueID, int? exceptionID, bool closed, DateTime createdDate, string category, bool isDroolException)
        {
            throw new NotImplementedException();
        }

        public bool ValidateNullClaimIndGrossAmount(string pirCode, string claimId, string nullClaimInd)
        {
            throw new NotImplementedException();
        }

        public bool ValidateNullClaimIndNetAmount(string pirCode, string claimId, string nullClaimInd)
        {
            throw new NotImplementedException();
        }

        public void WriteClaimTransactionAsync(string transactionID, string pirCode)
        {
            throw new NotImplementedException();
        }

        public void WritePaymentTransactionAsync(string transactionID, string pirCode)
        {
            throw new NotImplementedException();
        }
    }
}
