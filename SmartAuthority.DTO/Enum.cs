/*技术支持QQ群：226090960*/
namespace SmartAuthority.DTO
{
    public class Enum
    {
        public enum LoginStatus
        {
            AccountNotExist = 1,
            PasswordWrong = 2,
            Success = 3
        }

        public enum ChangePasswordStatus
        {
            OldPasswordWrong = 1,
            Success = 2
        }

        public enum AddAccountStatus
        {
            AccountNameExist = 1,
            Success = 2
        }
    }
}
