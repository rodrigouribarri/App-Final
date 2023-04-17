using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using datos;
using System.Windows.Forms;

namespace negocio
{
    public class ArticuloNegocio
    {
        AccesoDatos datos = new AccesoDatos();
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
           // AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT A.Id Id, Codigo, Nombre, C.Descripcion Categoria, M.Descripcion Marca,Precio, A.Descripcion, ImagenUrl, IdMarca, IdCategoria from Articulos A, CATEGORIAS C, Marcas M where A.IdMarca = M.Id and A.IdCategoria = C.Id");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Descipcion = (string)datos.Lector["Categoria"];
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            //AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from articulos where Id = @id");
                datos.setearParametro("@id",id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void agregar(Articulo nuevo)
        {
            //AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values('"+nuevo.Codigo+"', '"+nuevo.Nombre+"', '"+ nuevo.Descripcion+"', @IdMarca, @IdCategoria, @UrlImagen, @Precio)");
                datos.setearParametro("@IdMarca", nuevo.Marca.Id);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@UrlImagen", nuevo.UrlImagen);
                datos.setearParametro("@Precio", nuevo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void modificar(Articulo modificado)
        {
            //AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, ImagenUrl = @urlimagen, IdMarca = @marca, IdCategoria = @categoria, Precio = @precio where Id = @id");
                datos.setearParametro("@codigo", modificado.Codigo);
                datos.setearParametro("@nombre", modificado.Nombre);
                datos.setearParametro("@descripcion", modificado.Descripcion);
                datos.setearParametro("@urlimagen", modificado.UrlImagen);
                datos.setearParametro("@marca", modificado.Marca.Id);
                datos.setearParametro("@categoria", modificado.Categoria.Id);
                datos.setearParametro("@precio", modificado.Precio);
                datos.setearParametro("@id", modificado.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.ejecutarAccion();
            }

        }
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            //AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id Id, Codigo, Nombre, C.Descripcion Categoria, M.Descripcion Marca,Precio, A.Descripcion, ImagenUrl, IdMarca, IdCategoria from Articulos A, CATEGORIAS C, Marcas M where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        case "Igual a":
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre" || campo == "Marca" || campo == "Código" || campo == "Categoría")
                {
                    string x = campo.ToString();
                    switch (x)
                    {
                        case "Marca":
                            x = "M.Descripcion";
                            break;
                        case "Código":
                            x = "Codigo";
                            break;
                        case "Categoría":
                            x = "C.Descripcion";
                            break;

                    }
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += x + " like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += x + " like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += x + " like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descipcion = (string)datos.Lector["Categoria"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }
    }
}
