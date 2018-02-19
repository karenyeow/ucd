using Comlib.Common.Framework.DataAccess;
using Comlib.Common.Helpers.Connections;
using Comlib.Common.Helpers.Extensions;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCD.Model.V1;

namespace UCD.Repository
{

    public class UCDRepositoryProvider : BaseDataAccessService, IUCDRepository
    {
       // private readonly string _glsOnlineConnection;
        private readonly JsonSerializerSettings _ignoreDateFormat = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };
        private readonly IUCDConnectionProvider _ucdConnectionProvider;

        public UCDRepositoryProvider(IUCDConnectionProvider ucdConnectionProvider)
        {
            this._ucdConnectionProvider = ucdConnectionProvider;
        }
        public void CloseExceptionbySuppression(string transactionID, string pirCode, int exceptionID)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@RequestTransactionId", transactionID);
                    param.Add("@ExceptionId", exceptionID);


                    connection.Execute("[ctp].[CloseExceptionbySuppression]", param, commandType: CommandType.StoredProcedure);
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
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@APIKey", apiKey);
                    param.Add("@RequestTransId", transactionID);
                    param.Add("@SourceID", claimID);
                    param.Add("@PIRCode", pirCode);
                    param.Add("@Log", logClaimRequest);
                    param.Add("@RequestJSON", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);


                    connection.Query("[api].[GetClaimTransactionsJSON]", param, commandType: CommandType.StoredProcedure);
                    var s = param.Get<string>("@RequestJSON");
                    return s;

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public List<ExceptionClass> GetOpenException(string ctpExceptionType, string pirCode, string claimID)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@ExceptionType", ctpExceptionType);
                    param.Add("@PirCode", pirCode);
                    param.Add("@SourceId", claimID);
                    return connection.Query<ExceptionClass>("[ctp].[GetExceptionRecord]", param, commandTimeout: 1000, commandType: CommandType.StoredProcedure).ToList();


                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public string GetPaymentTransaction(string apiKey, string pirCode, string transactionID, string claimID)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@APIKey", apiKey);
                    param.Add("@RequestTransId", transactionID);
                    param.Add("@SourceID", claimID);
                    param.Add("@PIRCode", pirCode);
                    param.Add("@RequestJSON", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                    connection.Query("[api].[GetPaymentTransactionsJSON]", param, commandType: CommandType.StoredProcedure);

                    var s = param.Get<string>("@RequestJSON");
                    return s;
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public List<SLAReferenceDataClass> GetSLAReferenceData()
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {

                    return connection.Query<SLAReferenceDataClass>("[ctp].[GetSLAReferenceData]", null, commandTimeout: 1000, commandType: CommandType.StoredProcedure).ToList();


                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public bool IsAccidentEventNumUnique(string pirCode, string accidentID, string eventNum)
        {
            SqlConnection connection = null;
            try
            {
                using (connection =this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@accidentID", accidentID);
                    param.Add("@eventNum", eventNum);
                    param.Add("@IsUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);


                    connection.Query("[ctp].[IsEventNumUnique]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsUnique");
                    return isUnqiue;
     
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public bool IsAPIKeyInsurerCodeValid(string apiKey, string pirCode)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@InsurerCode", pirCode);
                    param.Add("@ApiKey", apiKey);

                    param.Add("@Validate", dbType: DbType.Boolean, direction: ParameterDirection.Output);



                    connection.Query("[ctp].[ValidateAPIKey]", param, commandType: CommandType.StoredProcedure);


                    var test = param.Get<Boolean>("@Validate");

                    return test;

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }

        }

        public bool IsNomDefRefNumUnique(string pirCode, string claimId, string nomDefRefNum)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@PirCode", pirCode);
                    param.Add("@NomDefRefNum", nomDefRefNum);
                    param.Add("@ClaimId", claimId);
                    param.Add("@IsUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    connection.Query("[ctp].[IsNomDefRefNumUnique]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsUnique");
                    return isUnqiue;

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }

        }

        public bool IsOriginalTransactionIDExists(string pirCode, string claimID, string originalTransactionID)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@originalTransactionID", originalTransactionID);
                    param.Add("@claimId", claimID);
                    param.Add("@IsExists", dbType: DbType.Boolean, direction: ParameterDirection.Output);


                    //using (var reader = connection.QueryMultiple("[ctp].[IsSiraRefNumUnique]", param, commandType: CommandType.StoredProcedure))
                    //{
                    //    reader.Read();
                    //}

                    connection.Query("[ctp].[IsOriginalTransactionIDExists]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsExists");
                    return isUnqiue;
                    // return isUnqiue.HasValue?isUnqiue.Value: false;
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public bool IsSiraRefNumberUnique(string pirCode, string claimID, string siraRefNumber)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@ClaimId", claimID);
                    param.Add("@SiraRefNum", siraRefNumber);
                    param.Add("@IsUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);


                    //using (var reader = connection.QueryMultiple("[ctp].[IsSiraRefNumUnique]", param, commandType: CommandType.StoredProcedure))
                    //{
                    //    reader.Read();
                    //}

                    connection.Query("[ctp].[IsSiraRefNumUnique]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsUnique");
                    return isUnqiue;
                    // return isUnqiue.HasValue?isUnqiue.Value: false;
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }

        }

        public bool IsTransactionNumberUnique(string pirCode, string transactionID)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@TransactionId", transactionID);
                    param.Add("@IsUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);


                    //using (var reader = connection.QueryMultiple("[ctp].[IsSiraRefNumUnique]", param, commandType: CommandType.StoredProcedure))
                    //{
                    //    reader.Read();
                    //}

                    connection.Query("[ctp].[IsTransactionIdUnique]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsUnique");
                    return isUnqiue;
                    // return isUnqiue.HasValue?isUnqiue.Value: false;
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public List<SearchClaimResultClass> SearchClaims(string transacionID, string apiKey, string pirCode, string claimID, string includeNullClaim, int minTier, string requestJSON)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@RequestTransId", transacionID);
                    param.Add("@APIKey", apiKey);
                    param.Add("@PIRCode", pirCode);
                    param.Add("@SourceID", claimID);
                    param.Add("@IncludeNullClaim", includeNullClaim);
                    param.Add("@MinTier", minTier);
                    param.Add("@RequestJSON", requestJSON);
                    return connection.Query<SearchClaimResultClass>("[api].[SearchClaim]", param, commandTimeout: 1000, commandType: CommandType.StoredProcedure).ToList();


                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }

        }

        public async Task<object> UploadClaimTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool validated, string request)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@APIKey", apiKey);
                    param.Add("@RequestTransId", transactionID);
                    param.Add("@SourceID", claimID);
                    param.Add("@ValidationPass", validated);
                    param.Add("@RequestJSON", request);
                    param.Add("@PIRCode", pirCode);

