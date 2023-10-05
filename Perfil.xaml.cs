using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppDiscoteca
{
    /// <summary>
    /// Lógica de interacción para Perfil.xaml
    /// </summary>
    public partial class Perfil : Window
    {
        public Persona Dades
        {
            get
            {
                Persona p = new Persona();
                p.setNom(txtNomPerfil.Text);
                p.setTempsCua(Int32.Parse(txtCuaPerfil.Text));
                p.setTempsDisco(Int32.Parse(txtDiscoPerfil.Text));
                p.setUrlFoto(txtUrlPerfil.Text);
                return p;
            }
            set
            {
                this.txtNomPerfil.Text = value.getNom();
                this.txtCuaPerfil.Text = value.getTempsCua().ToString();
                this.txtDiscoPerfil.Text = value.getTempsDintra().ToString();
                this.txtUrlPerfil.Text = value.getUrlFoto().ToString();
                this.txtNomPerfil.Focus();
            }
        }

        public Perfil()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
