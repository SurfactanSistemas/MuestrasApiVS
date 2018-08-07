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
    public class PrecioLista
    {
        public string Cliente { get; set; }
        public string DesCliente { get; set; }
        public string CantTerminados { get; set; }
        public int Vendedor { get; set; }
        public string DesVendedor { get; set; }
    }

    public class Precio
    {
        public string Clave { get; set; }
        public string Cliente { get; set; }
        public string DesCliente { get; set; }
        public string Terminado { get; set; }
        public string DesTerminado { get; set; }
        public double Valor { get; set; }
        public int Vendedor { get; set; }
        public string DesVendedor { get; set; }
    }

    public class PreciosController : ApiController
    {
        // GET: api/Muestras
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //public List<Precio> Get()
        //{
        //    string WAnio = DateTime.Now.ToString("yyyy");

        //    return ResultadoComoJson("select e.Numero, e.Renglon, Producto = CASE WHEN e.TipoProDy = 'M' THEN e.ArticuloDy ELSE e.Articulo END, e.Cantidad, e.Precio, e.PrecioUs, e.Cliente, c.Razon, e.Paridad, e.Vendedor, DesVendedor = CASE WHEN e.Vendedor = 1 THEN 'Directo' ELSE o.Descripcion END , e.Fecha, e.DescriTerminadoII from Estadistica e LEFT OUTER JOIN Operador o ON e.Vendedor = o.Vendedor LEFT OUTER JOIN Cliente c ON e.Cliente = c.Cliente where e.DescriTerminadoII <> '' and e.OrdFecha >= '" + WAnio + "0101' and e.OrdFecha <= '" + WAnio + "1231'  order by e.Vendedor, e.Cliente, e.Numero, e.Renglon, Producto");
        //}

        private static List<Precio> ResultadoComoJson(string WConsulta)
        {
            //DataTable tabla = new DataTable();
            List<Precio> Muestras = new List<Precio>();

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
                            Muestras.Add(new Precio()
                            {
                                Vendedor = int.Parse(dr["Vendedor"].ToString()),
                                DesVendedor = dr["DesVendedor"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                DesCliente = dr["DesCliente"].ToString(),
                                Terminado = dr["Terminado"].ToString(),
                                DesTerminado = dr["DesTerminado"].ToString(),
                                Valor = double.Parse(dr["Precio"].ToString()),

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
        public List<PrecioLista> Get(string param)
        {
            //if (param == "99" && param2 == "") return this.Get();
            //if (param2 == "") param2 = DateTime.Now.ToString("yyyy");
            if (param == "") param = "99";

            string WFiltroVendedor = "AND c.Vendedor = '" + param + "'";

            if (param == "99") WFiltroVendedor = "";

            string WComienzo = (DateTime.Now.Year - 1).ToString();
            string WFinal = DateTime.Now.Year.ToString();

            string WConsulta = "SELECT p.Cliente, c. Razon as DesCliente, c.Vendedor, o.descripcion as DesVendedor, COUNT(p.Terminado) as CantidadTerminados FROM Precios p INNER JOIN Cliente c ON p.cliente = c.Cliente INNER JOIN Operador o ON c.Vendedor = o.Vendedor WHERE p.Cliente IN (SELECT e.Cliente FROM Estadistica e WHERE e.OrdFecha BETWEEN '" + WComienzo + "0101' and '" + WFinal + "1231') " + WFiltroVendedor + " GROUP BY c.Vendedor, o.Descripcion, p.Cliente, c.Razon ORDER BY o.Descripcion, c.Razon";

            List<PrecioLista> Muestras = new List<PrecioLista>();

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
                            Muestras.Add(new PrecioLista()
                            {
                                Vendedor = int.Parse(dr["Vendedor"].ToString()),
                                DesVendedor = dr["DesVendedor"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                DesCliente = dr["DesCliente"].ToString(),
                                CantTerminados = dr["CantidadTerminados"].ToString()
                            });
                        }
                    }

                }
            }

            return Muestras;
        }

        // GET: api/Muestras/{Cliente}
        public List<Precio> Get(string param, string param2)
        {
            //if (param == "99" && param2 == "") return this.Get();
            //if (param2 == "") param2 = DateTime.Now.ToString("yyyy");
            //if (param == "") param = "99";

            //string WFiltroVendedor = "AND c.Vendedor = '" + param + "'";

            //if (param == "99") WFiltroVendedor = "";

            string WConsulta = "select p.Clave, p.Terminado, p.Descripcion as DesTerminado, p.Cliente, c.Razon as DesCliente, c.Vendedor, o.Descripcion as DesVendedor, p.Precio, p.PrecioAnterior from Precios p INNER JOIN Cliente c ON p.Cliente = c.Cliente INNER JOIN Operador o ON c.Vendedor = o.Vendedor WHERE p.Cliente = '" + param2 + "' and c.Vendedor = '" + param + "' ORDER BY c.Vendedor, p.Cliente, p.Terminado";

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
