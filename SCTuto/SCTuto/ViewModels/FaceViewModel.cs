using System;
using System.Linq;
using Plugin.Media;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using System.Collections.ObjectModel;
using SCTuto.Models;
using Xamarin.Forms;
using Plugin.Media.Abstractions;

namespace SCTuto.ViewModels
{
    public class FaceViewModel : BaseViewModel
    {
        public ObservableCollection<Pessoa> Pessoas { get; set; }

        FaceServiceClient client;

        private readonly string url = "https://brazilsouth.api.cognitive.microsoft.com/face/v1.0"; //Coloque a URL do seu serviço
        private readonly string apiKey = ""; //Coloque sua chave

        public Command IdCommand { get; }

        string politicoGrupoId;

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); IdCommand.ChangeCanExecute(); }
        }

        public FaceViewModel()
        {
            client = new FaceServiceClient(apiKey, url);
            IdCommand = new Command(async () => await ExecuteIdCommand(), () => !IsBusy);

            Pessoas = new ObservableCollection<Pessoa>
            {
                new Pessoa {Nome = "LuLa", Partido = "PT", FotoUrl = "http://lula.com.br/sites/all/themes/lula/images/ft-lula-biografia.jpg"},
                new Pessoa {Nome = "Dilma", Partido = "PT", FotoUrl = "http://www.cafecolombo.com.br/imagens//2017/05/dilma-rousseff.jpg"},
                new Pessoa {Nome = "FHC", Partido = "PSDB", FotoUrl = "http://edgblogs.s3.amazonaws.com/brunoastuto/files/2011/11/fhc.jpg"},
                new Pessoa {Nome = "Itamar", Partido = "PSDB", FotoUrl = "https://www.estudopratico.com.br/wp-content/uploads/2013/03/governo-de-itamar-franco.jpg"}
            };

            RegistrarPresidentes();
        }


        async Task ExecuteIdCommand()
        {
            if (!IsBusy)
            {
                Exception Erro = null;

                try
                {
                    IsBusy = true;
                    await CrossMedia.Current.Initialize();

                    MediaFile foto;

                    foto = await CrossMedia.Current.PickPhotoAsync();

                    using (var stream = foto.GetStream())
                    {
                        var faces = await client.DetectAsync(stream);
                        var faceIds = faces.Select(face => face.FaceId).ToArray();

                        var resultado = await client.IdentifyAsync(politicoGrupoId, faceIds);
                        var resposta =  resultado[0].Candidates[0].PersonId;

                        var politico = await client.GetPersonAsync(politicoGrupoId, resposta);

                        await DisplayAlert("Resultado", $"O político indentificado é : {politico.Name}","ok");
                    }
                }
                catch (Exception ex)
                {

                    Erro = ex;
                }

                finally
                {
                    IsBusy = false;
                }

                if (Erro != null)
                    await DisplayAlert("Algo de errado não está certo", Erro.Message, "ok");
            }
        }


        async Task RegistrarPresidentes()
        {
            politicoGrupoId = Guid.NewGuid().ToString();

            try
            {
                await client.CreatePersonGroupAsync(politicoGrupoId, "Presidentes");

                foreach (var presidentes in Pessoas)
                {
                    var p = await client.CreatePersonAsync(politicoGrupoId, presidentes.Nome);
                    await client.AddPersonFaceAsync(politicoGrupoId, p.PersonId, presidentes.FotoUrl);
                }

                await client.TrainPersonGroupAsync(politicoGrupoId);
            }
            catch (FaceAPIException ex)
            {

                await DisplayAlert("Algo de errado não está certo", ex.Message, "Ok");
            }
        }
    }
}
