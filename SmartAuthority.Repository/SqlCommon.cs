/*技术支持QQ群：226090960*/
using System;
using System.Data;
using System.Data.SqlClient;

namespace SmartAuthority.Repository
{
    public sealed class SqlCommon
    {
        public static DataSet GetList(string connectionString, string Order, int PageSize, int CurrentCount, string TableName, string Where, out int TotalCount)
        {
            SqlParameter[] parmList =
                {
                    new SqlParameter("@PageSize",PageSize),
                    new SqlParameter("@CurrentCount",CurrentCount),
                    new SqlParameter("@TableName",TableName),
                    new SqlParameter("@Where",Where),
                    new SqlParameter("@Order",Order),
                    new SqlParameter("@TotalCount",SqlDbType.Int,4)
                };
            parmList[5].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "prPager", parmList);
            TotalCount = Convert.ToInt32(parmList[5].Value);
            return ds;
        }
    }
}
