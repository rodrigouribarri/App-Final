using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_RodrigoURIBARRI
{
    class metodosHelper
    {
        public bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (char.IsLetter(caracter))
                    return false;
            }
            return true;
        }
        public bool validarCaracteres(string codigo, string nombre, string descripcion, string url)
        {
            if (codigo.Length > 50)
            {
                MessageBox.Show("El campo código admite hasta 50 carasteres");
                return false;
            }
            if (nombre.Length > 50)
            {
                MessageBox.Show("El campo nombre admite hasta 50 caracteres");
                return false;
            }
            if (descripcion.Length > 150)
            {
                MessageBox.Show("El campo descripción admite hasta 1000 caracteres");
                return false;
            }
            if(url.Length > 1000)
            {
                MessageBox.Show("La url indicada supera los 1000 caracteres");
                return false;
            }
            return true;
        }
        public string cargarImagen(string imagen)
        {
            try
            {
                return imagen;
            }
            catch (Exception)
            {

                return "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBEcRjSYF6uc6TrgAIF5d_44nJwx03DYUOejh_Hf4&s";
            }
        }


    }
}
