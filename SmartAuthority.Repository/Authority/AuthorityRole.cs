/*技术支持QQ群：226090960*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SmartAuthority.Interface.Authority;

namespace SmartAuthority.Repository.Authority
{
    public class AuthorityRole : IAuthorityRole
    {
        public List<DTO.Authority.AuthorityRole> GetList(string Name, DTO.PagerParm PagerParm, out int TotalCount)
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(Name))
            {
                where += string.Format(" and [name] like '%{0}%'", Name);
            }
            DataSet ds = SqlCommon.GetList(Constant.CONNSTRING, " authority_role_id desc ", PagerParm.PageSize, PagerParm.CurrentCount, "authority_role", where, out TotalCount);
            return ConvertTo(ds);
        }

        public DTO.Authority.AuthorityRole Get(int RoleId)
        {
            string sql = string.Format("select * from  authority_role where [authority_role_id] = {0}", RoleId);
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql);
            return ConvertTo(ds).FirstOrDefault();
        }

        public List<DTO.Authority.AuthorityRole> GetByAccountId(int AccountId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_account_id", AccountId)
            };

            string sql = string.Format(@"
                    select * from [dbo].[authority_role] where authority_role_id in( 
                    select authority_role_id from [dbo].[authority_account_role] where authority_account_id=@authority_account_id
                    )");
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds);
        }

        public bool Add(DTO.Authority.AuthorityRole Role, out int RoleId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@name", Role.Name),
               new SqlParameter("@describe", Role.Describe),
               new SqlParameter("@create_time", DateTime.Now),
               new SqlParameter("@enable", true)
            };

            string sql = string.Format(@"
                INSERT INTO [authority_role]
                       ([name]
                       ,[describe]
                       ,[create_time]
                       ,[enable])
                 VALUES
                       (@name
                       ,@describe
                       ,@create_time
                       ,@enable)
                 SELECT @@IDENTITY;"
                );
            return (RoleId = (int)SqlHelper.ExecuteScalar(Constant.CONNSTRING, CommandType.Text, sql, parmList)) > 0;
        }

        public bool Save(DTO.Authority.AuthorityRole Role)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@name", Role.Name),
               new SqlParameter("@describe", Role.Describe),
               new SqlParameter("@authority_role_id", Role.AuthorityRoleId)
            };

            string sql = string.Format(@"
                 UPDATE [authority_role]
                   SET [name] = @name
                      ,[describe] = @describe
                 WHERE [authority_role_id] = @authority_role_id"
                );
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList) > 0;
        }

        public void Remove(int RoleId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@role_id", RoleId)
            };

            string sql = string.Format(@"
                 DELETE FROM [authority_role] WHERE [authority_role_id] = @authority_role_id"
                );
            SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList);
        }

        #region 类型转换
        private List<DTO.Authority.AuthorityRole> ConvertTo(DataSet ds)
        {
            List<DTO.Authority.AuthorityRole> result = new List<DTO.Authority.AuthorityRole>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DTO.Authority.AuthorityRole item = new DTO.Authority.AuthorityRole();
                    item.AuthorityRoleId = Convert.ToInt32(dr["authority_role_id"]);
                    item.Name = dr["name"].ToString();
                    item.Describe = dr["describe"].ToString();
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
