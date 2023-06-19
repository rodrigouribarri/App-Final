using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;


namespace TPFinalNivel2_RodrigoURIBARRI
{
    public partial class frmAltaModifica : Form
    {
        metodosHelper helper = new metodosHelper();
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAltaModifica()
        {
            InitializeComponent();
        }
        public frmAltaModifica(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar artículo";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                if (txtNombre.Text == "")
                {
                    MessageBox.Show("El campo Nombre no puede estar vacío");
                }
                else
                {
                    if (txtPrecio.Text == "")
                    {
                        MessageBox.Show("El campo Precio no puede estar vacío");
                    }
                    else
                    {
                        if (helper.soloNumeros(txtPrecio.Text) == true)
                        {
                            articulo.Precio = decimal.Parse(txtPrecio.Text);
                            if (helper.validarCaracteres(txtCodigo.Text, txtNombre.Text, txtDescripcion.Text, txtUrlImagen.Text) == true)
                            {
                                articulo.Codigo = txtCodigo.Text;
                                articulo.Nombre = txtNombre.Text;
                                articulo.Descripcion = txtDescripcion.Text;
                                articulo.UrlImagen = txtUrlImagen.Text;

                                if (articulo.Id != 0)
                                {
                                    negocio.modificar(articulo);
                                    MessageBox.Show("¡El artículo fue modificado exitosamente!");
                                }
                                else
                                {
                                    if (negocio.ExisteArticulo(articulo))
                                    {
                                        MessageBox.Show("Ya existe el artículo");
                                    }
                                    else
                                    {
                                        negocio.agregar(articulo);
                                        MessageBox.Show("¡El artículo fue agregado exitosamente!");
                                    }

                                }
                                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-catalogo-app"] + archivo.SafeFileName);

                                Close();

                            }    
                        }
                        else
                            MessageBox.Show("El campo precio solo admite números");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Program.frmArticulosPrincipal.Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
            Program.frmArticulosPrincipal.Show();
        }

        private void frmAltaModifica_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcanegocio = new MarcaNegocio();
            CategoriaNegocio categorianegocio = new CategoriaNegocio();
            try
            {
                cboCategoria.DataSource = categorianegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "";
                cboMarca.DataSource = marcanegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "";
                
                if(articulo != null)
                {
                    try
                    {
                        pcbArticulo.Load(articulo.UrlImagen);
                    }
                    catch (Exception)
                    {
                        pcbArticulo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBEcRjSYF6uc6TrgAIF5d_44nJwx03DYUOejh_Hf4&s");
                    }
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.UrlImagen;
                    txtPrecio.Text = articulo.Precio.ToString("0.00");
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnagregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg |*.jpg; |png |*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                pcbArticulo.Load(archivo.FileName);
            }
        }
    }
}
