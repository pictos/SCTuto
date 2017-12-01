using SCTuto.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;

namespace SCTuto.Services
{
    public class EmocaoServico
    {
        private readonly string _key;
        private readonly string _reconhecimentoEmocaoUrl = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize"; //Coloque a URL do seu serviço

        public EmocaoServico(string key) => _key = key;

        public async Task<List<ResultadoEmocao>> ReconhecimentoEmocaoUrlAsync(string imageUrl)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);

            var stringContent = new StringContent(@"{""url"":""" + imageUrl + @"""}");

            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var resposta = await httpClient.PostAsync(_reconhecimentoEmocaoUrl, stringContent);
                var json = await resposta.Content.ReadAsStringAsync();

                if(resposta.IsSuccessStatusCode)
                {
                    var imagemResultadoUrl = JsonConvert.DeserializeObject<List<ResultadoEmocao>>(json);
                    return imagemResultadoUrl;
                }
                throw new Exception(json);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<ResultadoEmocao>> ReconhecimentoEmocaoStreamAsync(Stream stream )
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);

            var streamContent = new StreamContent(stream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            try
            {
                var resposta = await httpClient.PostAsync(_reconhecimentoEmocaoUrl, streamContent);
                var json = await resposta.Content.ReadAsStringAsync();

                if(resposta.IsSuccessStatusCode)
                {
                    var imagemResultado = JsonConvert.DeserializeObject<List<ResultadoEmocao>>(json);
                    return imagemResultado;
                }

                throw new Exception(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
