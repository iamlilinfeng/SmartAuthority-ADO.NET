/*技术支持QQ群：226090960*/
using System.Collections.Generic;

namespace SmartAuthority.DTO.Authority
{
    public class Account
    {
        public int AccouontId { get; set; }
        public string Name { get; set; }
        public List<AuthorityRole> Roles { get; set; }
        public List<AuthorityOperate> Operates { get; set; }
    }
}
