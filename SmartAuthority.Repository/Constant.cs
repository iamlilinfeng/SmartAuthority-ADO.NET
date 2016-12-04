/*技术支持QQ群：226090960*/
using System.Configuration;

namespace SmartAuthority.Repository
{
    public class Constant
    {
        public static string CONNSTRING = ConfigurationManager.ConnectionStrings["Conn"].ToString();
    }
}
