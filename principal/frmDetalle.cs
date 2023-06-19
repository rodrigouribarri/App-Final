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
    public partial class frmDetalle : Form
    {
        metodosHelper helper = new metodosHelper();
        public frmDetalle()
        {
            InitializeComponent();
        }
        
        public void articuloDetallado(string id,string cod, string nom, string desc,string marca, string cat, string pre, string url)
        {
            try
            {
                txtid.Text = id;
                txtnombre.Text = nom;
                txtcodigo.Text = cod;
                txtdescripcion.Text = desc;
                txtmarca.Text = marca;
                txtcategoria.Text = cat;
                txtprecio.Text = "$"+pre;
                try
                {
                    pcbArticulo.Load(url);
                }
                catch (Exception)
                {
                    pcbArticulo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBEcRjSYF6uc6TrgAIF5d_44nJwx03DYUOejh_Hf4&s");
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Close();
            Program.frmArticulosPrincipal.Show();

        }
    }
}
