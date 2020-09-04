using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Configuration;
using VietStar.Utility;

namespace VietStar.Repository
{
    public abstract class RepositoryBase
    {
        private IDbConnection _connection;
        protected readonly IConfiguration _configuration;
        protected int offset;
        public RepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("kingoffice"));
        }
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_configuration.GetConnectionString("kingoffice"));
            con.Open();
            return con;
        }
        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
        protected DynamicParameters GetParams<T>(T model, string[] ignoreKey = null, string outputParam = "", DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            if(!string.IsNullOrWhiteSpace(outputParam))
                p.Add(outputParam, dbType: type, direction: ParameterDirection.Output);
            var properties = model.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                var value = prop.GetValue(model);
                if (ignoreKey != null && ignoreKey.Contains(key))
                    continue;
                if (!string.IsNullOrWhiteSpace(outputParam))
                {
                    if (key.ToLower() == outputParam.ToLower())
                    {
                        continue;
                    }
                }
                if(!string.IsNullOrWhiteSpace(key))
                    p.Add(key, value);
            }
            return p;
        }
        protected void ProcessInputPaging(ref int page, ref int limit, out int offset)
        {
            page = page <= 0 ? 1 : page;
            if (limit <= 0)
                limit = 20;
            if (limit > 1000)
                limit = 100;
            offset = (page - 1) * limit;
        }
        protected string GetException(Exception e)
        {
            if (e == null)
                return string.Empty;
            return e.InnerException == null ? e.Dump() :e.InnerException.Dump();
        }
    }
}
