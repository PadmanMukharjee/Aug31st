using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.ClientData;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Client;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers
{
    // [Authorize]
    [Route("api/[controller]/[Action]")]
    public class ClientDataController : Controller
    {
        private IClientDataBusiness _clientDataBusiness;
        public ClientDataController(IClientDataBusiness clientDataBusiness)
        {
            _clientDataBusiness = clientDataBusiness;
        }

        /// <summary>
        /// To update and Save ClientData into database
        /// </summary>
        /// <param name="clientData"></param>

        [HttpPost, DisableRequestSizeLimit]
        //[AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive)]
        public ValidationViewModel SaveClientData(ClientViewModel clientData)
        {
            return _clientDataBusiness.SaveClientData(clientData);
        }

        /// <summary>
        /// To get client Data related to clientCode
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        public ClientViewModel GetClientData(string clientCode)
        {
            return _clientDataBusiness.GetClientData(clientCode);
        }
        /// <summary>
        /// save the Client target data based on Annual charges and Gross collection rate
        /// </summary>
        /// <param name="clientTargetData"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive)]
        public MonthWiseClientTargetViewModel SaveClientTargetData([FromBody]ClientTargetViewModel clientTargetData)
        {
            MonthWiseClientTargetViewModel monthWiseClientTargetViewModel = new MonthWiseClientTargetViewModel();
            if (clientTargetData != null)
            {
                List<ClientTargetViewModel> clientTargetViewModels = _clientDataBusiness.SaveClientTargetData(clientTargetData);
                if (clientTargetViewModels != null && clientTargetViewModels.Count > 0)
                {
                    monthWiseClientTargetViewModel.clientTargetViewModels = clientTargetViewModels;
                    monthWiseClientTargetViewModel.ValidationViewModel = new ValidationViewModel();
                    monthWiseClientTargetViewModel.ValidationViewModel.Success = true;
                }
                else
                {
                    monthWiseClientTargetViewModel.clientTargetViewModels = null;
                    monthWiseClientTargetViewModel.ValidationViewModel = new ValidationViewModel();
                    monthWiseClientTargetViewModel.ValidationViewModel.Success = false;
                }
                return monthWiseClientTargetViewModel;
            }
            else
            {
                monthWiseClientTargetViewModel.clientTargetViewModels = null;
                monthWiseClientTargetViewModel.ValidationViewModel = new ValidationViewModel();
                monthWiseClientTargetViewModel.ValidationViewModel.Success = false;
                return monthWiseClientTargetViewModel;
            }

        }
        /// <summary>
        /// To get the Client Target data if any
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive)]
        [HttpGet]
        public List<ClientTargetViewModel> GetClientTargetData(string clientCode, int year)
        {
            return _clientDataBusiness.GetClientTargetData(clientCode, year);
        }
        /// <summary>
        /// To Save the manually Edited  targets in Grid view
        /// </summary>
        /// <param name="manuallyEditedTargets"></param>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive)]
        [HttpPost]
        public ValidationViewModel SaveManuallyEditedTargetData([FromBody]ManuallyEditedTargets manuallyEditedTargets)
        {
            return _clientDataBusiness.SaveManuallyEditedTargetData(manuallyEditedTargets);
        }
        /// <summary>
        /// To get Contract document od the client depending on the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive)]
        [HttpGet]
        public ClientViewModel GetDocument(string path)
        {
            //TODO:Need to change to return only string
            ClientViewModel c = new ClientViewModel();
            c.Name = System.Convert.ToBase64String(_clientDataBusiness.GetClientDocument(path));
            return c;
        }

        /// <summary>
        /// Get all Client Configuration Step details 
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ClientStepDetailViewModel> GetClientStepStatusDetails(string clientCode)
        {
            return _clientDataBusiness.GetClientStepStatusDetails(clientCode);
        }

        /// <summary>
        /// Add/Update Client's Step status details
        /// </summary>
        /// <param name="stepDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public bool SaveClientStepStatusDetail([FromBody]ClientStepDetailViewModel stepDetail)
        {
            return _clientDataBusiness.SaveClientStepStatusDetail(stepDetail);
        }

        /// <summary>
        /// To get the all the clients data to view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ClientsDataViewModel> GetAllClientsData()
        {
            return _clientDataBusiness.GetAllClientsData();
        }

        /// <summary>
        /// To get the active clients for a user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ClientsDataViewModel> GetActiveClientsForAUser()
        {
            return _clientDataBusiness.GetActiveClientsForAUser();
        }

        /// <summary>
        /// To get the active clients for a user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ClientViewModel> GetClientsForAUser()
        {
            return _clientDataBusiness.GetClientsByUser();
        }

        /// <summary>
        /// To activate client once all the steps completed
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ActivateClient(string clientCode)
        {
            return _clientDataBusiness.ActivateClient(clientCode);
        }

        /// <summary>
        /// Check for ClientCode already exists or not
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [HttpGet]
        public bool CheckForExistingClientCode(string clientCode)
        {
            return _clientDataBusiness.CheckForExistingClientCode(clientCode);
        }

        /// <summary>
        /// To get the all sites.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValueModel> GetAllSites()
        {
            return _clientDataBusiness.GetAllSites();
        }

        /// <summary>
        /// To get the client history.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpGet]
        public List<ClientHistoryViewModel> GetClientHistory(string clientCode, DateTime startDate, DateTime endDate)
        {
            return _clientDataBusiness.GetClientHistory(clientCode, startDate, endDate);
        }

        /// <summary>
        /// To get the client created date and created user.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin)]
        [HttpGet]
        public Dictionary<string, object> GetClientCreationDetails(string clientCode)
        {
            return _clientDataBusiness.GetClientCreationDetails(clientCode);
        }
    }

}