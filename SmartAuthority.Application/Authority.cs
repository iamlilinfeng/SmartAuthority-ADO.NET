/*技术支持QQ群：226090960*/
using System;
using System.Collections.Generic;
using SmartAuthority.Interface.Authority;
using SmartAuthority.Util;
using System.Configuration;

namespace SmartAuthority.Application
{
    public class Authority
    {
        public IAuthorityAccount accountRepository;
        public IAuthorityOperate operateRepository;
        public IAuthorityRole roleRepository;
        public static readonly string SecurityKey = ConfigurationManager.AppSettings["SecurityKey"];

        public Authority(IAuthorityAccount _accountRepository, IAuthorityOperate _operateRepository, IAuthorityRole _roleRepository)
        {
            this.accountRepository = _accountRepository;
            this.operateRepository = _operateRepository;
            this.roleRepository = _roleRepository;
        }

        public DTO.Enum.AddAccountStatus AddAccount(DTO.Authority.AuthorityAccount authorityAccount)
        {
            try
            {
                var Account = accountRepository.GetByName(authorityAccount.Name);
                if (Account != null)
                {
                    return DTO.Enum.AddAccountStatus.AccountNameExist;
                }

                authorityAccount.Password = SecurityHelper.Encrypt(authorityAccount.Password.Trim(), SecurityKey);

                if (accountRepository.Add(authorityAccount))
                {
                    return DTO.Enum.AddAccountStatus.Success;
                }
                else
                {
                    LogHelper.Error("addaccount false. {0}", authorityAccount.ToJson());
                    return DTO.Enum.AddAccountStatus.AccountNameExist;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", authorityAccount.ToJson());
                return DTO.Enum.AddAccountStatus.AccountNameExist;
            }
        }

        public DTO.Enum.LoginStatus Login(string AccountName, string Password, out int AccountId)
        {
            AccountId = 0;
            try
            {
                var Account = accountRepository.GetByName(AccountName);
                if (Account == null)
                {
                    return DTO.Enum.LoginStatus.AccountNotExist;
                }
                if (SecurityHelper.Decrypt(Account.Password, SecurityKey) != Password.Trim())
                {
                    return DTO.Enum.LoginStatus.PasswordWrong;
                }
                AccountId = Account.AuthorityAccountId;
                return DTO.Enum.LoginStatus.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", AccountName, Password);
                return DTO.Enum.LoginStatus.PasswordWrong;
            }
        }

        public DTO.Authority.Account GetBaseAccount(int AccountId)
        {
            try
            {
                var result = new DTO.Authority.Account();
                var Account = accountRepository.Get(AccountId);
                result.AccouontId = Account.AuthorityAccountId;
                result.Name = Account.RealName;
                result.Roles = roleRepository.GetByAccountId(AccountId);
                result.Operates = operateRepository.GetOperateByAccountId(AccountId);
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", AccountId);
                return null;
            }
        }

        public DTO.Authority.AuthorityAccount GetAccountById(int AccountId)
        {
            try
            {
                return accountRepository.Get(AccountId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", AccountId);
                return null;
            }
        }

        public List<DTO.Authority.AuthorityAccount> GetAccountList(string Name, DTO.PagerParm PagerParm, out int TotalCount)
        {
            try
            {
                return accountRepository.GetList(Name, PagerParm, out TotalCount);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", Name, PagerParm.ToJson());
                TotalCount = 0;
                return new List<DTO.Authority.AuthorityAccount>();
            }
        }

        public DTO.Enum.ChangePasswordStatus ChangePassword(int AccountId, string OldPassword, string NewPassword)
        {
            try
            {
                if (SecurityHelper.Decrypt(GetAccountById(AccountId).Password, SecurityKey) != OldPassword.Trim())
                {
                    return DTO.Enum.ChangePasswordStatus.OldPasswordWrong;
                }

                if (accountRepository.ChangePassword(AccountId, SecurityHelper.Encrypt(NewPassword, SecurityKey)))
                {
                    return DTO.Enum.ChangePasswordStatus.Success;
                }
                else
                {
                    LogHelper.Error("changepassword false. {0},{1},{2}", AccountId, OldPassword, NewPassword);
                    return DTO.Enum.ChangePasswordStatus.OldPasswordWrong;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", AccountId, NewPassword);
                return DTO.Enum.ChangePasswordStatus.OldPasswordWrong;
            }
        }

        public bool AccountDisable(int AccountId)
        {
            try
            {
                return accountRepository.AccountDisable(AccountId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", AccountId);
                return false;
            }
        }

        public bool AccountEnable(int AccountId)
        {
            try
            {
                return accountRepository.AccountEnable(AccountId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", AccountId);
                return false;
            }
        }

        public List<DTO.Authority.AuthorityAccount> GetInRoleAccount(int RoleId)
        {
            try
            {
                return accountRepository.GetInRole(RoleId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", RoleId);
                return new List<DTO.Authority.AuthorityAccount>();
            }
        }

        public List<DTO.Authority.AuthorityAccount> GetOutRoleAccount(int RoleId)
        {
            try
            {
                return accountRepository.GetOutRole(RoleId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", RoleId);
                return new List<DTO.Authority.AuthorityAccount>();
            }
        }

        public bool SaveRoleAccount(int RoleId, List<int> AccountIds)
        {
            try
            {
                return accountRepository.SaveRoleAccount(RoleId, AccountIds);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", RoleId, AccountIds.ToJson());
                return false;
            }
        }

        public List<DTO.Authority.AuthorityRole> GetRoleList(string Name, DTO.PagerParm PagerParm, out int TotalCount)
        {
            try
            {
                return roleRepository.GetList(Name, PagerParm, out TotalCount);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", Name, PagerParm.ToJson());
                TotalCount = 0;
                return new List<DTO.Authority.AuthorityRole>();
            }
        }

        public DTO.Authority.AuthorityRole GetRoleById(int RoleId)
        {
            try
            {
                return roleRepository.Get(RoleId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", RoleId);
                return new DTO.Authority.AuthorityRole();
            }
        }

        public bool AddRole(DTO.Authority.AuthorityRole Role, List<int> OperateIds)
        {
            int RoleId = 0;
            try
            {
                if (roleRepository.Add(Role, out RoleId))
                {
                    return operateRepository.SaveRoleOperate(RoleId, OperateIds);
                }
                else
                {
                    LogHelper.Error("add role false. {0},{1}", Role.ToJson(), OperateIds.ToJson());
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", Role.ToJson(), OperateIds.ToJson());
                try
                {
                    //when error then rollback
                    roleRepository.Remove(RoleId);
                }
                catch (Exception innerEx)
                {
                    LogHelper.Exception(innerEx, "{0}", RoleId);
                }
                return false;
            }
        }

        public bool SaveRole(DTO.Authority.AuthorityRole Role, List<int> OperateIds)
        {
            try
            {
                roleRepository.Save(Role);
                return operateRepository.SaveRoleOperate(Role.AuthorityRoleId, OperateIds);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", Role.ToJson(), OperateIds.ToJson());
                return false;
            }
        }

        public List<DTO.Authority.AuthorityOperate> GetOperateList(string Name, DTO.PagerParm PagerParm, out int TotalCount)
        {
            try
            {
                return operateRepository.GetList(Name, PagerParm, out TotalCount);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0},{1}", Name, PagerParm.ToJson());
                TotalCount = 0;
                return new List<DTO.Authority.AuthorityOperate>();
            }
        }

        public List<DTO.Authority.AuthorityOperate> GetAllOperate()
        {
            try
            {
                return operateRepository.GetList();
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex);
                return new List<DTO.Authority.AuthorityOperate>();
            }
        }

        public List<DTO.Authority.AuthorityOperate> GetOperateListByRole(int RoleId)
        {
            try
            {
                return operateRepository.GetList(RoleId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", RoleId);
                return new List<DTO.Authority.AuthorityOperate>();
            }
        }

        public DTO.Authority.AuthorityOperate GetOperateById(int OperateId)
        {
            try
            {
                return operateRepository.Get(OperateId);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", OperateId);
                return new DTO.Authority.AuthorityOperate();
            }
        }

        public bool SaveOperate(DTO.Authority.AuthorityOperate operate)
        {
            try
            {
                return operateRepository.Save(operate);
            }
            catch (Exception ex)
            {
                LogHelper.Exception(ex, "{0}", operate.ToJson());
                return false;
            }
        }
    }
}
