/*技术支持QQ群：226090960*/
using System;

namespace SmartAuthority.DTO.Authority
{
    public class AuthorityRole
    {
        public int AuthorityRoleId { get; set; }
        public string Name { get; set; }
        public string Describe { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Enable { get; set; }
    }
}
