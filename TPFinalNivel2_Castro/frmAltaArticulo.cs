using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace TPFinalNivel2_Castro
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        //------------------------------------------------------------
        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }
        //------------------------------------------------------------
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Cierra el formulario al puslar en el botón Cancelar.
            Close();
        }
        //------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Metodo para agregar o modificar un articulo
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                //Crea un nuevo articulo si no existe.
                if(articulo == null)
                   articulo = new Articulo();
                //Asigna valores al objeto desde los campos del formulario
                articulo.Nombre = txtNombre.Text;
                articulo.Codigo = txtCodigo.Text;
                articulo.Precio = int.Parse(txtPrecio.Text);
                //Marca
                articulo.Marca = (Marca)cbxMarca.SelectedItem;
                //Categoria
                articulo.Categoria = (Categoria)cbxCategoria.SelectedItem;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Descripcion = txtDescripcion.Text;
                //Llama a los metodos de ArticuloNegocio para agregar o modificar 
                //según el estado del ID del Articulo.
                if(articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }
                //Guardo la imagen localmente
                if(archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                }
                //Cierro el formulario después de realizar la operación.
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        //------------------------------------------------------------
        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            //Metodo usado para cargar el formulario
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                //Configura las fuentes de datos para ambos comboBox.
                cbxMarca.DataSource = marcaNegocio.listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";
                                
                cbxCategoria.DataSource = categoriaNegocio.listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";
                //Si existe un articulo, carga sus datos en los campos del frm.
                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cbxMarca.SelectedValue = articulo.Marca.Id;
                    cbxCategoria.SelectedValue = articulo.Categoria.Id;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());             
            }
        }
        //------------------------------------------------------------
        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            //Carga la imagen al salir del cuadro de texto URL.
            cargarImagen(txtUrlImagen.Text);
        }
        //------------------------------------------------------------
        private void cargarImagen(string imagen)
        {
            //Metodo para cargar la imagen en el pbxArticulo.
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                //Cargar una imagen predeterminada si hay un error al cargar la imagen.
                pbxArticulo.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }
        //------------------------------------------------------------
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //Metodo para seleccionar y cargar una imagen de forma local con el sistema de archivos.
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg";
            
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
        }
    }
}
