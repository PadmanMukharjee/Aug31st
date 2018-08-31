using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Client;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Enums;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Business.Admin
{
    public class KPIBusiness : IKPIBusiness
    {
        #region Private Variables

        private IKPIRepository _kpiRepository;
        private ILogger _logger;

        #endregion Private Variables

        #region constructor
        public KPIBusiness(IKPIRepository kpiRepository, ILogger logger)
        {
            _kpiRepository = kpiRepository;
            _logger = logger;
        }

        #endregion constructor

        #region public Methods

        /// <summary>
        /// get checkList type
        /// </summary>
        /// <returns></returns>
        public List<CheckListTypeViewModel> GetCheckListTypes()
        {
            List<CheckListTypeViewModel> checkListTypes = new List<CheckListTypeViewModel>();
            try
            {
                List<BusinessModel.BusinessModels.CheckListType> checkListTypesDTO = _kpiRepository.GetCheckListTypes();
                if (checkListTypesDTO != null)
                {
                    foreach (BusinessModel.BusinessModels.CheckListType checkListTypeDTO in checkListTypesDTO)
                    {
                        CheckListTypeViewModel checkListType = new CheckListTypeViewModel();
                        checkListType.CheckListTypeID = checkListTypeDTO.CheckListTypeID;
                        checkListType.CheckListTypeCode = checkListTypeDTO.CheckListTypeCode;
                        checkListType.CheckListTypeName = checkListTypeDTO.CheckListTypeName;
                        checkListType.RecordStatus = checkListTypeDTO.RecordStatus;
                        checkListTypes.Add(checkListType);
                    }
                }
                return checkListTypes;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// getting KPI question based on checkListType
        /// </summary>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        public List<MeasureViewModel> GetKPIQuestionBasedOnCheckListType(string checkListTypeCode)
        {
            List<MeasureViewModel> measures = new List<MeasureViewModel>();
            try
            {
                if (checkListTypeCode != ChecklistType.M3.ToString())
                {
                    IEnumerable<Question> questionsDTO = _kpiRepository.GetKPIQuestionBasedOnCheckListType(checkListTypeCode);
                    if (questionsDTO != null)
                    {
                        foreach (Question questionDto in questionsDTO)
                        {
                            MeasureViewModel measure = new MeasureViewModel();
                            measure.MeasureId = questionDto.QuestionId;
                            measure.MeasureText = questionDto.QuestionText;
                            measure.MeasureCode = questionDto.QuestionCode;
                            measure.Standard = questionDto.ExpectedResponse;
                            measure.Universal = questionDto.IsUniversal;
                            measure.KPIID = GetKPIIdBasedonQuestion(questionDto.QuestionId, checkListTypeCode);
                            measures.Add(measure);
                        }
                    }
                }
                else
                {
                    List<M3metricsQuestion> questionsDTO = _kpiRepository.GetM3metricsQuestion();
                    if (questionsDTO != null)
                    {
                        foreach (M3metricsQuestion questionDto in questionsDTO)
                        {
                            MeasureViewModel measure = new MeasureViewModel();
                            measure.MeasureId = questionDto.M3metricsQuestionId;
                            measure.MeasureText = questionDto.M3metricsQuestionText;
                            measure.MeasureCode = questionDto.M3metricsQuestionCode;
                            measure.MeasureUnit = questionDto.M3metricsUnit;
                            measure.KPIID = GetKPIIdBasedonQuestion(questionDto.M3metricsQuestionId, checkListTypeCode);

                            measures.Add(measure);
                        }
                    }
                }

                return measures;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saving KPI Details
        /// </summary>
        /// <param name="kpiViewModel"></param>
        /// <returns></returns>
        public bool SaveKPIDetails(KPIViewModel kpiViewModel)
        {
            try
            {
                if (kpiViewModel != null)
                {
                    KPI kpiDTO = new KPI();
                    kpiDTO.KPIID = kpiViewModel.KPIID;
                    kpiDTO.KPIDescription = kpiViewModel.KPIDescription;

                    kpiDTO.Source = new CheckListType();
                    kpiDTO.Source.CheckListTypeID = kpiViewModel.Source.CheckListTypeID;
                    kpiDTO.Source.CheckListTypeCode = kpiViewModel.Source.CheckListTypeCode;
                    kpiDTO.Source.CheckListTypeName = kpiViewModel.Source.CheckListTypeName;
                    kpiDTO.Source.RecordStatus = kpiViewModel.Source.RecordStatus;

                    kpiDTO.Measure = new Measure();
                    kpiDTO.Measure.MeasureId = kpiViewModel.Measure.MeasureId;
                    kpiDTO.Measure.MeasureText = kpiViewModel.Measure.MeasureText;

                    kpiDTO.Standard = kpiViewModel.Standard;
                    kpiDTO.AlertLevel = kpiViewModel.AlertLevel;

                    kpiDTO.KPIMeasure = new KPIMeasure();
                    kpiDTO.KPIMeasure.KpimeasureId = kpiViewModel.KPIMeasure.KpimeasureId;
                    kpiDTO.KPIMeasure.Measure = kpiViewModel.KPIMeasure.Measure;
                    kpiDTO.KPIMeasure.CheckListTypeId = kpiViewModel.KPIMeasure.CheckListTypeId;
                    kpiDTO.KPIMeasure.RecordStatus = kpiViewModel.KPIMeasure.RecordStatus;

                    kpiDTO.IsHeatMapItem = kpiViewModel.IsHeatMapItem;
                    kpiDTO.HeatMapScore = kpiViewModel.HeatMapScore;
                    kpiDTO.IsUniversal = kpiViewModel.IsUniversal;
                    kpiDTO.RecordStatus = kpiViewModel.RecordStatus;

                    kpiDTO.KPIAlert = new KPIAlert();
                    kpiDTO.KPIAlert.KPIAlertId = kpiViewModel.KPIAlertId;
                    kpiDTO.KPIAlert.SendAlert = kpiViewModel.SendAlert;
                    kpiDTO.KPIAlert.SendAlertTitle = kpiViewModel.SendAlertTitle;
                    kpiDTO.KPIAlert.SendToRelationshipManager = kpiViewModel.SendToRelationshipManager;
                    kpiDTO.KPIAlert.SendToBillingManager = kpiViewModel.SendToBillingManager;
                    kpiDTO.KPIAlert.EscalateAlert = kpiViewModel.EscalateAlert;
                    kpiDTO.KPIAlert.EscalateAlertTitle = kpiViewModel.EscalateAlertTitle;
                    kpiDTO.KPIAlert.EscalateTriggerTime = kpiViewModel.EscalateTriggerTime;
                    kpiDTO.KPIAlert.IncludeKPITarget = kpiViewModel.IncludeKPITarget;
                    kpiDTO.KPIAlert.IncludeDeviationTarget = kpiViewModel.IncludeDeviationTarget;
                    kpiDTO.KPIAlert.IsSla = kpiViewModel.IsSla;
                    return _kpiRepository.SaveKPIs(kpiDTO);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Getting measure based in checkList Type
        /// </summary>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        public List<KPIMeasureViewModel> GetMeasureBasedOnClientTypeID(int checkListTypeId)
        {
            List<KPIMeasureViewModel> measures = new List<KPIMeasureViewModel>();
            try
            {
                List<KPIMeasure> questionsDTO = _kpiRepository.GetMeasureBasedOnClientTypeID(checkListTypeId);
                if (questionsDTO != null)
                {
                    foreach (KPIMeasure questionDto in questionsDTO)
                    {
                        KPIMeasureViewModel measure = new KPIMeasureViewModel();
                        measure.KpimeasureId = questionDto.KpimeasureId;
                        measure.CheckListTypeId = questionDto.CheckListTypeId;
                        measure.Measure = questionDto.Measure;
                        measure.RecordStatus = questionDto.RecordStatus;
                        measures.Add(measure);
                    }
                }
                return measures;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Getting all KPIs
        /// </summary>
        /// <returns></returns>
        public List<KPIViewModel> GetAllKPIs()
        {
            List<KPIViewModel> kpisDTO = new List<KPIViewModel>();
            try
            {
                List<KPI> kpis = _kpiRepository.GetAllKPIs();
                if (kpisDTO != null)
                {
                    foreach (KPI item in kpis)
                    {
                        KPIViewModel kpiDTO = BusinessMapper.MappingKPIBusinessToKPIViewModel(item);
                        kpisDTO.Add(kpiDTO);
                    }
                }
                return kpisDTO;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Getting all M3Metrics KPIs
        /// </summary>
        /// <returns></returns>
        public List<KPIViewModel> GetAllM3MetricsKPIs()
        {
            List<KPIViewModel> kpisDTO = new List<KPIViewModel>();
            try
            {
                List<KPI> kpis = _kpiRepository.GetM3MetricsKPIs();
                if (kpisDTO != null)
                {
                    foreach (KPI item in kpis)
                    {
                        KPIViewModel kpiDTO = BusinessMapper.MappingKPIBusinessToKPIViewModel(item);
                        kpisDTO.Add(kpiDTO);
                    }
                }
                return kpisDTO;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Getting kpi based on ID
        /// </summary>
        /// <param name="KPIId"></param>
        /// <returns></returns>
        public KPIViewModel GetKPIById(int KPIId)
        {
            KPIViewModel kpi = new KPIViewModel();
            try
            {
                KPI kpiDTO = _kpiRepository.GetKPIById(KPIId);
                return BusinessMapper.MappingKPIBusinessToKPIViewModel(kpiDTO); ;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Getting KPIId if KPI desciprion is created for that question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        public int GetKPIIdBasedonQuestion(int questionId, string checkListTypeCode)
        {
            try
            {
                return _kpiRepository.GetKPIIdBasedonQuestion(questionId, checkListTypeCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// To get KPI questions to assign for client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<KPIQuestionViewModel> GetKPIQuestionsForClient(string clientCode)
        {
            List<KPIQuestionViewModel> clientKpiViewModel = new List<KPIQuestionViewModel>();
            try
            {
                List<KPI> clientKPIQuestions = _kpiRepository.GetKPIQuestionsForClient(clientCode);

                List<int> kpiIds = clientKPIQuestions.Select(k => k.KPIID).Distinct().ToList();

                foreach (int kpiId in kpiIds)
                {
                    KPI kpis = clientKPIQuestions.Where(k => k.KPIID == kpiId).OrderBy(k => k.EndDate).FirstOrDefault();

                    KPIQuestionViewModel kpiViewModel = new KPIQuestionViewModel();
                    kpiViewModel.KpiId = kpis.KPIID;
                    kpiViewModel.KpiDescription = kpis.KPIDescription;
                    kpiViewModel.CompanyStandard = kpis.AlertLevel;
                    kpiViewModel.ChecklistTypeViewModel.CheckListTypeCode = kpis.Source.CheckListTypeCode;
                    kpiViewModel.IsUniversal = kpis.IsUniversal;
                    if (kpis.Measure != null && kpis.Source.CheckListTypeCode == DomainConstants.M3)
                    {
                        kpiViewModel.M3MeasureViewModel.MeasureCode = kpis.Measure.MeasureCode;
                        kpiViewModel.M3MeasureViewModel.MeasureUnit = kpis.Measure.MeasureUnit;

                    }
                    else
                    {
                        kpiViewModel.M3MeasureViewModel = null;
                    }
                    clientKpiViewModel.Add(kpiViewModel);


                }
                return clientKpiViewModel;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }


        /// <summary>
        /// To Save KPI for the Client
        /// </summary>
        /// <param name="clientKPI"></param>
        /// <returns></returns>
        public bool SaveKPIForClient(ClientKPISetupViewModel clientKPI)
        {
            try
            {
                if (clientKPI?.KpiQuestions?.Count > 0)
                {
                    ClientKPISetup clientKPISetup = new ClientKPISetup();
                    clientKPISetup.ClientCode = clientKPI.ClientCode;
                    foreach (KPISetupViewModel KPIViewModel in clientKPI.KpiQuestions)
                    {
                        KPISetup kPISetup = new KPISetup();
                        kPISetup.ClientStandard = KPIViewModel.ClientStandard;
                        kPISetup.Sla = KPIViewModel.Sla;
                        kPISetup.Kpi.KPIID = KPIViewModel.Kpi.KpiId;
                        kPISetup.ClientKPIMapId = KPIViewModel.clientKPIMapId;
                        foreach (AllUsersViewModel user in KPIViewModel.SendTo)
                        {
                            kPISetup.SendTo.Add(new AllUsers() { ID = user.ID, Email = user.Email });
                        }
                        clientKPISetup.KPIQuestions.Add(kPISetup);
                    }
                    return _kpiRepository.SaveKPIForClient(clientKPISetup);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientKPISetupViewModel GetClientAssignedKPIs(string clientCode)
        {
            try
            {
                ClientKPISetupViewModel clientKPISetupViewModel = new ClientKPISetupViewModel();

                ClientKPISetup clientAssignedM3KPIs = _kpiRepository.GetClientAssignedM3KPIs(clientCode);

                if (clientAssignedM3KPIs.KPIQuestions?.Count > 0)
                {

                    foreach (KPISetup clientKPIDto in clientAssignedM3KPIs.KPIQuestions)
                    {
                        KPISetupViewModel clientKPI = new KPISetupViewModel();

                        clientKPI.ClientStandard = clientKPIDto.ClientStandard;
                        if (!string.IsNullOrEmpty(clientKPI.ClientStandard))
                        {
                            string[] words = clientKPI.ClientStandard.Split(',');

                            clientKPI.AlertLevel = words[0];
                            clientKPI.AlertValue = words[1];
                            clientKPI.ClientStandard = clientKPI.ClientStandard.Replace(",", " ");
                        }

                        clientKPI.Sla = clientKPIDto.Sla;
                        clientKPI.clientKPIMapId = clientKPIDto.ClientKPIMapId;
                        clientKPI.Kpi.ChecklistTypeViewModel.CheckListTypeCode = clientKPIDto.Kpi.Source.CheckListTypeCode;
                        clientKPI.Kpi.CompanyStandard = clientKPIDto.Kpi.AlertLevel.Replace(",", " ");
                        clientKPI.Kpi.IsUniversal = clientKPIDto.Kpi.IsUniversal;
                        clientKPI.Kpi.KpiDescription = clientKPIDto.Kpi.KPIDescription;
                        clientKPI.Kpi.KpiId = clientKPIDto.Kpi.KPIID;
                        if (clientKPIDto.Kpi.Measure != null && clientKPIDto.Kpi.Source.CheckListTypeCode == DomainConstants.M3)
                        {
                            clientKPI.Kpi.M3MeasureViewModel.MeasureCode = clientKPIDto.Kpi.Measure.MeasureCode;
                            clientKPI.Kpi.M3MeasureViewModel.MeasureUnit = clientKPIDto.Kpi.Measure.MeasureUnit;
                        }
                        else
                        {
                            clientKPI.Kpi.M3MeasureViewModel = null;
                        }
                        if (clientKPIDto.SendTo?.Count > 0)
                        {
                            foreach (AllUsers user in clientKPIDto.SendTo)
                            {
                                clientKPI.SendTo.Add(BusinessMapper.AllUsersBusinessToAllUsesrViewModel(user));
                            }
                        }
                        clientKPISetupViewModel.KpiQuestions.Add(clientKPI);

                    }
                }

                ClientKPIDetails clientAssignedWeeklyMonthlyKPIs = _kpiRepository.GetClientAssignedWeeklyMonthlyKPIs(clientCode);

                List<int> kpiIds = clientAssignedWeeklyMonthlyKPIs.clientKPIAssignedDetails.Select(c => c.KPIID)?.Distinct()?.ToList();

                foreach (int kpiId in kpiIds)
                {
                    List<ClientKPIAssignedDetails> kpiDetailsForKPIId = clientAssignedWeeklyMonthlyKPIs.clientKPIAssignedDetails.Where(k => k.KPIID == kpiId)?.OrderBy(k => k.ClientKPIMapID).ToList();
                    KPISetupViewModel clientKPI = new KPISetupViewModel();
                    if (kpiDetailsForKPIId.Count >= 2)
                    {
                        if (kpiDetailsForKPIId.Count > 2)
                        {
                            kpiDetailsForKPIId = kpiDetailsForKPIId.Where(k => k.KPIID == kpiId && k.ChecklistEndDate != DateTime.MaxValue.Date).ToList();
                        }

                        if (kpiDetailsForKPIId.Count == 2)
                        {
                            ClientKPIAssignedDetails previousClientKPI = kpiDetailsForKPIId[0];
                            ClientKPIAssignedDetails newClientKPI = kpiDetailsForKPIId[1];

                            if (previousClientKPI.IsKPI)
                            {
                                ClientKPIAssignedDetailsToKPISetupViewModel(clientAssignedWeeklyMonthlyKPIs, clientKPI, previousClientKPI);
                                if (!newClientKPI.IsKPI)//From KPI to made NonKPI //4th,5th scenario
                                {

                                    clientKPI.Info = InfoMessages.ItemWillRemoveFrom + previousClientKPI.QuestionEndDate.Date.ToString("MM/dd/yyyy");
                                    clientKPI.FutureRemoverOrUniversal = true;
                                }
                                else if (previousClientKPI.IsUnivarsal && !newClientKPI.IsUnivarsal)  //From Universal to made non Universal 3rd scenario
                                {

                                    clientKPI.Info = InfoMessages.ItemNonUniversalFrom + previousClientKPI.QuestionEndDate.Date.ToString("MM/dd/yyyy");


                                }
                                else if (!previousClientKPI.IsUnivarsal && newClientKPI.IsUnivarsal)  //Ftom nonUniversal to made universal 2nd Scenario
                                {
                                    clientKPI.Info = InfoMessages.ItemUniversalFrom + newClientKPI.QuestionEffectiveDate.Date.ToString("MM/dd/yyyy");
                                    clientKPI.FutureRemoverOrUniversal = true;

                                }
                                else if (previousClientKPI.ChecklistQuestionEffectiveDate > DateTime.Now.Date && newClientKPI.ChecklistQuestionEffectiveDate > DateTime.Now.Date) //No changes on KPI and Universal
                                {
                                    clientKPI.Info = InfoMessages.ItemEffectiveFrom + previousClientKPI.ChecklistQuestionEffectiveDate.ToString("MM/dd/yyyy");
                                }
                                else if (previousClientKPI.ChecklistEffectiveDate > DateTime.Now.Date && newClientKPI.ChecklistEffectiveDate > DateTime.Now.Date) //No changes on KPI and Universal
                                {
                                    clientKPI.Info = InfoMessages.ItemEffectiveFrom + previousClientKPI.ChecklistEffectiveDate.ToString("MM/dd/yyyy");
                                }
                                clientKPISetupViewModel.KpiQuestions.Add(clientKPI);

                            }
                            else if (!previousClientKPI.IsKPI && newClientKPI.IsKPI) //If previously non KPI and now made KPI
                            {
                                ClientKPIAssignedDetailsToKPISetupViewModel(clientAssignedWeeklyMonthlyKPIs, clientKPI, newClientKPI);
                                if (newClientKPI.ChecklistEffectiveDate > DateTime.Now.Date)// To check if checklist is starting from future
                                {
                                    clientKPI.Info = InfoMessages.ItemEffectiveFrom + newClientKPI.ChecklistEffectiveDate.ToString("MM/dd/yyyy");
                                }
                                else if (newClientKPI.ChecklistQuestionEffectiveDate > DateTime.Now.Date) //If assigned Quedtion is starting from future
                                {
                                    clientKPI.Info = InfoMessages.ItemEffectiveFrom + newClientKPI.ChecklistQuestionEffectiveDate.ToString("MM/dd/yyyy");
                                }
                                if (newClientKPI.IsUnivarsal)
                                {
                                    clientKPI.FutureRemoverOrUniversal = true;
                                }
                                clientKPISetupViewModel.KpiQuestions.Add(clientKPI);
                            }
                        }
                    }
                    else
                    { //1st scenario

                        ClientKPIAssignedDetails currentAssignedKPI = kpiDetailsForKPIId.FirstOrDefault();
                        ClientKPIAssignedDetailsToKPISetupViewModel(clientAssignedWeeklyMonthlyKPIs, clientKPI, currentAssignedKPI);

                        if (currentAssignedKPI.KPIAssignedEndDate != DateTime.MaxValue.Date) //To check if the KPI is ending in future and only one record is assigned
                        {

                            clientKPI.Info = InfoMessages.ItemWillRemoveFrom + currentAssignedKPI.KPIAssignedEndDate.Date.ToString("MM/dd/yyyy");
                            clientKPI.FutureRemoverOrUniversal = true;

                        }
                        else if (currentAssignedKPI.ChecklistEndDate != DateTime.MaxValue.Date) //If assigned Question is starting from future
                        {
                            clientKPI.Info = InfoMessages.ItemWillRemoveFrom + currentAssignedKPI.ChecklistEndDate.Date.ToString("MM/dd/yyyy");
                            clientKPI.FutureRemoverOrUniversal = true;
                        }
                        else if (currentAssignedKPI.ChecklistEffectiveDate > DateTime.Now.Date)// To check if checklist is starting from future
                        {

                            clientKPI.Info = InfoMessages.ItemEffectiveFrom + currentAssignedKPI.ChecklistEffectiveDate.ToString("MM/dd/yyyy");

                        }
                        else if (currentAssignedKPI.ChecklistQuestionEffectiveDate > DateTime.Now.Date) //If assigned Question to checklist is starting from future
                        {
                            clientKPI.Info = InfoMessages.ItemEffectiveFrom + currentAssignedKPI.ChecklistQuestionEffectiveDate.ToString("MM/dd/yyyy");
                        }
                        else if (currentAssignedKPI.QuestionEffectiveDate > DateTime.Now.Date) //If assigned Question is starting from future
                        {
                            clientKPI.Info = InfoMessages.ItemEffectiveFrom + currentAssignedKPI.QuestionEffectiveDate.ToString("MM/dd/yyyy");
                        }
                        clientKPISetupViewModel.KpiQuestions.Add(clientKPI);
                    }
                }
                return clientKPISetupViewModel;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;

            }
        }

        /// <summary>
        /// To get the kpi id's of heat map items. 
        /// </summary>
        /// <returns></returns>
        public List<int> GetKpiHeatMapItems()
        {
            return _kpiRepository.GetKpiHeatMapItems();
        }
        #endregion Public Methods

        #region private Methods

        /// <summary>
        /// Mapping weekly and monthly kpis
        /// </summary>
        /// <param name="clientAssignedWeeklyMonthlyKPIs"></param>
        /// <param name="clientKPI"></param>
        /// <param name="currentAssignedKPI"></param>
        private void ClientKPIAssignedDetailsToKPISetupViewModel(ClientKPIDetails clientAssignedWeeklyMonthlyKPIs, KPISetupViewModel clientKPI, ClientKPIAssignedDetails currentAssignedKPI)
        {
            clientKPI.Sla = currentAssignedKPI.IsSLA;
            clientKPI.clientKPIMapId = currentAssignedKPI.ClientKPIMapID;
            clientKPI.Kpi.ChecklistTypeViewModel.CheckListTypeCode = currentAssignedKPI.ChecklistType;
            clientKPI.Kpi.CompanyStandard = currentAssignedKPI.CompanyStandard.ToString() == "1" ? BusinessConstants.YES : BusinessConstants.NO;
            clientKPI.Kpi.IsUniversal = currentAssignedKPI.IsUnivarsal;
            clientKPI.Kpi.KpiDescription = currentAssignedKPI.KPIDescription;
            clientKPI.Kpi.KpiId = currentAssignedKPI.KPIID;
            List<AllUsers> sendTo = clientAssignedWeeklyMonthlyKPIs.clientKPIAssignedUserDetails.Where(k => k.ClientKPIMapId == currentAssignedKPI.ClientKPIMapID)?.ToList();
            if (sendTo?.Count > 0)
            {
                foreach (AllUsers user in sendTo)
                {
                    clientKPI.SendTo.Add(BusinessMapper.AllUsersBusinessToAllUsesrViewModel(user));
                }
            }
        }
        #endregion private Methods
    }
}
