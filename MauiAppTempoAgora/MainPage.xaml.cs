using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System;
using System.Net.Http;             // Adicionando a diretiva using correta para HttpClient
using System.Threading.Tasks;     // Adicionando a diretiva using correta para tarefas assíncronas
using Microsoft.Maui.Networking; // Adicionando para verificar conexão

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se há conexão com a internet
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("Sem conexão", "Você está offline. Por favor, verifique sua conexão com a internet e tente novamente.", "OK");
                    return;
                }

                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Cidade: {txt_cidade.Text}\n" +
                                        $"Temperatura Minima: {t.temp_min}°C\n" +
                                        $"Temperatura Maxima: {t.temp_max}°C\n" +
                                        $"Descrição: {t.description}\n" +
                                        $"Velocidade do Vento: {t.speed} m/s\n" +
                                        $"Visibilidade: {t.visibility} m\n" +
                                        $"Latitude: {t.lat}\n" +
                                        $"Longitude: {t.lon}\n"
                                        + $"Nascer do Sol: {t.sunrise}\n" +
                                        $"Pôr do Sol: {t.sunset}\n" +
                                        $"Direção do Vento: {t.wind}\n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão.";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha o nome da cidade.";
                }
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("No connection"))
            {
                await DisplayAlert("Erro de conexão", "Não foi possível conectar ao servidor. Verifique sua conexão com a internet.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}