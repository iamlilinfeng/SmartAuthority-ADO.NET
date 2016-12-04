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
    public class AuthorityOperate : IAuthorityOperate
    {
        public List<DTO.Authority.AuthorityOperate> GetList(string Name, DTO.PagerParm PagerParm, out int TotalCount)
        {
            string where = " 1=1 ";
            if (!string.IsNullOrEmpty(Name))
            {
                where += string.Format(" and [name] like '%{0}%'", Name);
            }
            DataSet ds = SqlCommon.GetList(Constant.CONNSTRING, " authority_operate_id desc ", PagerParm.PageSize, PagerParm.CurrentCount, "authority_operate", where, out TotalCount);
            return ConvertTo(ds);
        }

        public List<DTO.Authority.AuthorityOperate> GetList()
        {
            string sql = "select * from authority_operate(nolock) ";
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql);
            return ConvertTo(ds);
        }

        public bool SaveRoleOperate(int RoleId, List<int> OperateIds)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int OperateId in OperateIds)
            {
                sb.AppendLine(string.Format(@"
                        INSERT INTO [authority_role_operate]
                               ([authority_role_id]
                               ,[authority_operate_id])
                         VALUES
                               ({0}
                               ,{1})
                ", RoleId, OperateId));
            }
            string sql = string.Format(@"
                BEGIN try BEGIN transaction tr_add_role_operate
                    DELETE FROM authority_role_operate WHERE authority_role_id = {0}
                    {1}
                    COMMIT transaction tr_add_role_operate 
                END try 
                BEGIN catch 
                    ROLLBACK transaction tr_add_role_operate 
                END catch
            ", RoleId, sb.ToString());
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql) > 0;
        }

        public List<DTO.Authority.AuthorityOperate> GetList(int RoleId)
        {
            string sql = string.Format(@"
                SELECT *
                FROM authority_operate(nolock)
                WHERE authority_operate_id IN
                    (SELECT authority_operate_id
                     FROM authority_role_operate(nolock)
                     WHERE authority_role_id={0})
            ", RoleId);
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql);
            return ConvertTo(ds);
        }

        public DTO.Authority.AuthorityOperate Get(int OperateId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_operate_id", OperateId)
            };
            string sql = string.Format(@"
                select * from [authority_operate](nolock) where authority_operate_id = @authority_operate_id
            ");
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds).FirstOrDefault();
        }

        public bool Save(DTO.Authority.AuthorityOperate Operate)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_operate_id", Operate.AuthorityOperateId),
               new SqlParameter("@name", Operate.Name),
               new SqlParameter("@order", Operate.Order)
            };
            string sql = string.Format(@"
                update [authority_operate] set [name] = @name,[order] = @order where authority_operate_id = @authority_operate_id
            ");
            return SqlHelper.ExecuteNonQuery(Constant.CONNSTRING, CommandType.Text, sql, parmList) > 0;
        }

        public List<DTO.Authority.AuthorityOperate> GetOperateByAccountId(int AccountId)
        {
            SqlParameter[] parmList = new SqlParameter[]{
               new SqlParameter("@authority_account_id", AccountId)
            };
            string sql = string.Format(@"
             SELECT *
                FROM authority_operate(nolock)
                WHERE authority_operate_id IN
                    (SELECT authority_operate_id
                     FROM authority_role_operate(nolock)
                     WHERE authority_role_id IN
                         (SELECT authority_role_id
                          FROM authority_account_role(nolock)
                          WHERE authority_account_id=@authority_account_id))");
            DataSet ds = SqlHelper.ExecuteDataset(Constant.CONNSTRING, CommandType.Text, sql, parmList);
            return ConvertTo(ds);
        }

        #region 类型转换
        private List<DTO.Authority.AuthorityOperate> ConvertTo(DataSet ds)
        {
            List<DTO.Authority.AuthorityOperate> result = new List<DTO.Authority.AuthorityOperate>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DTO.Authority.AuthorityOperate item = new DTO.Authority.AuthorityOperate();
                    item.AuthorityOperateId = Convert.ToInt32(dr["authority_operate_id"]);
                    item.Name = dr["name"].ToString();
                    item.Control = dr["control"].ToString();
                    item.Action = dr["action"].ToString();
                    item.Icon = dr["icon"].ToString();
                    item.Visible = Convert.ToBoolean(dr["visible"]);
                    item.Order = Convert.ToInt32(dr["order"]);
                    item.CreateTime = Convert.ToDateTime(dr["create_time"]);
                    item.Enable = Convert.ToBoolean(dr["enable"]);
                    item.ParentId = Convert.ToInt32(dr["parent_id"]);
                    item.Level = Convert.ToInt32(dr["level"]);
                    result.Add(item);
                }
            }
            return result;
        }
        #endregion
    }
}
