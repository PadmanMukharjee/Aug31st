using M3Pact.Infrastructure.Enums;
using M3Pact.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.BusinessModel.Mapper
{
    public static class CommonMapper
    {
        #region public methods
        /// <summary>
        /// Converts BusinessResponse to ValidationViewModel
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ValidationViewModel ToValidationViewModel(this BusinessResponse response)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            if (response != null)
            {
                validationViewModel.Success = response.IsSuccess;
                validationViewModel.IsExceptionOccured = response.IsExceptionOccured;
                validationViewModel.AdditionalInfo = response.AdditionalInfo;
                if (response.BusinessRules != null && response.BusinessRules.Any())
                {
                    foreach (BusinessRuleDTO DTO in response.BusinessRules)
                    {
                        if (DTO.Messages != null && DTO.Messages.Any())
                        {
                            validationViewModel = MapMessages(validationViewModel, DTO.Messages);
                        }
                    }
                }
                if (response.Messages != null && response.Messages.Any())
                {
                    validationViewModel = MapMessages(validationViewModel, response.Messages);
                }
            }

            return validationViewModel;
        }
        #endregion public methods

        #region private methods

        /// <summary>
        /// Maps messages to appropriate MessageType
        /// </summary>
        /// <param name="validationViewModel"></param>
        /// <param name="dtoMessages"></param>
        /// <returns></returns>
        private static ValidationViewModel MapMessages(ValidationViewModel validationViewModel, List<MessageDTO> dtoMessages)
        {
            foreach (MessageDTO message in dtoMessages)
            {
                if (message.MessageType == MessageType.Error)
                {
                    validationViewModel.ErrorMessages.Add(message.Message);
                }
                else if (message.MessageType == MessageType.Warning)
                {
                    validationViewModel.WarningMessages.Add(message.Message);
                }
                else if (message.MessageType == MessageType.Info)
                {
                    validationViewModel.InfoMessages.Add(message.Message);
                }
                else if (message.MessageType == MessageType.RateAlert)
                {
                    validationViewModel.RateMessages.Add(message.Message);
                }
            }
            return validationViewModel;
        }

        #endregion private methods
    }
}
