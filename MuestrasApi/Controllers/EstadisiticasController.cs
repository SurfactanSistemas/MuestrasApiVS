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
    public class Estadistica
    {
        public int Numero { get; set; }
        public string Producto { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double PrecioUs { get; set; }
        public string Cliente { get; set; }
        public string DesCliente { get; set; }
        public double Paridad { get; set; }
        public int Vendedor { get; set; }
        public string DesVendedor { get; set; }
        public string Fecha { get; set; }
        public string DescTerminado { get; set; }
    }

    public class EstadisticasController : ApiController
    {
        // GET: api/Muestras
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public List<Estadistica> Get()
        {
            string WAnio = DateTime.Now.ToString("yyyy");

            return ResultadoComoJson("select e.Numero, e.Renglon, Producto = CASE WHEN e.TipoProDy = 'M' THEN e.ArticuloDy ELSE e.Articulo END, e.Cantidad, e.Precio, e.PrecioUs, e.Cliente, c.Razon, e.Paridad, e.Vendedor, DesVendedor = CASE WHEN e.Vendedor = 1 THEN 'Directo' ELSE o.Descripcion END , e.Fecha, e.DescriTerminadoII from Estadistica e LEFT OUTER JOIN Operador o ON e.Vendedor = o.Vendedor LEFT OUTER JOIN Cliente c ON e.Cliente = c.Cliente where e.DescriTerminadoII <> '' and e.OrdFecha >= '" + WAnio + "0101' and e.OrdFecha <= '" + WAnio + "1231'  order by e.Vendedor, e.Cliente, e.Numero, e.Renglon, Producto");
        }

        private static List<Estadistica> ResultadoComoJson(string WConsulta)
        {
            //DataTable tabla = new DataTable();
            List<Estadistica> Muestras = new List<Estadistica>();

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
                            Muestras.Add(new Estadistica()
                            {
                                Numero = int.Parse(dr["Numero"].ToString()),
                                Producto = dr["Producto"].ToString(),
                                Cantidad = double.Parse(dr["Cantidad"].ToString()),
                                Precio = double.Parse(dr["Precio"].ToString()),
                                PrecioUs = double.Parse(dr["PrecioUs"].ToString()),
                                Cliente = dr["Cliente"].ToString(),
                                DesCliente = dr["Razon"].ToString(),
                                Paridad = double.Parse(dr["Paridad"].ToString()),
                                Vendedor = int.Parse(dr["Vendedor"].ToString()),
                                DesVendedor = dr["DesVendedor"].ToString(),
                                Fecha = dr["Fecha"].ToString(),
                                DescTerminado = dr["DescriTerminadoII"].ToString()

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
        public List<Estadistica> Get(string param, string param2)
        {
            if (param == "99" && param2 == "") return this.Get();
            if (param2 == "") param2 = DateTime.Now.ToString("yyyy");
            if (param == "") param = "99";

            string WFiltroVendedor = "AND e.Vendedor = '" + param + "'";

            if (param == "99") WFiltroVendedor = "";

            string WConsulta = "select e.Numero, e.Renglon, Producto = CASE WHEN e.TipoProDy = 'M' THEN e.ArticuloDy ELSE e.Articulo END, e.Cantidad, e.Precio, e.PrecioUs, e.Cliente, c.Razon, e.Paridad, e.Vendedor, DesVendedor = CASE WHEN e.Vendedor = 1 THEN 'Directo' ELSE o.Descripcion END , e.Fecha, e.DescriTerminadoII from Estadistica e LEFT OUTER JOIN Operador o ON e.Vendedor = o.Vendedor LEFT OUTER JOIN Cliente c ON e.Cliente = c.Cliente where e.DescriTerminadoII <> '' and e.OrdFecha >= '" + param2 + "0101' and e.OrdFecha <= '" + param2 +"1231' " + WFiltroVendedor + " order by e.Vendedor, e.Cliente, e.Numero, e.Renglon, Producto";

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
