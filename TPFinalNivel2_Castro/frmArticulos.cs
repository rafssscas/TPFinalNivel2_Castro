using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using dominio;
using negocio;

namespace TPFinalNivel2_Castro
{
    public partial class frmArticulos : Form
    {
        private List<Articulo> listaArticulo;
        public frmArticulos()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------
        private void frmArticulos_Load(object sender, EventArgs e)
        {
            //Carga de datos y configuracion incial de los campos desplegables
            //del filtro.
            cargar();
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");
            // Después de cargar los datos, actualiza el contador de elementos
            actualizarContadorElementos();
        }
        //------------------------------------------------------------
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            // Si hay algun objeto seleccionado carga una imagen
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }
        //------------------------------------------------------------
        private void cargar()
        {
            // Carga la lista de artículos, configura el dgvArticulos y muestra
            // la imagen del primer artículo
            //Creo una instancia de la clase ArticuloNegocio
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                //Llamo al metodo listar() y lo guardo en listaArticulo
                listaArticulo = negocio.listar();
                //Asignar y cargar los datos con DataSource a la tabla del form.
                dgvArticulos.DataSource = listaArticulo;
                ocultarColumnas();
                //Cargar la imagen de la imagen un PictureBox
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }        
        }
        //------------------------------------------------------------
        private void ocultarColumnas()
        {
            //Con el metodo Visible, la Columna de imagenUrl queda oculta o no visible.
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }
        //------------------------------------------------------------
        private void cargarImagen (string imagen)
        {
            //Carga la imagen en pbxArticulo, si el campo es NULL, carga
            //una imagen por defecto.
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pbxArticulo.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }
        //------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Abre el formulario para agregar un nuevo artículo y actualizo
            // la lista al cerrar el formulario
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
            actualizarContadorElementos();
        }
        //------------------------------------------------------------
        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Obtiene el artículo seleccionado, abre el formulario para modificar
            // y actualizar la lista al cerrar el formulario.
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }
        //------------------------------------------------------------
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            //Elimina fisicamente el Articulo de la BD.
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Confirma la eliminacion del articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
            actualizarContadorElementos();
        }
        //------------------------------------------------------------
        private bool validarFiltro()
        {
            // Valida los campos de filtro antes de realizar la búsqueda
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                return true;
            }
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
                if(string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes ingresar un precio.");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Ingrese solo números para filtrar.");
                    return true;
                }   
            }            
            return false;
        }
        //------------------------------------------------------------
        private bool soloNumeros(string cadena)
        {
            // Verifica si una cadena contiene solo números
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                return false;
            }

            return true;
        }
        //------------------------------------------------------------
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            // Filtrar la lista de artículos según los criterios seleccionados
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;           
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //------------------------------------------------------------
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            // Filtrar la lista de artículos por nombre o descripción al escribir en el txtFiltro
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;
            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }
        //------------------------------------------------------------
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Configurar opciones de criterios según el campo seleccionado en cboCampo.                             
            string opcion = cboCampo.SelectedItem.ToString();
            if(opcion == "Nombre")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Igual a");
            }
            else if(opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Igual a");
            }
        }
        private void dgvArticulos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.Rows[e.RowIndex].DataBoundItem;
                //Crear e instanciar el formulario de detalle y mostrarlo
                frmDetalleArticulo frmDetalleArticulo = new frmDetalleArticulo(seleccionado, listaArticulo);
                frmDetalleArticulo.ShowDialog();

            }

        }
        private void actualizarContadorElementos()
        {
            int cantidadElementos = dgvArticulos.Rows.Count;
            lblCantElementos.Text = $"Cantidad total de elementos: {cantidadElementos}";
        }
        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {            
            // Limpia el TextBox
            txtFiltroAvanzado.Text = string.Empty;
            // Restablece la selección en el ComboBox
            cboCampo.SelectedIndex = 0;
            cboCriterio.SelectedIndex = 0;
            // Recarga los datos originales en el DataGridView o realiza la acción que desees
            cargar(); // Puedes implementar este método según tus necesidades
        }
        private void btnLimpiarDos_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = string.Empty;
            cargar();
        }

   
    }
}
