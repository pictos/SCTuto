using Xamarin.Forms;

namespace SCTuto.Views
{
    public class EmocaoView : ContentPage
    {
        public EmocaoView()
        {
            Title = "Detecção de Emoção";

            BindingContext = new ViewModels.EmocaoViewModel();

            var tirarFotoButton = new Button
            {
                Text = "Tirar Foto"
            };

            tirarFotoButton.SetBinding(Button.CommandProperty, "TirarFotoCommand");

            var abrirFotoButton = new Button
            {
                Text = "Abrir Foto"
            };
            abrirFotoButton.SetBinding(Button.CommandProperty, "AbrirFotoCommand");

            var imagemUrlEntry = new Entry();
            imagemUrlEntry.SetBinding(Entry.TextProperty, "ImagemUrl");

            var imagem = new Image
            {
                HeightRequest = 300
            };
            imagem.SetBinding(Image.SourceProperty, "ImagemUrl");

            var analisarImagemUrlButton = new Button
            {
                Text = "Reconhecer emoção por URL",
                FontSize = 21
            };
            analisarImagemUrlButton.SetBinding(Button.CommandProperty, "AnalisarImagemUrlCommand");

            var analisarImagemStreamButton = new Button
            {
                Text = "Reconhecer emoção por stream",
                FontSize = 21
            };
            analisarImagemStreamButton.SetBinding(Button.CommandProperty, "AnalisarImagemStreamCommand");

            var activeIndicator = new ActivityIndicator();
            activeIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            activeIndicator.SetBinding(ActivityIndicator.IsEnabledProperty, "IsBusy");
            activeIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            var msgErroLabel = new Label
            {
                TextColor = Color.Red,
                FontSize = 22
            };
            msgErroLabel.SetBinding(Label.TextProperty, "MenssagemErro");

            var emocaoDataTemplate = new DataTemplate(() =>
            {
                var raivaLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                raivaLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Anger",
                    BindingMode.Default,
                    null,
                    null,
                    "Raiva : {0:F0}"));

                var desprezoLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                desprezoLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Contempt",
                    BindingMode.Default,
                    null,
                    null,
                    "Desprezo : {0:F0}"));

                var nojoLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                nojoLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Disgust",
                    BindingMode.Default,
                    null,
                    null,
                    "Nojo : {0:F0}"));

                var medoLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                medoLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Fear",
                    BindingMode.Default,
                    null,
                    null,
                    "Medo : {0:F0}"));

                var alegriaLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                alegriaLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Happiness",
                    BindingMode.Default,
                    null,
                    null,
                    "Alegria : {0:F0}"));

                var neutroLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                neutroLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Neutral",
                    BindingMode.Default,
                    null,
                    null,
                    "Neutro : {0:F0}"));

                var tristezaLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };
                tristezaLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Sadness",
                    BindingMode.Default,
                    null,
                    null,
                    "Tristeza : {0:F0}"));

                var surpresaLabel = new Label
                {
                    TextColor = Color.Black,
                    FontSize = 15
                };

                surpresaLabel.SetBinding(Label.TextProperty, new Binding(
                    "Scores.Surprise",
                    BindingMode.Default,
                    null,
                    null,
                    "Surpresa : {0:F0}"));

                var faceStackLayout = new StackLayout
                {
                    Padding = 2,
                    Children =
                    {
                        raivaLabel,
                        desprezoLabel,
                        nojoLabel,
                        medoLabel,
                        alegriaLabel,
                        neutroLabel,
                        tristezaLabel,
                        surpresaLabel
                    }
                };

                return new ViewCell
                {
                    View = faceStackLayout
                };
            });

            var emocaoListView = new ListView
            {
                HasUnevenRows = true,
                ItemTemplate = emocaoDataTemplate
            };
            emocaoListView.SetBinding(ListView.ItemsSourceProperty, "ResultadoImagem");

            var stackLayout = new StackLayout
            {
                Padding = new Thickness(10, 20),
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            tirarFotoButton,
                            abrirFotoButton
                        }
                    },
                    imagemUrlEntry,
                    imagem,
                    analisarImagemStreamButton,
                    analisarImagemUrlButton,
                    msgErroLabel,
                    emocaoListView
                }
            };

            Content = new ScrollView
            {
                Content = stackLayout
            };
        }
    }
}
