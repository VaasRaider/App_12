using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App10
{
    public partial class MainPage : ContentPage
    {
        //List<Persona> Personas = new List<Persona>();

        public MainPage()
        {
            InitializeComponent();
            lstPersonas.ItemsSource = App.Personas.ToList();
            
        }

        public void Actualizar()
        {
            var conn = new SQLiteConnection(App.RUTABD);
            conn.CreateTable<App.Persona>();
            App.Personas = conn.Table<App.Persona>().OrderBy(n => n.Nombre).ToList();
            lstPersonas.ItemsSource = conn.Table<App.Persona>().OrderBy(n => n.Nombre).ToList();
            //lstPersonas.ItemsSource = conn.Table<App.Persona>().OrderBy(n => n.Nombre).ToList();

            conn.Close();
            //lstPersonas.ItemsSource = App.Personas.OrderBy(n => n.Nombre).ToList();

        }
        public void verificar()
        {
            if ( App.Personas.Count() > 0 )
            {
                if (App.press == true)
                {
                    editar.IsVisible = true;
                    eliminar.IsVisible = true;
                }
                else
                {
                    editar.IsVisible = false;
                    eliminar.IsVisible = false;
                }

            }
            else
            {
                editar.IsVisible = false;
                eliminar.IsVisible = false;
            }
            App.press = false;
        }
       
protected override void OnAppearing()
        {
            Actualizar();
            App.press = false;
            verificar();

        }


        // AGREGAR
        private void button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageNuevo());
            /*
            App.Persona nuevaPersona = new App.Persona() { Nombre = txtNombre.Text, Correo = txtCorreo.Text };
            App.Personas.Add(nuevaPersona);
            lstPersonas.ItemsSource = App.Personas.OrderBy(n => n.Nombre).ToList();
            txtNombre.Text = "";
            txtCorreo.Text = "";
            */
        }

        private void cmdLimpiar(object sender, EventArgs e)
        {
            App.Persona nuevaPersona = new App.Persona() { Id = App.temp_Id, Nombre = App.temp_nombre, Correo = App.temp_correo };
            using (var conn = new SQLiteConnection(App.RUTABD))
            {
 
                    conn.Delete(nuevaPersona);
                


            }
            Actualizar();
            //App.Personas.RemoveAll(n => n.Nombre == App.temp_nombre);
            //lstPersonas.ItemsSource = App.Personas.OrderBy(n => n.Nombre).ToList();
            App.press = false;
            verificar();
            
        }

        private void lstPersonas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            App.press = true;
            var personaSeleccionada = e.SelectedItem as App.Persona;
            verificar();
            App.temp_Id = personaSeleccionada.Id;
            App.temp_nombre = personaSeleccionada.Nombre;
            App.temp_correo = personaSeleccionada.Correo;
            App.press = false;
        }
        // EDITAR
        private void button_modificar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageEditar()
            {
                BindingContext = new App.Persona() { Id = App.temp_Id, Nombre = App.temp_nombre, Correo = App.temp_correo }
            });
            

        }


    }


}
