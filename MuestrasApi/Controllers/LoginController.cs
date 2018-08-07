using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MuestrasApi.Controllers
{
    public class LoginController : ApiController
    {
        // GET: api/Muestras
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public IEnumerable<DataRow> Get()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Vendedor");
            tabla.Rows.Add("0");
            return tabla.AsEnumerable().ToList();
        }

        private static IEnumerable<DataRow> ResultadoComoJson(string WConsulta)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = new SqlConnection("workstation id=SurfactanSa.mssql.somee.com;packet size=4096;user id=fergthh_SQLLogin_1;pwd=nppge4uooo;data source=SurfactanSa.mssql.somee.com;persist security info=False;initial catalog=SurfactanSa"))
            {
                cn.ConnectionString = ConfigurationManager.AppSettings["CS"];
                cn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = WConsulta;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tabla.Load(dr);
                    }

                    return tabla.AsEnumerable().ToList();
                }
            }
        }

        // GET: api/Muestras/5
        public IEnumerable<DataRow> Get(string param)
        {
            string WClave = param.Trim();

            if (WClave == "") return this.Get();

            string WConsulta = "SELECT Vendedor FROM Operador WHERE Vendedor <> 0 AND UPPER(Clave) = '" + WClave + "'";

            return ResultadoComoJson(WConsulta);
        }

        // POST: api/Muestras
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Muestras/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Muestras/5
        public void Delete(int id)
        {
        }
    }
}
