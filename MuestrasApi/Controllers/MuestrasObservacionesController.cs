using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MuestrasApi.Controllers
{
    public class MuestraObs
    {
        public string Producto { get; set; }
        public string Observacion { get; set; }
        public int Pedido { get; set; }
    }

    public class Respuesta
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
    }

    public class MuestrasObservacionesController : ApiController
    {
        // GET: api/MuestrasObservaciones
        public string Get()
        {
            return "";
        }

        // GET: api/MuestrasObservaciones/Pedido/Producto
        public MuestraObs Get(string param, string param2)
        {
            MuestraObs m = new MuestraObs() { Producto= "", Observacion= "", Pedido= 0 };

            try
            {
                using (SqlConnection cn = new SqlConnection("workstation id=SurfactanSa.mssql.somee.com;packet size=4096;user id=fergthh_SQLLogin_1;pwd=nppge4uooo;data source=SurfactanSa.mssql.somee.com;persist security info=False;initial catalog=SurfactanSa"))
                {
                    cn.ConnectionString = ConfigurationManager.AppSettings["CS"];
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "SELECT Pedido, Observacion, Producto FROM MuestrasObservaciones WHERE Pedido = '" + param + "' AND Producto = '" + param2 + "'";

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();

                                m.Pedido = int.Parse(dr["Pedido"].ToString());
                                m.Observacion = dr["Observacion"].ToString();
                                m.Producto = dr["Producto"].ToString();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return m;
        }

        // POST: api/MuestrasObservaciones
        public Respuesta Post([FromBody] MuestraObs value)
        {
            Respuesta resp = new Respuesta() { Error = false, Msg = "" };

            string WObservacion = value.Observacion.ToString();

            try
            {
                if (WObservacion.Length > 200) WObservacion = WObservacion.Substring(0, 200);

                using (SqlConnection cn = new SqlConnection("workstation id=SurfactanSa.mssql.somee.com;packet size=4096;user id=fergthh_SQLLogin_1;pwd=nppge4uooo;data source=SurfactanSa.mssql.somee.com;persist security info=False;initial catalog=SurfactanSa"))
                {
                    cn.ConnectionString = ConfigurationManager.AppSettings["CS"];
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "SELECT Clave FROM MuestrasObservaciones WHERE Pedido = '" + value.Pedido + "' AND Producto = '" + value.Producto + "'";

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {

                                cmd.CommandText = "UPDATE MuestrasObservaciones SET Observacion = '" + WObservacion + "' WHERE Pedido = '" + value.Pedido + "' AND Producto = '" + value.Producto + "'";
                            }
                            else
                            {
                                string WClave = value.Pedido.ToString() + value.Producto.ToString().Trim();

                                cmd.CommandText = "INSERT INTO MuestrasObservaciones (Clave, Pedido, Observacion, Producto) VALUES ('" + WClave + "', '" + value.Pedido + "', '" + WObservacion + "', '" + value.Producto + "')";
                            }

                            if (!dr.IsClosed) dr.Close();

                            cmd.ExecuteNonQuery();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                resp.Error = true;
                resp.Msg = ex.Message;
            }

            return resp;
        }

        // PUT: api/MuestrasObservaciones/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MuestrasObservaciones/5
        public void Delete(int id)
        {
        }
    }
}
