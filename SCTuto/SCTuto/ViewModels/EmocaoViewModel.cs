using Plugin.Media;
using SCTuto.Models;
using SCTuto.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media.Abstractions;

namespace SCTuto.ViewModels
{
    public class EmocaoViewModel : BaseViewModel
    {
        private const string EmocaoApiKey = ""; // Coloque sua chave
        private readonly EmocaoServico _servicoEmocao = new EmocaoServico(EmocaoApiKey);
        private Stream _imagemStream;

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); TirarFotoCommnand.ChangeCanExecute();
                AbrirFotoCommand.ChangeCanExecute(); AnalisarImagemStreamCommand.ChangeCanExecute();
                AnalisarImagemUrlCommand.ChangeCanExecute();
            }
        }

        private List<ResultadoEmocao> _resultadoImagem;

        public List<ResultadoEmocao> ResultadoImagem
        {
            get { return _resultadoImagem; }
            set { _resultadoImagem = value; OnPropertyChanged(); 
            }
        }

        private string _imagemUrl;

        public string ImagemUrl
        {
            get { return _imagemUrl; }
            set { _imagemUrl = value; OnPropertyChanged(); }
        }

        private string _mensagemErro;

        public string MenssagemErro
        {
            get { return _mensagemErro; }
            set { _mensagemErro = value; OnPropertyChanged(); }
        }

        public Command TirarFotoCommnand { get; }
        public Command AbrirFotoCommand { get; }
        public Command AnalisarImagemUrlCommand { get; }
        public Command AnalisarImagemStreamCommand { get; }

        public EmocaoViewModel()
        {
            TirarFotoCommnand = new Command(async () => await ExecuteTirarFotoCommnand(), () => !IsBusy);
            AbrirFotoCommand = new Command(async () => await ExecuteAbrirFotoCommand(), () => !IsBusy);
            AnalisarImagemUrlCommand = new Command(async () => await ExecuteAnalisarImagemUrlCommand(), () => !IsBusy);
            AnalisarImagemStreamCommand = new Command(async () => await ExecuteAnalisarImagemStreamCommand(), () => !IsBusy);
        }

        async Task ExecuteAnalisarImagemStreamCommand()
        {

            if (!IsBusy)
            {
                Exception Erro = null;

                try
                {
                    IsBusy = true;
                    ResultadoImagem = null;
                    MenssagemErro = string.Empty;

                    ResultadoImagem = await _servicoEmocao.ReconhecimentoEmocaoStreamAsync(_imagemStream);
                }
                catch (Exception ex)
                {

                    Erro = ex;
                    MenssagemErro = Erro.ToString();
                }
                finally
                {
                    IsBusy = false;
                }
                
            }
            return;
        }

        async Task ExecuteAnalisarImagemUrlCommand()
        {

            if (!IsBusy)
            {
                Exception Erro = null;

                try
                {
                    IsBusy = true;

                    ResultadoImagem = null;
                    MenssagemErro = string.Empty;

                    ResultadoImagem = await _servicoEmocao.ReconhecimentoEmocaoUrlAsync(ImagemUrl);
                }
                catch (Exception ex)
                {

                    Erro = ex;
                    MenssagemErro = Erro.ToString();
                }
                finally
                {
                    IsBusy = false;
                }                
            }
            return;
        }

        async Task ExecuteAbrirFotoCommand()
        {
            await CrossMedia.Current.Initialize();
            var arquivoFoto = await CrossMedia.Current.PickPhotoAsync();

            _imagemStream = arquivoFoto?.GetStream();
            ImagemUrl = arquivoFoto?.Path;
        }

        async Task ExecuteTirarFotoCommnand()
        {
            await CrossMedia.Current.Initialize();
            var arquivoFoto = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

            _imagemStream = arquivoFoto?.GetStream();
            ImagemUrl = arquivoFoto?.Path;
        }
    }
}
