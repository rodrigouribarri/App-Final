using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_RodrigoURIBARRI
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary> 
        public static frmArticulosPrincipal frmArticulosPrincipal;  
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(frmArticulosPrincipal = new frmArticulosPrincipal());
        }
    }
}
