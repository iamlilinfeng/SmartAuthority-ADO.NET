/*技术支持QQ群：226090960*/
using System.Collections.Generic;

namespace SmartAuthority.Interface.Authority
{
    public interface IAuthorityOperate
    {
        List<DTO.Authority.AuthorityOperate> GetList(string Name, DTO.PagerParm PagerParm, out int TotalCount);

        List<DTO.Authority.AuthorityOperate> GetList();

        bool SaveRoleOperate(int RoleId, List<int> OperateIds);

        List<DTO.Authority.AuthorityOperate> GetList(int RoleId);

        DTO.Authority.AuthorityOperate Get(int OperateId);

        bool Save(DTO.Authority.AuthorityOperate Operate);

        List<DTO.Authority.AuthorityOperate> GetOperateByAccountId(int AccountId);
    }
}
