using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TPFinalNivel2_Castro
{
    public partial class frmInicio : Form
    {
        //Configuracion de progress bar
        private Timer timer2;
        private int progresoMaximo = 100;
        private int duracionEnMilisegundos = 3000; // 3000 milisegundos = 3 segundos
        //--------
        public frmInicio()
        {
            InitializeComponent();
            // Configura el temporizador
            Timer timer = new Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 300; // 300 milisegundos = 0.3 segundos
            timer.Start(); 
            inicializarProgressBar();
            inicializarTimer();
        }
        private void inicializarProgressBar()
        {
            pgsbInicio.Minimum = 0;
            pgsbInicio.Maximum = progresoMaximo;
            pgsbInicio.Value = 0;
        }
        private void inicializarTimer()
        {
            timer2 = new Timer();
            timer2.Interval = 50; // Intervalo de actualización del timer en milisegundos
            timer2.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Incrementa el progreso de pgsbInicio
            pgsbInicio.PerformStep();

            // Si alcanza el progreso máximo, detiene el temporizador
            if (pgsbInicio.Value == pgsbInicio.Maximum)
            {
                timer2.Stop();

                // Cierra el formulario de inicio después de completar el progreso
                this.Close();
            }           
        }       
    }
}
