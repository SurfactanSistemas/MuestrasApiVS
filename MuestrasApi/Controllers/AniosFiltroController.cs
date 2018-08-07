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
    public class Anio
    {
        public string Valor { get; set; }
    }
    public class AniosFiltroController : ApiController
    {
        // GET: api/Muestras
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public List<Anio> Get()
        {
            return ResultadoComoJson("select distinct Anio = LEFT(ordfecha, 4) from Muestra where OrdFecha > '20030101' order by Anio desc");
        }

        private static List<Anio> ResultadoComoJson(string WConsulta)
        {
            //DataTable tabla = new DataTable();
            List<Anio> Anios = new List<Anio>();

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
                        while (dr.Read())
                        {
                            Anios.Add(new Anio() {
                                Valor = dr["Anio"].ToString()
                            });
                        }
                    }

                    //EnumerableRowCollection<DataRow> en = tabla.AsEnumerable();

                    //List<DataRow> r = en.ToList<DataRow>();

                    return Anios;
                }
            }
        }

        // GET: api/Muestras/5
        public List<Anio> Get(string param)
        {
            if (param == "99") return this.Get();

            string WConsulta = "select distinct Anio = LEFT(ordfecha, 4) from Muestra where OrdFecha > '20030101' and Vendedor = '" + param + "' order by Anio desc";

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
