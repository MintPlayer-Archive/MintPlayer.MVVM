using MintPlayer.MVVM.Demo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MintPlayer.MVVM.Demo.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetArtists();
        Task<Artist> GetArtist();
        Task<Artist> InsertArtist(Artist artist);
        Task<Artist> UpdateArtist(Artist artist);
        Task<Artist> DeleteArtist(Artist artist);
    }
    internal class ArtistService : IArtistService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl = "https://mintplayer.com/api";
        public ArtistService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            using (var response = await httpClient.GetAsync($"{baseUrl}/Artist"))
            {
                var content = await response.Content.ReadAsStringAsync();
                var artists = JsonConvert.DeserializeObject<IEnumerable<Artist>>(content);
                return artists;
            }
        }

        public Task<Artist> GetArtist()
        {
            throw new NotImplementedException();
        }

        public Task<Artist> InsertArtist(Artist artist)
        {
            throw new NotImplementedException();
        }

        public Task<Artist> UpdateArtist(Artist artist)
        {
            throw new NotImplementedException();
        }

        public Task<Artist> DeleteArtist(Artist artist)
        {
            throw new NotImplementedException();
        }
    }
}
