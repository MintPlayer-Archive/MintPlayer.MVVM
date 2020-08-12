using System.Collections.Generic;

namespace MintPlayer.MVVM.Demo.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? YearStarted { get; set; }
        public int? YearQuit { get; set; }

        public List<Song> Songs { get; set; }
    }
}