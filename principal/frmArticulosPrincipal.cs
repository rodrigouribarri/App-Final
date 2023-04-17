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
            frmAltaModifica alta = new frmAltaModifica();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaModifica modificar = new frmAltaModifica(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                DialogResult respuesta = MessageBox.Show("Si elimina el artículo es posible que no pueda recuperarlo. ¿Realmente desea eliminar el artículo seleccionado?", "Eliminando", MessageBoxButtons.YesNo);
                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
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
                ListaFiltro = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descipcion.ToUpper().Contains(filtro.ToUpper()));
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
            string filtro = txtFiltro.Text;
            listaFiltrada = negocio.filtrar(campo,criterio,filtro);
            dgvArticulos.DataSource = listaFiltrada;
           
        }

        private bool validarBusqueda()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("¡Debe seleccionar el campo de búsqueda!");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("¡Debe seleccionar el criterio de búsqueda!");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Para el campo precio es necesario que complete el filtro con números");
                    return true;
                }
                if (!(helper.soloNumeros(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo se admiten números para filtrar por Precio");
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
    }
}
