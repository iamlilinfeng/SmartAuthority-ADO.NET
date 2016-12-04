/*技术支持QQ群：226090960*/
namespace SmartAuthority.Factory
{
    public class ApplicationFactory
    {
        public static Application.Authority GetAuthorityApplication()
        {
            return new Application.Authority(new Repository.Authority.AuthorityAccount(), new Repository.Authority.AuthorityOperate(), new Repository.Authority.AuthorityRole());
        }
    }
}
