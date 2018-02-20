using Comlib.Common.Helpers.Constants;
using Comlib.Common.Helpers.Email;
using Comlib.Common.Helpers.Extensions;
using iCare.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UCD.Model.Base;
using UCD.Model.Drools.V1;
using UCD.Model.V1;
using UCD.Repository;

namespace UCD.API.Controllers
{

    public class UCDAPIBaseController : AbstractApiController
    {
        protected readonly IUCDRepository _ucdRepository;
        public UCDAPIBaseController (IUCDRepository ucdRepository, IOneGovEmailSender onegovEmailSender): base(onegovEmailSender)
        {
            _ucdRepository = ucdRepository;
        }
    
        private const string CTPCLAIMEXCEPTIONTYPE = "CTPClaim";
        private const string CTPPAYMENTEXCEPTIONTYPE = "CTPPayment";
        private List<SLAReferenceDataClass> _SLAReferenceData;
        public List<SLAReferenceDataClass> SLAReferenceData
        {
            get
            {
                if (this._SLAReferenceData == null)
                {
                    this._SLAReferenceData = this._ucdRepository.GetSLAReferenceData();
                }
                return this._SLAReferenceData;

            }
        }


        private string _insurerCode;
        public string  InsurerCode
        {
            get
            {
                if (this._insurerCode == null)
                {
                    this._insurerCode = GetHeaderValues("insurerCode");
                }
                return this._insurerCode;

            }
        }

        private string _apiKey;
       public  string APIKey
        {
            get
            {
                if (this._apiKey.IsNullOrEmptyAfterTrim()) _apiKey = GetHeaderValues(APIHeaderConstants.ApiKeyHeaderKey);
                return this._apiKey;
            }
        }
        private string _transactionID;
        protected string TransactionID
        {
            get
            {
                if (this._transactionID.IsNullOrEmptyAfterTrim()) _transactionID = GetHeaderValues(APIHeaderConstants.TransactionIdHeaderKey);
                return this._transactionID;
            }
        }

