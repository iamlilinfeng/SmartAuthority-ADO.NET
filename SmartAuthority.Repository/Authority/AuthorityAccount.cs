/*技术支持QQ群：226090960*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SmartAuthority.Interface.Authority;

namespace SmartAuthority.Repository.Authority
{
    public class AuthorityAccount : IAuthorityAccount
    {
        public bool Add(DTO.Authority.AuthorityAccount authorityAccount)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@name", authorityAccount.Name),
               new SqlParameter("@real_name", authorityAccount.RealName),
               new SqlParameter("@password", authorityAccount.Password),
               new SqlParameter("@create_time", DateTime.Now),
               new SqlParameter("@enable", true)
            };
            string sql = string.Format(@"
                INSERT INTO [authority_account]
                       ([name]
                       ,[real_name]
                       ,[password]
                       ,[create_time]
                       ,[enable])
                 VALUES
                       (@name
                       ,@real_name
                       ,@password
                       ,@create_time
                       ,@enable)");
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList) > 0;
        }

        public DTO.Authority.AuthorityAccount GetByName(string Name)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@name", Name)
            };
            string sql = string.Format(@"SELECT * FROM authority_account WHERE name = @name and enable=1");
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds).SingleOrDefault();
        }

        public List<DTO.Authority.AuthorityAccount> GetList(string Name, DTO.PagerParm PagerParm, out int TotalCount)
        {
            string where = " [enable]=1 ";
            if (!string.IsNullOrEmpty(Name))
            {
                where += string.Format(" and [name] like '%{0}%'", Name);
            }
            DataSet ds = SqlCommon.GetList(Constant.CONNSTRING, " authority_account_id desc ", PagerParm.PageSize, PagerParm.CurrentCount, "authority_account", where, out TotalCount);
            return ConvertTo(ds);
        }

        public DTO.Authority.AuthorityAccount Get(int AccountId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_account_id", AccountId)
            };
            string sql = string.Format(@"SELECT * FROM authority_account WHERE authority_account_id = @authority_account_id");
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds).SingleOrDefault();
        }

        public bool AccountDisable(int AccountId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_account_id", AccountId)
            };
            string sql = string.Format(@"
                UPDATE [authority_account]
                   SET [enable] = 0
                 WHERE [authority_account_id] = @authority_account_id
            ");
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList) > 0;
        }

        public bool AccountEnable(int AccountId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_account_id", AccountId)
            };
            string sql = string.Format(@"
                UPDATE [authority_account]
                   SET [enable] = 1
                 WHERE [authority_account_id] = @authority_account_id
            ");
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList) > 0;
        }

        public bool ChangePassword(int AccountId, string NewPassword)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_account_id", AccountId),
               new SqlParameter("@password", NewPassword)
            };
            string sql = string.Format(@"
                UPDATE [authority_account]
                   SET [password] = @password
                 WHERE [authority_account_id] = @authority_account_id
            ");
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList) > 0;
        }

        public List<DTO.Authority.AuthorityAccount> GetInRole(int RoleId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@role_id", RoleId)
            };
            string sql = string.Format(@"
                SELECT *
                FROM [authority_account]
                WHERE authority_account_id IN
                    (SELECT authority_account_id
                     FROM authority_account_role
                     WHERE authority_role_id=@role_id)
            ", RoleId);
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds);
        }

        public List<DTO.Authority.AuthorityAccount> GetOutRole(int RoleId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@role_id", RoleId)
            };
            string sql = string.Format(@"
                SELECT *
                FROM [authority_account]
                WHERE authority_account_id not IN
                    (SELECT authority_account_id
                     FROM authority_account_role
                     WHERE authority_role_id=@role_id)
            ", RoleId);
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds);
        }

        public bool SaveRoleAccount(int RoleId, List<int> AccountIds)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int AccountId in AccountIds)
            {
                sb.AppendLine(string.Format(@"
                        INSERT INTO [authority_account_role]
                               ([authority_role_id]
                               ,[authority_account_id])
                         VALUES
                               ({1}
                               ,{2})
                ", RoleId, RoleId, AccountId));
            }
            string sql = string.Format(@"
                BEGIN try BEGIN transaction tr_add_role_account
                    DELETE FROM authority_account_role WHERE authority_role_id = {0}
                    {1}
                    COMMIT transaction tr_add_role_account 
                END try 
                BEGIN catch 
                    ROLLBACK transaction tr_add_role_account 
                END catch
            ", RoleId, sb.ToString());
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql) > 0;
        }

        #region 类型转换
        private List<DTO.Authority.AuthorityAccount> ConvertTo(DataSet ds)
        {
            List<DTO.Authority.AuthorityAccount> result = new List<DTO.Authority.AuthorityAccount>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DTO.Authority.AuthorityAccount item = new DTO.Authority.AuthorityAccount();
                    item.AuthorityAccountId = Convert.ToInt32(dr["authority_account_id"]);
                    item.Name = dr["name"].ToString();
                    item.RealName = dr["real_name"].ToString();
                    item.Password = dr["password"].ToString();
                    item.CreateTime = Convert.ToDateTime(dr["create_time"]);
                    item.Enable = Convert.ToBoolean(dr["enable"]);
                    result.Add(item);
                }
            }
            return result;
        }
        #endregion
    }
}
