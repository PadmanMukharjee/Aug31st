using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Payer;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Client;
using M3Pact.ViewModel.Common;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class PayerController : Controller
    {
        #region Internal Properties
        private IPayerBusiness _payerBusiness;
        #endregion Internal Properties

        #region Constructor
        public PayerController(IPayerBusiness payerBusiness)
        {
            _payerBusiness = payerBusiness;
        }
        #endregion Constructor


        #region Public Methods
        /// <summary>
        /// API call to return the Payers
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpGet]
        public PayerViewModelList GetPayers(bool fromClient = false)
        {
            return _payerBusiness.GetPayers(fromClient);
        }

        /// <summary>
        /// API call to return Active & Unassigned Payers For A Client
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpGet]
        public PayerViewModelList GetActivePayersToAssignForClient(string clientCode)
        {
            return _payerBusiness.GetActivePayersToAssignForClient(clientCode);
        }

        /// <summary>
        /// API call to Save the Payers
        /// </summary>
        /// <param name="payers"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpPost]
        public ValidationViewModel SavePayers([FromBody]List<PayerViewModel> payers)
        {
            return _payerBusiness.SavePayers(payers);
        }

        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User, Roles.Client)]
        [HttpGet]
        public PayerDataViewModelList GetTopPayers(string clientCode, int month, int year)
        {
            return _payerBusiness.GetTopPayersData(clientCode, month, year);
        }

        /// <summary>
        /// API call to return Payers Associated With A Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpGet]
        public ClientPayerViewModelList GetClientPayers(string clientCode)
        {
            return _payerBusiness.GetClientPayers(clientCode);
        }

        /// <summary>
        /// API call to Save(Insert/Update) Client Payer(s)
        /// </summary>
        /// <param name="clientPayers"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpPost]
        public ValidationViewModel SaveClientPayers([FromBody]List<ClientPayerViewModel> clientPayers)
        {
            return _payerBusiness.SaveClientPayers(clientPayers);
        }

        /// <summary>
        /// To get the clients assigned to payer.
        /// </summary>
        /// <param name="payerCode"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpGet]
        public StringList GetClientsAssignedtoPayer(string payerCode)
        {
            return _payerBusiness.GetClientsAssignedtoPayer(payerCode);
        }

        /// <summary>
        /// To active or inactive the payer.
        /// </summary>
        /// <param name="payer"></param>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpPost]
        public ValidationViewModel ActivateOrDeactivatePayer([FromBody] PayerViewModel payer)
        {
            return _payerBusiness.ActivateOrDeactivatePayer(payer);
        }

        /// <summary>
        /// To activate or inactivate client payer
        /// </summary>
        /// <param name="clientPayer"></param>
        [AuthorizationFilter(Roles.Admin, Roles.Manager, Roles.Executive, Roles.User)]
        [HttpPost]
        public ValidationViewModel ActivateOrDeactivateClientPayer([FromBody] ClientPayerViewModel clientPayer)
        {
            return _payerBusiness.ActivateOrDeactivateClientPayer(clientPayer);
        }
        #endregion Public Methods
    }
}