                    using (var result = await connection.QueryMultipleAsync("[api].[UploadClaimTransactions]", param, commandTimeout: 1000, commandType: CommandType.StoredProcedure))
                    {
                        var errors = await result.ReadAsync<string>();

                        return new
                        {
                            errors
                        };
                    }
                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public void UploadPaymentTransaction(string apiKey, string pirCode, string transactionID, string claimID, bool validated, string request)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@APIKey", apiKey);
                    param.Add("@RequestTransId", transactionID);
                    param.Add("@SourceID", claimID);
                    param.Add("@ValidationPass", validated);
                    param.Add("@RequestJSON", request);
                    param.Add("@PIRCode", pirCode);

                    connection.Execute("[api].[UploadPaymentTransactions]", param, commandTimeout: 1000, commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }

        }

        public void UpsertException(string transactionID, string ctpExceptionType, string pirCode, string claimID, int tier, string exceptionType, string rule, string description, string node, string element, int? index, string uniqueID, int? exceptionID, bool closed, DateTime createdDate, string category, bool isDroolException)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@RequestTransactionId", transactionID);
                    param.Add("@ExceptionType", ctpExceptionType);
                    param.Add("@PirCode", pirCode);
                    param.Add("@SourceId", claimID);
                    param.Add("@Tier", tier);
                    param.Add("@Type", exceptionType);
                    param.Add("@Rule", rule);
                    param.Add("@Description", description);

                    param.Add("@RaisedDate", createdDate);
                    param.Add("@Category", category);
                    param.Add("@Closed", closed);

                    if (node.IsNullOrEmptyAfterTrim())
                        param.Add("@Node", null);
                    else
                        param.Add("@Node", node);
                    if (element.IsNullOrEmptyAfterTrim())
                        param.Add("@Element", null);
                    else
                        param.Add("@Element", element);
                    if (index.HasValue)
                        param.Add("@Index", index.Value);
                    else
                        param.Add("@Index", null);

                    if (exceptionID.HasValue)
                        param.Add("@ExceptionId", exceptionID.Value);
                    else
                        param.Add("@ExceptionId", 0);

                    if (uniqueID.IsNullOrEmptyAfterTrim())
                    {
                        param.Add("@UniqueId", null);
                    }
                    else
                    {
                        param.Add("@UniqueId", uniqueID);
                    }

                    param.Add("@IsDrools", isDroolException);
                    //  if (closed) param.Add("@ClosedDate", DateTime.Now);

                    connection.Execute("[ctp].[WriteExceptionRecord]", param: param, commandType: CommandType.StoredProcedure);


                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public bool ValidateNullClaimIndGrossAmount(string pirCode, string claimId, string nullClaimInd)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@ClaimId", claimId);

                    param.Add("@NullClaimInd", nullClaimInd);
                    param.Add("@IsTrue", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    connection.Query("[ctp].[ValidateNullClaimIndGrossAmount]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsTrue");
                    return isUnqiue;

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public bool ValidateNullClaimIndNetAmount(string pirCode, string claimId, string nullClaimInd)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();
                    param.Add("@PirCode", pirCode);
                    param.Add("@ClaimId", claimId);

                    param.Add("@NullClaimInd", nullClaimInd);
                    param.Add("@IsTrue", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    connection.Query("[ctp].[ValidateNullClaimIndNetAmount]", param, commandType: CommandType.StoredProcedure);


                    var isUnqiue = param.Get<Boolean>("@IsTrue");
                    return isUnqiue;

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }

        }

        public void WriteClaimTransactionAsync(string transactionID, string pirCode)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@RequestTransId", transactionID);
                    param.Add("@PIRCode", pirCode);

                    connection.Execute("[ctp].[WriteClaimTransactionsAsync]", param, commandTimeout: 1000, commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }

        public void WritePaymentTransactionAsync(string transactionID, string pirCode)
        {
            SqlConnection connection = null;
            try
            {
                using (connection = this._ucdConnectionProvider.CreateSqlConnection())
                {
                    var param = new DynamicParameters();

                    param.Add("@RequestTransId", transactionID);
                    param.Add("@PIRCode", pirCode);

                    connection.Execute("[ctp].[WritePaymentTransactionsAsync]", param, commandTimeout: 1000, commandType: CommandType.StoredProcedure);

                }

            }
            catch (Exception e)
            {
                connection.CloseAndDispose();
                throw e;
            }
        }
    }
}
