/*技术支持QQ群：226090960*/
using System.Collections.Generic;

namespace SmartAuthority.Interface.Authority
{
    public interface IAuthorityAccount
    {
        bool Add(DTO.Authority.AuthorityAccount authorityAccount);

        DTO.Authority.AuthorityAccount GetByName(string Name);

        List<DTO.Authority.AuthorityAccount> GetList(string Name, DTO.PagerParm PagerParm, out int TotalCount);

        DTO.Authority.AuthorityAccount Get(int AccountId);

        bool AccountDisable(int AccountId);

        bool AccountEnable(int AccountId);

        bool ChangePassword(int AccountId, string NewPassword);

        List<DTO.Authority.AuthorityAccount> GetInRole(int RoleId);

        List<DTO.Authority.AuthorityAccount> GetOutRole(int RoleId);

        bool SaveRoleAccount(int RoleId, List<int> AccountIds);
    }
}
