using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPFinalNivel2_Castro
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Muestra el formulario de inicio
            /*using (var inicio = new frmInicio())
            {
                inicio.StartPosition = FormStartPosition.CenterScreen;
                inicio.Show();
                Application.DoEvents();

                // Simula un trabajo que toma tiempo (3 segundos)
                Thread.Sleep(3000);

                // Cierra el formulario de inicio
                inicio.Close();
            }*/
            // Inicia el formulario principal
            Application.Run(new frmInicio());
            Application.Run(new frmArticulos());
        }
    }
}
