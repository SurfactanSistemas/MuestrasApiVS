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
    public class Muestra
    {
        public string Producto { get; set; }
        public string Cantidad { get; set; }
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        public string Razon { get; set; }
        public string DesProducto { get; set; }
        public int Pedido { get; set; }
        public int Vendedor { get; set; }
        public string DesVendedor { get; set; }
        public string Remito { get; set; }
    }

    public class MuestrasController : ApiController
    {
        // GET: api/Muestras
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public List<Muestra> Get()
        {
            string WAnio = DateTime.Now.ToString("yyyy");

            return ResultadoComoJson("SELECT Producto = CASE p.TipoPro WHEN 'T' THEN m.Producto ELSE m.Articulo END, RTRIM(LTRIM(m.Cantidad)) Cantidad, m.Fecha, m.Cliente, RTRIM(LTRIM(m.Razon)) Razon, RTRIM(LTRIM(m.DescriCliente)) as DesProducto, m.Pedido, m.Vendedor, LTRIM(RTRIM(ISNULL(m.DesVendedor, ''))) DesVendedor, ISNULL(m.Remito, '') Remito from Muestra m INNER JOIN Pedido p ON m.ClavePedido = p.Clave WHERE m.OrdFecha >= '" + WAnio + "0101' AND m.OrdFecha <= '" + WAnio + "1231' order by DesVendedor, m.Razon");
        }

        private static List<Muestra> ResultadoComoJson(string WConsulta)
        {
            //DataTable tabla = new DataTable();
            List<Muestra> Muestras = new List<Muestra>();

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
                            Muestras.Add(new Muestra() {
                                Pedido = int.Parse(dr["Pedido"].ToString()),
                                Vendedor = int.Parse(dr["Vendedor"].ToString()),
                                Producto = dr["Producto"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                Fecha = dr["Fecha"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                Razon = dr["Razon"].ToString(),
                                DesProducto = dr["DesProducto"].ToString(),
                                DesVendedor = dr["DesVendedor"].ToString(),
                                Remito = dr["Remito"].ToString()
                            });
                        }
                    }

                    //EnumerableRowCollection<DataRow> en = tabla.AsEnumerable();

                    //List<DataRow> r = en.ToList<DataRow>();

                    return Muestras;
                }
            }
        }

        // GET: api/Muestras/5
        public List<Muestra> Get(string param, string param2)
        {
            if (param == "99" && param2 == "") return this.Get();
            if (param2 == "") param2 = DateTime.Now.ToString("yyyy");
            if (param == "") param = "99";

            string WFiltroVendedor = "AND m.Vendedor = '" + param + "'";

            if (param == "99") WFiltroVendedor = "";

            string WConsulta = "SELECT Producto = CASE p.TipoPro WHEN 'T' THEN m.Producto ELSE m.Articulo END, RTRIM(LTRIM(m.Cantidad)) Cantidad, m.Fecha, m.Cliente, RTRIM(LTRIM(m.Razon)) Razon, RTRIM(LTRIM(m.DescriCliente)) as DesProducto, m.Pedido, m.Vendedor, LTRIM(RTRIM(ISNULL(m.DesVendedor, ''))) DesVendedor, ISNULL(m.Remito, '') Remito from Muestra m LEFT JOIN Pedido p ON m.ClavePedido = p.Clave WHERE m.OrdFecha >= '" + param2 + "0101' AND m.OrdFecha <= '" + param2 + "1231' " + WFiltroVendedor + " order by m.Vendedor, m.Razon";

            return ResultadoComoJson(WConsulta);
        }

        // POST: api/Muestras
        public void Post(MuestraObs value)
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
