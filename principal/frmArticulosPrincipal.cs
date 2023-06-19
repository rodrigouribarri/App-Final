using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_RodrigoURIBARRI
{
    public partial class frmArticulosPrincipal : Form
    {
        private List<Articulo> listaArticulos;
        private List<Articulo> listaFiltrada;
        metodosHelper helper = new metodosHelper();
        public frmArticulosPrincipal()
        {
            InitializeComponent();
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listar();
                dgvArticulos.DataSource = listaArticulos;
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "C2";
                ocultarColumnas();
                pcbArticulos.Load(listaArticulos[0].UrlImagen);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void ocultarColumnas()
        {
            if(dgvArticulos.DataSource != null)
            {
                dgvArticulos.Columns["Descripcion"].Visible = false;
                dgvArticulos.Columns["UrlImagen"].Visible = false;
                dgvArticulos.Columns["Id"].Visible = false;
            }
        }

        private void frmArticulosPrincipal_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Código");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoría");
        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                try
                {
                    pcbArticulos.Load(seleccionado.UrlImagen);
                }
                catch (Exception)
                {
                    pcbArticulos.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBEcRjSYF6uc6TrgAIF5d_44nJwx03DYUOejh_Hf4&s");
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Program.frmArticulosPrincipal.Hide();
            frmAltaModifica alta = new frmAltaModifica();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            try
            {
                if(dgvArticulos.CurrentRow != null)
                {
                    Program.frmArticulosPrincipal.Hide();
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    frmAltaModifica modificar = new frmAltaModifica(seleccionado);
                    modificar.ShowDialog();
                    cargar();
                }
                else
                {
                    MessageBox.Show("No se ha seleccionado ningún artículo");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if(dgvArticulos.CurrentRow != null)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    DialogResult respuesta = MessageBox.Show("Si elimina el artículo es posible que no pueda recuperarlo. ¿Realmente desea eliminar el artículo seleccionado?", "Eliminando", MessageBoxButtons.YesNo);
                    if (respuesta == DialogResult.Yes)
                    {
                        negocio.eliminar(seleccionado.Id);
                        cargar();
                    }
                }
                else
                {
                    MessageBox.Show("No se ha seleccionado ningún artículo para eliminar");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDetalle artDetallado = new frmDetalle();
            Articulo s = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            artDetallado.articuloDetallado(s.Id.ToString(), s.Codigo,s.Nombre,s.Descripcion,s.Marca.ToString(), s.Categoria.ToString(),s.Precio.ToString("0.00"),s.UrlImagen);
            artDetallado.ShowDialog();
        }

        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> ListaFiltro;
            string filtro = txtFiltroRapido.Text;

            if (filtro.Length >= 3)
                ListaFiltro = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            else
                ListaFiltro = listaArticulos;

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = ListaFiltro;
            ocultarColumnas();
        }

        private void btnbusquedaAvanzada_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            if (validarBusqueda())
                return;
            string campo = cboCampo.SelectedItem.ToString();
            string criterio = cboCriterio.SelectedItem.ToString();
            string filtro;
            if (cboCampo.Text == "Marca")
                filtro = criterio;
            else if (cboCampo.Text == "Categoría")
                filtro = criterio;
            else
                filtro = txtFiltro.Text;
            listaFiltrada = negocio.filtrar(campo,criterio,filtro);
            dgvArticulos.DataSource = listaFiltrada;
           
        }

        private bool validarBusqueda()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                lblCampo.ForeColor = Color.Red;
                lblCampoRequerido.Visible = true;
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                lblCriterio.ForeColor = Color.Red;
                lblCriterioRequerido.Visible = true;
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    lblFiltroRequerido.Visible = true;
                    return true;
                }
                if (!(helper.soloNumeros(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo puede ingresar números para filtrar por Precio");
                    return true;
                }
            }
            return false;
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
                cboCriterio.Items.Add("Mayor a");
            }
            else if (opcion == "Marca")
            {
                cboCriterio.Items.Clear();
                MarcaNegocio negocio = new MarcaNegocio();
                cboCriterio.Items.AddRange(negocio.listarMarcas().ToArray());
            }
            else if (opcion == "Categoría")
            {
                cboCriterio.Items.Clear();
                CategoriaNegocio negocio = new CategoriaNegocio();
                cboCriterio.Items.AddRange(negocio.listarCategorias().ToArray());
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.Items.Add("Termina con");
            }
        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            cargar();
            txtFiltro.Text = "";
            cboCriterio.SelectedItem = null;
        }

        private void cboCampo_SelectedValueChanged_2(object sender, EventArgs e)
        {
            lblCampo.ForeColor = Color.DarkBlue;
            lblCampoRequerido.Visible = false;
        }

        private void cboCriterio_SelectedValueChanged_1(object sender, EventArgs e)
        {
            lblCriterio.ForeColor = Color.DarkBlue;
            lblCriterioRequerido.Visible = false;
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if(txtFiltro.Text.Length > 0)
            {
                lblFiltroRequerido.Visible = false;
            }
        }
    }
}
