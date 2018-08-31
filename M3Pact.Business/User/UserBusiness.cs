using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.User;
using M3Pact.Infrastructure.Interfaces.Repository.User;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.User;
using System;
using System.Collections.Generic;

namespace M3Pact.Business.User
{
    public class UserBusiness : IUserBusiness
    {
        #region Internal Properties
        private IUserRepository _userRepo;
        private ILogger _logger;
        #endregion

        #region Constructor
        public UserBusiness(IUserRepository userRepo , ILogger logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }
        #endregion


        #region Public Methods

        /// <summary>
        /// To get the Screen Actions based on User role
        /// </summary>
        /// <param name="screenCode"></param>
        /// <returns></returns>
        public List<string> GetUserScreenActions(string screenCode)
        {
            List<string> screenActions = new List<string>(); 
            try
            {
                string role = UserHelper.getUserContext().Role;
                screenActions =  _userRepo.GetUserScreenActions(screenCode, role);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return screenActions;
        }

        /// <summary>
        /// To get the left nav menu based on role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public MenuItemViewModelList GetScreensOfNavMenuBasedOnRole()
        {
            MenuItemViewModelList menuItemViewModelList = new MenuItemViewModelList();
            try
            {
                string roleCode = UserHelper.getUserContext().Role;
                List<M3Pact.ViewModel.User.MenuItemViewModel> menuItemList = null;
                List<M3Pact.DomainModel.DomainModels.Screen> parentScreens = new List<M3Pact.DomainModel.DomainModels.Screen>();
                List<M3Pact.DomainModel.DomainModels.Screen> screenList = _userRepo.GetScreensOfNavMenuBasedOnRole(roleCode);
                if (screenList.Count > 0)
                {
                    menuItemList = new List<M3Pact.ViewModel.User.MenuItemViewModel>();
                    foreach (M3Pact.DomainModel.DomainModels.Screen screen in screenList)
                    {
                        if (screen.ParentScreenId == null)
                        {
                            parentScreens.Add(screen);
                        }
                    }
                    foreach (M3Pact.DomainModel.DomainModels.Screen screen in parentScreens)
                    {
                        M3Pact.ViewModel.User.MenuItemViewModel menuItem = new ViewModel.User.MenuItemViewModel();
                        menuItem.MenuItemViewModelId = screen.ScreenId;
                        menuItem.NodeName = screen.ScreenName;
                        menuItem.Url = screen.ScreenPath;
                        menuItem.Icon = screen.Icon;
                        menuItem.NodeId = screen.Displayorder;
                        menuItem.ParentId = screen.ParentScreenId;
                        menuItem.Info = screen.ScreenDescription;
                        menuItem.SubNodes = GetChildNodesOfParentNode(menuItem, screenList);
                        menuItemList.Add(menuItem);
                    }
                }
                menuItemViewModelList.ListOfMenuItemViewModel =  menuItemList;
                menuItemViewModelList.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                menuItemViewModelList.Success = false;
                menuItemViewModelList.IsExceptionOccured = true;
                menuItemViewModelList.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return menuItemViewModelList;
        }

        #endregion


        #region Private Methods.
        /// <summary>
        /// To get the child nodes of parent node.
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="screenList"></param>
        /// <returns></returns>
        private List<M3Pact.ViewModel.User.MenuItemViewModel> GetChildNodesOfParentNode(M3Pact.ViewModel.User.MenuItemViewModel menuItem, List<M3Pact.DomainModel.DomainModels.Screen> screenList)
        {
            List<M3Pact.ViewModel.User.MenuItemViewModel> subNodesList = new List<M3Pact.ViewModel.User.MenuItemViewModel>();
                foreach (M3Pact.DomainModel.DomainModels.Screen screen in screenList)
                {
                    if (menuItem.MenuItemViewModelId == screen.ParentScreenId)
                    {
                        M3Pact.ViewModel.User.MenuItemViewModel navMenuItem = new ViewModel.User.MenuItemViewModel();
                        navMenuItem.MenuItemViewModelId = screen.ScreenId;
                        navMenuItem.NodeName = screen.ScreenName;
                        navMenuItem.Url = screen.ScreenPath;
                        navMenuItem.Icon = screen.Icon;
                        navMenuItem.NodeId = screen.Displayorder;
                        navMenuItem.ParentId = screen.ParentScreenId;
                        navMenuItem.Info = screen.ScreenDescription;
                        navMenuItem.SubNodes = GetChildNodesOfParentNode(navMenuItem, screenList);
                        subNodesList.Add(navMenuItem);
                    }
                }
                return subNodesList;
        }
        #endregion
    }
}