        public virtual async Task<IActionResult> UploadCTPClaim()
        {
            try
            {
                SetHeaderValuesUTC();

                var pirCode = this.InsurerCode;

                var rawData = await Request.GetRawBodyStringAsync();


                var ctpClaim = JsonConvert.DeserializeObject<ClaimRequestClass>(rawData);
               var  claimID = ctpClaim.claim.claimID;

                ctpClaim.claim.managingInsCode = pirCode;
                //Get Proc  to retrieve existing proc
                var existingExceptions = this._ucdRepository.GetOpenException(CTPCLAIMEXCEPTIONTYPE, this.InsurerCode, claimID);

                this._ValidateTier0(this.TransactionID,claimID, ctpClaim, existingExceptions);

                var currentExceptions = this._ucdRepository.GetOpenException(CTPCLAIMEXCEPTIONTYPE,this.InsurerCode, claimID);

                bool passTier0 = false;
                if (!currentExceptions.Exists(x => x.tier == 0))
                {
                    this._ValidateOtherTiers(this.TransactionID,pirCode, claimID, ctpClaim, existingExceptions);
                    currentExceptions = this._ucdRepository.GetOpenException(CTPCLAIMEXCEPTIONTYPE,this.InsurerCode, claimID);
                    passTier0 = true;
                }

                var errors = this._ucdRepository.UploadClaimTransaction(this.APIKey, pirCode, this.TransactionID, claimID,
                passTier0, JsonConvert.SerializeObject(ctpClaim, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                var response = _CreateResponse(claimID, ctpClaim, currentExceptions);
 
                Task.Factory.StartNew(() => _UnpackClaimTransaction(TransactionID, pirCode));
                if (passTier0)
                    return new OkObjectResult(response);
                else
                    return new BadRequestObjectResult(response);
            }
            catch (Exception exception)
            {
                return await HandleException(exception);
            }

        }

        public virtual async Task<IActionResult> GetCTPClaim(string id)
        {
            try
            {
                SetHeaderValues();
                var pirCode = this.InsurerCode;
                var ctpClaim =  this._ucdRepository.GetClaimTransaction(this.APIKey, pirCode,this.TransactionID,
                    id,true);
                var response = new OkObjectResult(ctpClaim ?? string.Empty);


                return response;

            }
            catch (Exception exception)
            {
                return await HandleException(exception);
            }
        }

      public virtual async Task<IActionResult> UploadCTPPayments()
        {
            try
                          
            {

                SetHeaderValuesUTC();

                var pirCode = this.InsurerCode;

                var rawData =  await Request.GetRawBodyStringAsync();


                var ctpPaymentRequest = JsonConvert.DeserializeObject<PaymentRequestClass>(rawData);
                var claimID = ctpPaymentRequest.claimID;

           
                //Get Proc  to retrieve existing proc
                var existingExceptions = this._ucdRepository.GetOpenException(CTPPAYMENTEXCEPTIONTYPE ,this.InsurerCode, claimID);

                this._ValidateTier0(this.TransactionID,claimID, ctpPaymentRequest, existingExceptions);

                var ctpClaim = this._GetClaimRequest(claimID);
                if (ctpClaim == null)
                {
                    this._InsertClaimDoesNotExistError(this.TransactionID,CTPPAYMENTEXCEPTIONTYPE, claimID, existingExceptions,false);
                }
                else
                {
                    this._CloseExistingClaimDoesNotExistErrors(this.TransactionID,CTPPAYMENTEXCEPTIONTYPE, existingExceptions);
                }

                    var currentExceptions = this._ucdRepository.GetOpenException(CTPPAYMENTEXCEPTIONTYPE, this.InsurerCode, claimID);

                bool passTier0 = false;
                if (!currentExceptions.Exists(x => x.tier == 0))
                {
                    //TODO: Retrieve CTPClaim
                    this._ValidateOtherTiers(this.TransactionID,this.InsurerCode ,claimID, ctpPaymentRequest, ctpClaim,existingExceptions);
                    currentExceptions = this._ucdRepository.GetOpenException("", this.InsurerCode, claimID);
                    passTier0 = true;
                }

                this._ucdRepository.UploadPaymentTransaction(this.APIKey, pirCode,this.TransactionID, claimID,
                passTier0, JsonConvert.SerializeObject(ctpPaymentRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
 
                var response = _CreateResponse(claimID, ctpPaymentRequest, currentExceptions);


                Task.Factory.StartNew(() => _UnpackPaymentTransaction(TransactionID,pirCode ));

                if (passTier0)
                    return new OkObjectResult(response);
                else
                    return new BadRequestObjectResult(response);
            }
            catch (Exception exception)
            {
                return await  HandleException(exception);
            }
        }

        public virtual async Task<IActionResult> GetCTPPayment(string id)
        {
            try
            {
     
                SetHeaderValues();
                var pirCode = this.InsurerCode;
                var ctpPayments = this._ucdRepository.GetPaymentTransaction(this.APIKey, pirCode, this.TransactionID,
                    id);


                return ctpPayments == null ? new OkObjectResult("") : new OkObjectResult(ctpPayments);
               
            }
            catch (Exception exception)
            {
                return await HandleException(exception);
            }
        }

        public virtual async Task<IActionResult> SearchCTPClaims()
        {
            try
            {
                SetHeaderValuesUTC();

                var rawData = await Request.GetRawBodyStringAsync();


                var claimSearchRequest  = JsonConvert.DeserializeObject<ClaimSearchRequestClass>(rawData);


                var claimsResult = this._ucdRepository.SearchClaims(this.TransactionID, this.APIKey , this.InsurerCode, claimSearchRequest.claim.claimID, claimSearchRequest.searchScope.includeNullClaim, claimSearchRequest.searchScope.minTier ?? 0, JsonConvert.SerializeObject(claimSearchRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));


                var claimsSearchResult = new ClaimSearchResponseClass();
                var index = 1;
                foreach (var row in claimsResult)
                {
                    var claimSearchResultRow = new ClaimSearchResultResponseClass()
                    {
                        apiReceivedDateTime = row.receivedDateTime.ToString("yyyyMMddTHHmmss") + "Z",
                        providerSnapshotDateTime = row.providerSnapshotDateTime,
                         managingInsCode = row.managingInsCode ,
                          resultNum = index.ToString(),
                          claim = JsonConvert.DeserializeObject<ClaimRequestClass>( row.JSONText?? string.Empty).claim 

                    };
                    claimsSearchResult.results.Add(claimSearchResultRow);

                }

                return new OkObjectResult(claimsSearchResult);
         
        
            }
            catch (Exception exception)
            {
                return await HandleException(exception);
            }
        }



        public virtual async Task<IActionResult> ClearException()
        {
            try
            {
                SetHeaderValuesUTC();

                var rawData = await Request.GetRawBodyStringAsync();




                var clearExceptionRequest = JsonConvert.DeserializeObject<ClearExceptionRequestClass>(rawData);

                foreach (var exceptionID in clearExceptionRequest.exceptionID)
                {
                            
                    this._ucdRepository.CloseExceptionbySuppression(this.TransactionID, this.InsurerCode, exceptionID.ToInt());
                }


                return new OkObjectResult("");            }
            catch (Exception exception)
            {
                return  await HandleException(exception);
            }

        }

        private ClaimResponseClass _CreateResponse(string id, ClaimRequestClass request, List<ExceptionClass> openExceptions)
        {
            HttpStatusCode httpStatus;
            var response = new ClaimResponseClass()
            {
                claimID = id,
                providerProcessedDateTime = request.providerProcessedDateTime,
                submissionID = this.TransactionID,
                receivedDateTime = Response.Headers[APIHeaderConstants.ResponseTimeHeaderKey], 
                claim  = request.claim  
  

            };
      
            response.exception = new List<Model.Base.ResponseExceptionClass>();
            foreach (var openEx in openExceptions)
            {
                response.exception.Add(new Model.Base.ResponseExceptionClass()
                {
                    description = openEx.description,
                    exceptionID = openEx.sequenceID,
                    exceptionRaisedDateTime = openEx.exceptionRaisedDateTime.ToString("yyyyMMddTHHmmss") + "Z",
                    regulatoryRequirementDate = openEx.regulatoryRequirementDate.ToString("yyyyMMddTHHmmss") + "Z",
                    rule = openEx.rule,
                    tier = openEx.tier,
                    type = openEx.type,
                    exceptionReference = openEx.exceptionReference

                });
            }
            return response;;




        }

        private PaymentResponseClass _CreateResponse(string id, PaymentRequestClass  request, List<ExceptionClass> openExceptions)
        {

            var response = new PaymentResponseClass()
            {
                claimID = id,
                providerProcessedDateTime = request.providerProcessedDateTime,
                submissionID = this.TransactionID,
                receivedDateTime =Response.Headers[APIHeaderConstants.ResponseTimeHeaderKey], 
                payment = request.payment 

            };
 
            response.exception = new List<Model.Base.ResponseExceptionClass>();
            foreach (var openEx in openExceptions)
            {
                response.exception.Add(new Model.Base.ResponseExceptionClass()
                {
                    description = openEx.description,
                    exceptionID = openEx.sequenceID,
                    exceptionRaisedDateTime = openEx.exceptionRaisedDateTime.ToString("yyyyMMddTHHmmss") + "Z",
                    regulatoryRequirementDate = openEx.regulatoryRequirementDate.ToString("yyyyMMddTHHmmss") + "Z",
                    rule = openEx.rule,
                    tier = openEx.tier,
                    type = openEx.type,
                    exceptionReference = openEx.exceptionReference 

                });
            }

            return response;
        }
        private void _ValidateTier0(string transactionID,string claimID, ClaimRequestClass request, List<ExceptionClass> openExceptions)
        {
            List<Task> tasks = new List<Task>
            {
                Task.Factory.StartNew(() => this._ValidateTier0<ClaimRequestClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "", claimID, request, openExceptions))
            };
            if (request.claim != null)
            {

                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<VehicleClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "vehicle",claimID, request.claim.vehicle, openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<PersonClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "person",claimID, request.claim.person, openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<CaseEstimateClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "caseEstimate", claimID, request.claim.caseEstimate , openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<CertificateOfFitnessClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "certificateOfFitness", claimID, request.claim.certificateOfFitness , openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<EmploymentClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "employment", claimID, request.claim.employment, openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<SharingClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "sharing", claimID, request.claim.sharing, openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<CommonLawSettlementClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "commonLawSettlement", claimID, request.claim.commonLawSettlement, openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<InternalReviewClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "internalReview", claimID, request.claim.internalReview , openExceptions)));
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<RecoveryClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "recovery", claimID, request.claim.recovery, openExceptions)));

                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<ClaimClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "claim", claimID, request.claim, openExceptions)));

                if (request.claim.accident != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<AccidentClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "accident", claimID, request.claim.accident, openExceptions)));
                if (request.claim.contribNeg != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<ContribNegClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "contribNeg", claimID, request.claim.contribNeg, openExceptions)));
                if (request.claim.injury != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<InjuryClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "injury", claimID, request.claim.injury, openExceptions)));
                if (request.claim.injurySeverity != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<InjurySeverityClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "injurySeverity", claimID, request.claim.injurySeverity, openExceptions)));
                if (request.claim.minorInjury != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<MinorInjuryClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "minorInjury", claimID, request.claim.minorInjury, openExceptions)));
                if (request.claim.statutoryBenefits  != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<StatutoryBenefitsClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "statutoryBenefits", claimID, request.claim.statutoryBenefits, openExceptions)));
                if (request.claim.returnToWork != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<ReturnToWorkClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "returnToWork", claimID, request.claim.returnToWork , openExceptions)));
                if (request.claim.wpi != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<WPIClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "wpi", claimID, request.claim.wpi, openExceptions)));
                if (request.claim.earningCapacity   != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<EarningCapacityClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "earningCapacity", claimID, request.claim.earningCapacity, openExceptions)));

                if (request.claim.commonLaw != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<CommonLawClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "commonLaw", claimID, request.claim.commonLaw, openExceptions)));
                if (request.claim.ltcs != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<LTCSClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "ltcs", claimID, request.claim.ltcs, openExceptions)));
                if (request.claim.riskScreening != null) tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<RiskScreeningClass>(transactionID, CTPCLAIMEXCEPTIONTYPE, "riskScreening", claimID, request.claim.riskScreening, openExceptions)));
            }



            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                throw ae;
            }
        }



        private void _ValidateTier0(string transactionID, string claimID, PaymentRequestClass  request, List<ExceptionClass> openExceptions)
        {

            List<Task> tasks = new List<Task>
            {
                Task.Factory.StartNew(() => this._ValidateTier0<PaymentRequestClass>(transactionID, CTPPAYMENTEXCEPTIONTYPE, "", claimID, request, openExceptions))
            };
            if (request.payment!= null)
            {
                tasks.Add(Task.Factory.StartNew(() => this._ValidateTier0<PaymentClass>(transactionID, CTPPAYMENTEXCEPTIONTYPE, "payment", claimID, request.payment, openExceptions)));
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                throw ae;
            }
        }
            private void _ValidateTier0<T>(string transactionID, string ctpExceptionType,string dataName, string claimID, T data, List<ExceptionClass> openExceptions) where T : BaseSiraClass
        {
            try {

                var dataerrors = new List<ValidationResult>();
                var isValid = true;
                List<Task> tasks = new List<Task>();
                isValid = Validator.TryValidateObject(data, new ValidationContext(data), dataerrors, true);

                if (!isValid)
                {

                    this._InsertNewErrors(transactionID, ctpExceptionType, claimID, dataName, dataerrors, openExceptions, "General");
                }


                _CloseExistingErrors(transactionID, dataName, dataerrors, openExceptions);
            }
            catch (Exception exception)
            {
              
                throw exception;
            }



        }

        private void _ValidateTier0<T>(string transactionID,string ctpExceptionType, string dataName,string claimID, List<T> listOfData, List<ExceptionClass> openExceptions) where T: BaseSiraClass
        {
            try {
                if (listOfData == null) return;
                var newDataErrors = new List<ValidationResult>();
                for (int i = 0; i < listOfData.Count; i++)
                {
                    var dataErrors = new List<ValidationResult>();
                    var isValid = Validator.TryValidateObject(listOfData[i], new ValidationContext(listOfData[i]), dataErrors, true);
                    if (!isValid)
                    {

                        foreach (var dataError in dataErrors)
                        {
                            if (newDataErrors.Exists(x => x.MemberNames.First() == dataError.MemberNames.First() && x.ErrorMessage == dataError.ErrorMessage)) continue;
                            newDataErrors.Add(dataError);
                        }

                    }

                }
                this._InsertNewErrors(transactionID,ctpExceptionType, claimID, dataName, newDataErrors, openExceptions,"General");
                this._CloseExistingErrors(transactionID, dataName, newDataErrors, openExceptions);
            }
            catch (Exception exception)
            {
                //HandleException(exception);
                throw exception;
            }
       
        }
    

        private void _InsertNewErrors(string transactionID, string ctpExceptionType,string claimID, string nodeName, List<ValidationResult> newErrors, List<ExceptionClass> existingExceptions, string category )
        {

            foreach (var newError in newErrors)
            {
                string elementName = newError.MemberNames.First();
                nodeName = nodeName.IsNullOrEmptyAfterTrim() ? null : nodeName;
                if (!(existingExceptions.Exists(x => x.tier == 0 && x.node == nodeName && x.element == elementName && x.description == newError.ErrorMessage)))
                {
                    this._ucdRepository.UpsertException(transactionID, ctpExceptionType, this.InsurerCode, claimID, 0, "Error", string.Empty,
                        newError.ErrorMessage, nodeName, elementName, null, null, null, false, DateTime.UtcNow, category, false);
                }

            }

        }

        private void _InsertClaimDoesNotExistError(string transactionID,string ctpExceptionType, string claimID, List<ExceptionClass> existingExceptions, bool isDrools)
        {
            if (!(existingExceptions.Exists(x => x.tier == 0 && x.description == "Claim does not exist" && x.exceptionType == ctpExceptionType)))
            {
                this._ucdRepository.UpsertException(transactionID, ctpExceptionType, this.InsurerCode, claimID, 0, "Error", string.Empty,
                    "Claim does not exist","    ", "     ", null, null, null, false, DateTime.UtcNow, "General", isDrools);
            }
           

        }




        private void _InsertNewErrors(string transactionID,string ctpExceptionType,string claimID, List<BaseResponseValueClass> newErrors, List<ExceptionClass> existingExceptions, bool isDrools)
        {
            foreach (var newError in newErrors)
            {
              
                if (!(existingExceptions.Exists(x => x.rule == newError.ResponseException.Rule && ((newError.ResponseException.exceptionReference.IsNullOrEmptyAfterTrim() && x.exceptionReference.IsNullOrEmptyAfterTrim()) || !newError.ResponseException.exceptionReference.IsNullOrEmptyAfterTrim() && x.exceptionReference==newError.ResponseException.exceptionReference))))
                {


                    this._ucdRepository.UpsertException(transactionID, ctpExceptionType, this.InsurerCode, claimID, newError.ResponseException.Tier, newError.ResponseException.Type, newError.ResponseException.Rule,
                        newError.ResponseException.ShortDescription, null, null, null, newError.ResponseException.exceptionReference, null, false, DateTime.UtcNow, newError.ResponseException.SLACategory, isDrools);
                }

            }

        }



        private void _InsertNewErrors(string transactionID, string ctpExceptionType,string claimID, ClaimResponseValueClass newError, List<ExceptionClass> existingExceptions, bool isDrools)
        {


                if (!(existingExceptions.Exists(x => x.rule == newError.ResponseException.Rule)))
                {
                    this._ucdRepository.UpsertException(transactionID, ctpExceptionType, this.InsurerCode, claimID, newError.ResponseException.Tier, newError.ResponseException.Type, newError.ResponseException.Rule,
                        newError.ResponseException.ShortDescription, null, null, null,newError.ResponseException.exceptionReference,null, false, DateTime.UtcNow, newError.ResponseException.SLACategory, isDrools);
                }

            

        }
        private void _CloseExistingErrors(string transactionID,string nodeName, List<ValidationResult> newErrors, List<ExceptionClass> existingExceptions)
        {
            nodeName = nodeName.IsNullOrEmptyAfterTrim() ? null : nodeName;
            var existingNodeErrors = existingExceptions.Where(x => x.tier == 0 && x.node == nodeName && x.description != "Claim does not exist").ToList();
            if (existingNodeErrors.Count == 0) return;
            foreach (var openError in existingNodeErrors)
            {
                if (newErrors.Exists(x => x.MemberNames.First() == openError.element && x.ErrorMessage == openError.description)) continue;
                
                this._ucdRepository.UpsertException(transactionID, openError.exceptionType, this.InsurerCode, openError.sourceID, 0, openError.type, openError.rule, openError.description, nodeName, openError.element, null,openError.exceptionReference,
                    openError.sequenceID, true, openError.exceptionRaisedDateTime, openError.categorySLA, false);
            }

        }
        private void _CloseExistingErrors(string transactionID,string nodeName, int index, List<ValidationResult> newErrors, List<ExceptionClass> existingExceptions)
        {
            nodeName = nodeName.IsNullOrEmptyAfterTrim() ? null : nodeName;
            var existingNodeErrors = existingExceptions.Where(x => x.tier == 0 && x.node == nodeName && x.index == index).ToList();
            if (existingNodeErrors.Count == 0) return;
            foreach (var openError in existingNodeErrors)
            {
                if (newErrors.Exists(x => x.MemberNames.First() == openError.element && openError.description.Contains(x.ErrorMessage))) continue;
                this._ucdRepository.UpsertException(transactionID, openError.exceptionType, this.InsurerCode, openError.sourceID, 0, openError.type, openError.rule, openError.description, nodeName, openError.element, index,openError.exceptionReference,
                    openError.sequenceID, true, openError.exceptionRaisedDateTime, openError.categorySLA, openError.isDrools);
            }

        }
        private void _CloseExistingErrors(string transactionID,List<ExceptionClass> existingExceptions, bool isDrools)
        {
            var existingNodeErrors = existingExceptions.Where(x => x.tier > 0 && x.isDrools== isDrools ).ToList();
            if (existingNodeErrors.Count == 0) return;
            foreach (var openError in existingNodeErrors)
            {

                    this._ucdRepository.UpsertException(transactionID, openError.exceptionType, this.InsurerCode, openError.sourceID, openError.tier, openError.type, openError.rule, openError.description, openError.node, openError.element, null, openError.exceptionReference,
                        openError.sequenceID, true, openError.exceptionRaisedDateTime, openError.categorySLA , isDrools);
                

            }

        }
        private void _CloseExistingErrors(string transactionID, List<ExceptionClass> existingExceptions, int index, bool isDrools)
        {

                var existingNodeErrors = existingExceptions.Where(x => x.tier > 0 && x.index == index && x.isDrools == isDrools).ToList();
                if (existingNodeErrors.Count == 0) return;
                foreach (var openError in existingNodeErrors)
                {

                    this._ucdRepository.UpsertException(transactionID, openError.exceptionType, this.InsurerCode, openError.sourceID, openError.tier, openError.type, openError.rule, openError.description, openError.node, openError.element, openError.index, openError.exceptionReference,
                        openError.sequenceID, true, openError.exceptionRaisedDateTime, openError.categorySLA , openError.isDrools);
                }





        }
        private void _CloseExistingErrors(string transactionID, string claimID, List<BaseResponseValueClass> newErrors, List<ExceptionClass> existingExceptions, bool isDrools)
        {

            var existingNodeErrors = existingExceptions.Where(x => x.tier > 0 && x.isDrools == isDrools).ToList();
            if (existingNodeErrors.Count == 0) return;
            foreach (var openError in existingNodeErrors)
            {
                if (newErrors.Exists(x => x.ResponseException.Rule == openError.rule && ((openError.exceptionReference.IsNullOrEmptyAfterTrim() && x.ResponseException.exceptionReference.IsNullOrEmptyAfterTrim()) || !openError.exceptionReference.IsNullOrEmptyAfterTrim() && openError.exceptionReference == x.ResponseException.exceptionReference))) continue;
                this._ucdRepository.UpsertException(transactionID, openError.exceptionType, this.InsurerCode, openError.sourceID, 0, openError.type, openError.rule, openError.description,null, null, null, openError.exceptionReference,
                    openError.sequenceID, true, openError.exceptionRaisedDateTime, openError.categorySLA, openError.isDrools);

            }
            


        }

        private void _CloseExistingErrors(string transactionID, string claimID,int index, List<BaseResponseValueClass> newErrors, List<ExceptionClass> existingExceptions, bool isDrools)
        {
            var existingNodeErrors = existingExceptions.Where(x => x.tier > 0 &&  x.index == index &&   x.isDrools == isDrools).ToList();
            if (existingNodeErrors.Count == 0) return;
            foreach (var openError in existingNodeErrors)
            {
                if (newErrors.Exists(x => x.ResponseException.Rule == openError.rule)) continue;
                this._ucdRepository.UpsertException(transactionID, openError.exceptionType, this.InsurerCode, openError.sourceID, 0, openError.type, openError.rule, openError.description, null, null, null,openError.exceptionReference,
                    openError.sequenceID, true, openError.exceptionRaisedDateTime, openError.categorySLA, openError.isDrools);
            }

        }

        private void _CloseExistingErrors(string transactionID,string claimID, int tier, string ruleID, List<ExceptionClass> existingExceptions, bool isDrools)
        {
            var existingRuleErrors = existingExceptions.Where(x => x.tier == tier && x.rule == ruleID && x.isDrools == isDrools).ToList();
            if (existingRuleErrors.Count == 0) return;
            foreach ( var existingRuleError in existingRuleErrors)
            {
                this._ucdRepository.UpsertException(transactionID, existingRuleError.exceptionType, this.InsurerCode, existingRuleError.sourceID, 0, existingRuleError.type, existingRuleError.rule, existingRuleError.description, null, null, null,existingRuleError.exceptionReference,
existingRuleError.sequenceID, true, existingRuleError.exceptionRaisedDateTime, existingRuleError.categorySLA, existingRuleError.isDrools);
            }



        }
        private void _CloseExistingClaimDoesNotExistErrors(string transactionID, string ctpExceptionType, List<ExceptionClass> existingExceptions)
        {
            var existingRuleErrors = existingExceptions.Where(x => x.tier == 0 && x.exceptionType == ctpExceptionType && x.description == "Claim does not exist").ToList();
            if (existingRuleErrors.Count == 0 ) return;
            foreach (var existingRuleError in existingRuleErrors)
            {
                this._ucdRepository.UpsertException(transactionID, existingRuleError.exceptionType, this.InsurerCode, existingRuleError.sourceID, 0, existingRuleError.type, existingRuleError.rule, existingRuleError.description, null, null, null,existingRuleError.exceptionReference,
        existingRuleError.sequenceID, true, existingRuleError.exceptionRaisedDateTime, existingRuleError.categorySLA , existingRuleError.isDrools);
            }
        }
        private void _ValidateOtherTiers(string transactionID,string pirCode,string claimID, ClaimRequestClass request, List<ExceptionClass> openExceptions)
        {
            List<Task> tasks = new List<Task>
            {
                Task.Factory.StartNew(() => this._ValidateWithDrools(transactionID, claimID, request, openExceptions)),
                Task.Factory.StartNew(() => this._ValidateIsSiraRefNumUnique(transactionID, pirCode, claimID, request, openExceptions)),
                Task.Factory.StartNew(() => this._ValidateIsNomDefRefNumUnique(transactionID, claimID, pirCode, request.claim.nomDefRefNum, openExceptions)),
                Task.Factory.StartNew(() => this._ValidateNullClaimIndEqualsYGrossAmountEquals0(transactionID, claimID, pirCode, request.claim.nullClaimInd, openExceptions)),
                Task.Factory.StartNew(() => this._ValidateNullClaimIndEqualsYNetAmountEquals0(transactionID, claimID, pirCode, request.claim.nullClaimInd, openExceptions)),
                Task.Factory.StartNew(() => this._ValidateIsAccidentEventNumUnique(transactionID, claimID, pirCode, request, openExceptions))
            };

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                var d =  ae.Flatten();
                throw d;
            }

        }

        private void _ValidateOtherTiers(string transactionID, string pirCode, string claimID, PaymentRequestClass paymentRequest, ClaimRequestClass claimRequest, List<ExceptionClass> openExceptions)
        {
            List<Task> tasks = new List<Task>
            {
                Task.Factory.StartNew(() => this._ValidateWithDrools(transactionID, claimID, paymentRequest, claimRequest, openExceptions)),
                Task.Factory.StartNew(() => this._ValidateOrigTranIdExists(transactionID, claimID, pirCode, paymentRequest, openExceptions))
            };
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                throw ae;
            }
        }

        private void _ValidateIsSiraRefNumUnique(string transactionID,string pirCode, string claimID, ClaimRequestClass request, List<ExceptionClass> openExceptions)
        {
            try {
             

                if (request.claim.siraRefNum.IsNullOrEmptyAfterTrim()) return;


                if (this._ucdRepository.IsSiraRefNumberUnique(pirCode, claimID, request.claim.siraRefNum))
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "90.1", openExceptions, false);
                }
                else
                {
                    ClaimResponseValueClass newError = new ClaimResponseValueClass { ResponseException = new Model.Drools.V1.ResponseExceptionClass() { Rule = "90.1", ShortDescription = "SiraRefNum must be unique for a claim", SLACategory = "IntegrityChecksOnAllNonBlankFields", Tier = 1, Type = "Warning" } };
                    this._InsertNewErrors(transactionID, CTPCLAIMEXCEPTIONTYPE, claimID, newError, openExceptions, false);
                }

            }
            catch (Exception exception)
            {
                //HandleException(exception);
                throw exception;
            }



        }
        private void _ValidateIsNomDefRefNumUnique(string transactionID,string claimID, string pirCode, string nomDefRefNum, List<ExceptionClass> openExceptions)
        {
            try {

                if (nomDefRefNum.IsNullOrEmptyAfterTrim()) return;


                if (this._ucdRepository.IsNomDefRefNumUnique(pirCode, claimID , nomDefRefNum))
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "137.2", openExceptions, false);
                }
                else
                {
                    ClaimResponseValueClass newError = new ClaimResponseValueClass { ResponseException = new Model.Drools.V1.ResponseExceptionClass() { Rule = "137.2", ShortDescription = "NomDefRefNum must be unique", SLACategory = "IntegrityChecksOnAllNonBlankFields", Tier = 1, Type = "Warning" } };
                    this._InsertNewErrors(transactionID, CTPCLAIMEXCEPTIONTYPE, claimID, newError, openExceptions, false);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw;
            }


        }

        private void _ValidateNullClaimIndEqualsYGrossAmountEquals0(string transactionID,string claimID, string pirCode, string nullClaimInd, List<ExceptionClass> openExceptions)
        {
            try {

                if (nullClaimInd.IsNullOrEmptyAfterTrim() || nullClaimInd.ToUpper() != "Y")
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "298.4", openExceptions, false);
                    return;
                }

                if (this._ucdRepository.ValidateNullClaimIndGrossAmount(pirCode, claimID, nullClaimInd))
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "298.4", openExceptions, false);
                }
                else
                {
                    ClaimResponseValueClass newError = new ClaimResponseValueClass { ResponseException = new Model.Drools.V1.ResponseExceptionClass() { Rule = "298.4", ShortDescription = "nullClaimInd cannot by Y. Payment gross amount not equal to 0", SLACategory = "IntegrityChecksOnAllNonBlankFields", Tier = 1, Type = "Warning" } };
                    this._InsertNewErrors(transactionID, CTPCLAIMEXCEPTIONTYPE, claimID, newError, openExceptions, false);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw;
            }


        }

        private void _ValidateNullClaimIndEqualsYNetAmountEquals0(string transactionID,string claimID, string pirCode, string nullClaimInd, List<ExceptionClass> openExceptions)
        {

            try {

                if (nullClaimInd.IsNullOrEmptyAfterTrim() || nullClaimInd.ToUpper() != "Y")
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "298.5", openExceptions, false);
                    return;
                }

                if (this._ucdRepository.ValidateNullClaimIndNetAmount(pirCode, claimID, nullClaimInd))
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "298.5", openExceptions, false);
                }
                else
                {
                    ClaimResponseValueClass newError = new ClaimResponseValueClass { ResponseException = new Model.Drools.V1.ResponseExceptionClass() { Rule = "298.5", ShortDescription = "nullClaimInd cannot by Y. Net gross amount not equal to 0", SLACategory = "IntegrityChecksOnAllNonBlankFields", Tier = 1, Type = "Warning" } };
                    this._InsertNewErrors(transactionID, CTPCLAIMEXCEPTIONTYPE, claimID, newError, openExceptions, false);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw exception;
            }

          
        }
        private void _ValidateIsAccidentEventNumUnique(string transactionID, string claimID, string pirCode, ClaimRequestClass request,List<ExceptionClass> openExceptions)
        {
            try {

                if (request.claim.accident == null ||request.claim.accident.accidentID.IsNullOrEmptyAfterTrim()|| request.claim.accident.eventNum.IsNullOrEmptyAfterTrim()) return;



                if (this._ucdRepository.IsAccidentEventNumUnique(pirCode, request.claim.accident.accidentID, request.claim.accident.eventNum))
                {
                    this._CloseExistingErrors(transactionID, claimID, 1, "186.1", openExceptions, false);
                }
                else
                {
                    ClaimResponseValueClass newError = new ClaimResponseValueClass { ResponseException = new Model.Drools.V1.ResponseExceptionClass() { Rule = "186.1", ShortDescription = "EventNum must be unique", SLACategory = "IntegrityChecksOnAllNonBlankFields", Tier = 1, Type = "Warning" } };
                    this._InsertNewErrors(transactionID, CTPCLAIMEXCEPTIONTYPE, claimID, newError, openExceptions, false);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw exception;
            }
        
        }
        private void _ValidateOrigTranIdExists(string transactionID, string claimID, string pirCode, PaymentRequestClass paymentRequest, List<ExceptionClass> openExceptions)
        {

            try
            {
                foreach(var payment in paymentRequest.payment)
                {
                    if (payment.originalTransactionID.IsNullOrEmptyAfterTrim()) continue;

                    if (this._ucdRepository.IsOriginalTransactionIDExists(pirCode ,claimID, payment.originalTransactionID))
                    {
                        this._CloseExistingErrors(transactionID, claimID, 1, "309.2", openExceptions, false);
                    }
                    else
                    {
                        ClaimResponseValueClass newError = new ClaimResponseValueClass { ResponseException = new Model.Drools.V1.ResponseExceptionClass() { Rule = "309.2", ShortDescription = "originalTransactionID must exists", SLACategory = "IntegrityChecksOnAllNonBlankFields", Tier = 1, Type = "Error", exceptionReference= payment.transactionID  } };
                        this._InsertNewErrors(transactionID, CTPPAYMENTEXCEPTIONTYPE, claimID, newError, openExceptions, false);
                    }
                }

            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw exception;
            }

        }
        private void _ValidateWithDrools(string transactionID,string claimID, ClaimRequestClass request, List<ExceptionClass> openExceptions)
        {
            try {

                var apiRequest = _CreateWebRequest("claimEndPoint");

                var droolsRequest = new Model.Drools.V1.RequestClass();
                //var javaUtilDate = request.providerProcessedDateTime.ParseIso8601("yyyyMMdd'T'HHmmss'Z'").Value.ToString("yyyy-MM-dd");
                var javaUtilDate = request.providerProcessedDateTime.ParseIso8601("yyyyMMdd'T'HHmmss'Z'").Value.Date.ToString("yyyyMMddTHHmmss") + "Z";
                droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { SetGlobalCommand = new Model.Drools.V1.SetGlobalCommandClass() { JavaUtilDateCommand = new Model.Drools.V1.JavaUtilDateCommandClass() { JavaUtilDate = javaUtilDate } } });
                // droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { InsertCommand = new Model.Drools.V1.InsertCommandClass() { ReturnObject=true,  OutIdentifier = "inValue", InsertObject = new Model.Drools.V1.InsertObjectCommandClass() { Claim = request.claim } } });
                droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { InsertCommand = new Model.Drools.V1.InsertCTPClaimCommandClass() { InsertObject = new Model.Drools.V1.InsertCTPClaimObjCommandClass() { Claim = request.claim } } });
                droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { FireAllRules = string.Empty });
                droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { GetObjects = new Model.Drools.V1.GetObjectsCommandClass() });
                var jsonString = JsonConvert.SerializeObject(droolsRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var responseString = string.Empty;
                using (var streamWriter = new StreamWriter(apiRequest.GetRequestStream()))
                {


                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)apiRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        responseString = streamReader.ReadToEnd();
                    }
                }
                var response = JsonConvert.DeserializeObject<Model.Drools.V1.ResponseClass>(responseString);

                if (response.Type.ToUpper().Trim() == "FAILURE")
                {
                    throw new Exception(response.Msg);

                }

                var result = response.Result.ExecutionResult.ExecutionResultResult[0];
                if (result.Value.Count == 0)
                {
                    this._CloseExistingErrors(transactionID, openExceptions, true);
                    return;
                }
                var droolExceptions = new List<Model.Drools.V1.BaseResponseValueClass>();
                for (int i = 0; i <= result.Value.Count - 1; i++)
                {
                    var droolException = result.Value[i].ToObject<Model.Drools.V1.ClaimResponseValueClass>();
                    if (droolException.ResponseException == null) continue;
                    droolExceptions.Add(droolException);

                }

                this._InsertNewErrors(transactionID, CTPCLAIMEXCEPTIONTYPE, claimID, droolExceptions, openExceptions, true);
                this._CloseExistingErrors(transactionID, claimID, droolExceptions, openExceptions, true);



            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw exception;
            }


        }


        private void _ValidateWithDrools(string transactionID, string claimID, PaymentRequestClass paymentRequest, ClaimRequestClass claimRequest, List<ExceptionClass> openExceptions)
        {

            try {
                int index = 0;
                var droolExceptions = new List<Model.Drools.V1.BaseResponseValueClass>();

                foreach (var payment in paymentRequest.payment)
                {

                    var droolsRequest = new Model.Drools.V1.RequestClass();
                    //var javaUtilDate = request.providerProcessedDateTime.ParseIso8601("yyyyMMdd'T'HHmmss'Z'").Value.ToString("yyyy-MM-dd");
                    var javaUtilDate = paymentRequest.providerProcessedDateTime.ParseIso8601("yyyyMMdd'T'HHmmss'Z'").Value.Date.ToString("yyyyMMddTHHmmss") + "Z";
                    droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { SetGlobalCommand = new Model.Drools.V1.SetGlobalCommandClass() { JavaUtilDateCommand = new Model.Drools.V1.JavaUtilDateCommandClass() { JavaUtilDate = javaUtilDate } } });
                    // droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { InsertCommand = new Model.Drools.V1.InsertCommandClass() { ReturnObject=true,  OutIdentifier = "inValue", InsertObject = new Model.Drools.V1.InsertObjectCommandClass() { Claim = request.claim } } });
                    droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass()
                    {
                        InsertCommand = new Model.Drools.V1.InsertCTPClaimPayCommandClass()
                        {
                            InsertObject = new Model.Drools.V1.InsertCTPClaimPayObjCommandClass()
                            {
                                paymentPayload = new Model.Drools.V1.InsertCTPPayPayloadObjCommandClass()
                                {
                                    Payment = payment,
                                    Claim = claimRequest.claim
                                }
                            }
                        }
                    });
                    droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { FireAllRules = string.Empty });
                    droolsRequest.commands.Add(new Model.Drools.V1.RequestCommandClass() { GetObjects = new Model.Drools.V1.GetObjectsCommandClass() { OutIdentifier = string.Empty } });
                    var jsonString = JsonConvert.SerializeObject(droolsRequest, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    var responseString = string.Empty;
                    var apiRequest = _CreateWebRequest("paymentEndPoint");
                    using (var streamWriter = new StreamWriter(apiRequest.GetRequestStream()))
                    {


                        streamWriter.Write(jsonString);
                        streamWriter.Flush();
                        streamWriter.Close();

                        var httpResponse = (HttpWebResponse)apiRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            responseString = streamReader.ReadToEnd();
                        }
                    }
                    var response = JsonConvert.DeserializeObject<Model.Drools.V1.ResponseClass>(responseString);

                    if (response.Type.ToUpper().Trim() == "FAILURE")
                    {
                        throw new Exception(response.Msg);

                    }

                    var result = response.Result.ExecutionResult.ExecutionResultResult[0];
                    //if (result.Value.Count == 0)
                    //{
                    //    this._CloseExistingErrors(transactionID, openExceptions, index, true);
                    //    return;
                    //}

                    for (int i = 0; i <= result.Value.Count - 1; i++)
                    {
                        var droolException = result.Value[i].ToObject<Model.Drools.V1.PaymentResponseValueClass>();
                        if (droolException.ResponseException == null) continue;
                        if (droolExceptions.Exists(x => x.ResponseException.Rule == droolException.ResponseException.Rule && x.ResponseException.exceptionReference == droolException.ResponseException.exceptionReference)) continue;
                        droolExceptions.Add(droolException);

                    }


                    index++;


                }

                this._InsertNewErrors(transactionID, CTPPAYMENTEXCEPTIONTYPE, claimID, droolExceptions, openExceptions, true);
                this._CloseExistingErrors(transactionID, claimID, droolExceptions, openExceptions, true);
            }
            catch (Exception exception)
            {
                HandleException(exception);
                throw exception;
            }
 

        }



        private WebRequest _CreateWebRequest(string droolsEndPoint)
        {
            NameValueCollection droolsConfiguration = ConfigurationManager.GetSection("DroolsConfiguration") as NameValueCollection;
            var apiRequest = (HttpWebRequest)WebRequest.Create(droolsConfiguration[droolsEndPoint]);
            apiRequest.Method = "POST";
            apiRequest.ContentType = droolsConfiguration["header-Content-Type"];
            apiRequest.Accept = droolsConfiguration["header-Accept"];
            apiRequest.Headers.Add("X-KIE-ContentType", droolsConfiguration["header-X-KIE-ContentType"]);
            apiRequest.Headers.Add("Authorization", droolsConfiguration["header-Authorization"]);
            return apiRequest;
        }

        private ClaimRequestClass _GetClaimRequest(string claimID)
        {
            var ctpClaim = this._ucdRepository.GetClaimTransaction(this.APIKey, this.InsurerCode, this.TransactionID,
claimID, false);
            if (ctpClaim == null) return null;

            return JsonConvert.DeserializeObject<ClaimRequestClass>(ctpClaim.ToString());
        }


        private void _UnpackClaimTransaction (string transactionID, string pirCode)
        {
            try {

                System.Threading.Thread.Sleep(2000);

                this._ucdRepository.WriteClaimTransactionAsync(transactionID, pirCode);
            }
            catch (Exception exception)
            {
                HandleException(exception);
                return;
            }
        }

        private void _UnpackPaymentTransaction(string transactionID, string pirCode)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);

                this._ucdRepository.WritePaymentTransactionAsync(transactionID, pirCode);
            }
            catch (Exception exception)
            {
                HandleException(exception);
                return;
            }
        }
    }
}
