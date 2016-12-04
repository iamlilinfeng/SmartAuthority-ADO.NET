/*技术支持QQ群：226090960*/
using System;

namespace SmartAuthority.DTO.Authority
{
    public class AuthorityOperate
    {
        public int AuthorityOperateId { get; set; }
        public string Name { get; set; }
        public string Control { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public bool Visible { get; set; }
        public int Order { get; set; }
        public int ParentId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Enable { get; set; }
        public int Level { get; set; }
    }

}
