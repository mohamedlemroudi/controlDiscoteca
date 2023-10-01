using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppDiscoteca
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int capacitat = 0;
        private SemaphoreSlim semaforo;
        private Queue<Persona> colaEspera = new Queue<Persona>();
        private List<Task> tasquesPersona = new List<Task>();

        public MainWindow()
        {
            InitializeComponent();
            // Inicialitzem el semafor amb un recompta de 0
            semaforo = new SemaphoreSlim(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbNomPersona.Text) ||
                string.IsNullOrWhiteSpace(tbCapacitat.Text) ||
                string.IsNullOrWhiteSpace(tbPersonaCua.Text) ||
                string.IsNullOrWhiteSpace(tbPersonaDintra.Text) ||
                string.IsNullOrWhiteSpace(tbUrlFoto.Text))
            {
                MessageBox.Show("Falta algún camp per completar.");
            }
            else
            {
                capacitat = Int32.Parse(tbCapacitat.Text);

                string nomPersona = tbNomPersona.Text;
                int tempsCua = Int32.Parse(tbPersonaCua.Text);
                int tempsDintra = Int32.Parse(tbPersonaDintra.Text);
                string urlFoto = tbUrlFoto.Text;

                var persona = new Persona(nomPersona, tempsCua, tempsDintra, urlFoto);
                afegirPersonaDintre(persona);
            }
        }

        private async void afegirPersonaDintre(Persona persona)
        {
            if (listaDintre.Items.Count < capacitat)
            {
                // Si hay espacio en la discoteca, la persona puede entrar de inmediato
                await EntrarEnDiscoteca(persona);
            }
            else
            {
                // Si no hay espacio, la persona se agrega a la cola de espera
                colaEspera.Enqueue(persona);
                await EsperarEnCola(persona);
            }
        }

        private async Task EntrarEnDiscoteca(Persona persona)
        {
            // Crear un objeto de datos con las propiedades necesarias
            var itemPersona = new
            {
                ImagenSource = new BitmapImage(new Uri(persona.getUrlFoto())),
                Nom = persona.getNom()
            };
            listaCua.Items.Remove(itemPersona);
            listaDintre.Items.Add(itemPersona);

            // Espera el tiempo especificado antes de continuar
            await Task.Delay(TimeSpan.FromSeconds(persona.getTempsDintra()));

            var itemToRemove = listaDintre.Items.Cast<object>().FirstOrDefault(item =>
            {
                if (item is var objeto)
                {
                    if (objeto.GetType().GetProperty("Nom")?.GetValue(objeto) is string nombre && nombre == persona.getNom())
                    {
                        return true;
                    }
                }
                return false;
            });

            if (itemToRemove != null)
            {
                listaDintre.Items.Remove(itemToRemove);
                listaHistoric.Items.Add(itemToRemove);
            }

            // Comprueba si hay personas en la cola de espera y permite que entren
            if (colaEspera.Count > 0)
            {
                var personaEnCola = colaEspera.Dequeue();
                await EntrarEnDiscoteca(personaEnCola);
            }
        }

        private async Task EsperarEnCola(Persona persona)
        {
            // Agrega la persona a la cola de espera visualmente
            var itemPersona = new
            {
                ImagenSource = new BitmapImage(new Uri(persona.getUrlFoto())),
                Nom = persona.getNom()
            };

            listaCua.Items.Add(itemPersona);

            // Espera el tiempo especificado antes de continuar
            await Task.Delay(TimeSpan.FromSeconds(persona.getTempsCua()));

            // Elimina a la persona de la cola visualmente
            // Obtener el índice del elemento que coincide con la persona
            int indexToRemove = listaCua.Items.IndexOf(itemPersona);

            if (indexToRemove != -1)
            {
                // Si se encuentra, eliminar por índice
                listaCua.Items.RemoveAt(indexToRemove);
            }

            // Comprueba si hay espacio en la discoteca y permite que entren personas en la cola
            if (listaDintre.Items.Count < capacitat && colaEspera.Count > 0)
            {
                var personaEnCola = colaEspera.Dequeue();
                await EntrarEnDiscoteca(personaEnCola);
            } else if(colaEspera.Count > 0)
            {
                colaEspera.Dequeue();
            }
        }


        // Borrar Items
        private void borrarItemCua(object sender, RoutedEventArgs e)
        {
            // Verifica si hay un elemento seleccionado en la lista
            if (listaCua.SelectedItem != null)
            {
                // Elimina el elemento seleccionado de la lista
                listaCua.Items.Remove(listaCua.SelectedItem);
            }
        }

        private void borrarItemDintre(object sender, RoutedEventArgs e)
        {
            // Verifica si hay un elemento seleccionado en la lista
            if (listaDintre.SelectedItem != null)
            {
                // Elimina el elemento seleccionado de la lista
                listaDintre.Items.Remove(listaDintre.SelectedItem);
            }
        }

        private void borrarItemHistoric(object sender, RoutedEventArgs e)
        {
            // Verifica si hay un elemento seleccionado en la lista
            if (listaHistoric.SelectedItem != null)
            {
                // Elimina el elemento seleccionado de la lista
                listaHistoric.Items.Remove(listaHistoric.SelectedItem);
            }
        }
    }
}

