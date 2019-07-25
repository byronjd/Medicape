﻿using Clinic.Behavior;
using Clinic.Clases;
using Clinic.Models;
using Clinic.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Clinic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public static string Username = "";
        ILoginManager iml = null;
        public Login(ILoginManager ilm)
        {
            InitializeComponent();
            iml = ilm;
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            MaterialControls control = new MaterialControls();
            control.ShowLoading("Iniciando sesion...");
            BaseUrl get = new BaseUrl();
            string url = get.url;
            var usuario = user.Text;
            var contraseña = password.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                message.IsVisible = true;
            }
            else
            {
                
                CheckUrlConnection test = new CheckUrlConnection();
                bool inf = test.TestConnection(url);
                if (inf == true)
                {

                    Usuario users = new Usuario()
                    {
                        user_Name = usuario,
                        user_Password = contraseña
                    };

                    HttpClient client = new HttpClient();

                    string controlador = "/Api/usuario/auth_admin.php";
                    client.BaseAddress = new Uri(url);

                    string json = JsonConvert.SerializeObject(users);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(controlador, content);


                    if (response.IsSuccessStatusCode)
                    {

                        string res = await response.Content.ReadAsStringAsync();
                        var result = res.ToString().Replace('"', ' ').Trim();
                            Username = usuario;
                            App.Current.Properties["name"] = Username;
                            App.Current.Properties["type"] = result;
                            App.Current.Properties["IsLoggedIn"] = true;
                            iml.ShowMainPage();
                    }
                    else
                    {
                          control.ShowAlert("Los datos estan incorrectos", "Error", "Ok");
                    }
                }
                else
                {
                    control.ShowAlert("No se pudo conectar con el servidor", "Error", "Ok");
                }
            }
        }
    }
}