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

namespace TPFinalNivel2_Castro
{
    public partial class frmDetalleArticulo : Form
    {
        private List<Articulo> listaArticulo;
        private int indiceActual;
        private Articulo articulo = null;
        public frmDetalleArticulo(Articulo articulo, List<Articulo> listaArticulo)
        {
            InitializeComponent();
            this.listaArticulo = listaArticulo;
            this.indiceActual = listaArticulo.IndexOf(articulo);
            mostrarDetalles();
        }
        private void mostrarDetalles()
        {
            //Verificar que el indice este dentro del rango
            if(indiceActual >= 0 && indiceActual < listaArticulo.Count)
            {
                Articulo aux = listaArticulo[indiceActual];
                lblPrecio.Text = aux.Precio.ToString();
                lblNombre.Text = aux.Nombre;
                txtDescripcion.Text = aux.Descripcion;
                pbxArticuloDetalle.Text = aux.ImagenUrl;
                cargarImagen(aux.ImagenUrl);                
            }
        }              
        private void cargarImagen(string imagen)
        {
            //Metodo para cargar la imagen en el pbxArticulo.
            try
            {
                pbxArticuloDetalle.Load(imagen);
            }
            catch (Exception)
            {
                //Cargar una imagen predeterminada si hay un error al cargar la imagen.
                pbxArticuloDetalle.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            indiceActual++;
            if (indiceActual >= listaArticulo.Count)
            {
                // Si supera el último artículo, volver al primero
                indiceActual = 0;
            }
            mostrarDetalles();
        }
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            indiceActual--;
            if (indiceActual < 0)
            {
                // Si va antes del primer artículo, ir al último
                indiceActual = listaArticulo.Count - 1;
            }
            mostrarDetalles();
        }
    }
}
