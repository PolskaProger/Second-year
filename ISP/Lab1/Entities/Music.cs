using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Entities
{
    [Table("Musics")]
    public class Music
    {
        [PrimaryKey, AutoIncrement, Indexed]
        [Column("Id")]
        public int MusicId { get; set; }
        public string NameOfMusic { get; set; }
        public string YearOfRelease { get; set; }
        [Indexed]
        public int ArtistId { get; set; }
    }

}
