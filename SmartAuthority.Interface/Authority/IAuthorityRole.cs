/*技术支持QQ群：226090960*/
using System.Collections.Generic;

namespace SmartAuthority.Interface.Authority
{
    public interface IAuthorityRole
    {
        List<DTO.Authority.AuthorityRole> GetList(string Name, DTO.PagerParm PagerParm, out int TotalCount);

        DTO.Authority.AuthorityRole Get(int RoleId);

        List<DTO.Authority.AuthorityRole> GetByAccountId(int AccountId);

        bool Add(DTO.Authority.AuthorityRole Role, out int RoleId);

        bool Save(DTO.Authority.AuthorityRole Role);

        void Remove(int RoleId);
    }
}
