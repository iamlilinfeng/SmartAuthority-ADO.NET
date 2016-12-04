/*技术支持QQ群：226090960*/
using System;

namespace SmartAuthority.DTO.Authority
{
    public class AuthorityAccount
    {
        public int AuthorityAccountId { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Enable { get; set; }
    }

}
