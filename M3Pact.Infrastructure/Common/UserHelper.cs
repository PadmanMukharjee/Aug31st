namespace M3Pact.Infrastructure.Common
{
    public class UserHelper
    {
        private static UserContext _userContext;
        /// <summary>
        /// To create singleton instances of usercontext
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public static void Instance(UserContext userContext = null)
        {
            if (userContext != null)
            {
                _userContext = userContext;
            }
        }
        /// <summary>
        /// To get the User clams details
        /// </summary>
        /// <returns></returns>
        public static UserContext getUserContext()
        {
            return _userContext;
        }
    }
}
