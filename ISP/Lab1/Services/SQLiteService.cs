using Lab1.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class SQLiteService : IDbService
    {
        SQLiteConnection database;
        public IEnumerable<Artist> GetAllArtists()
        {
            return database.Table<Artist>().ToList();
        }
        public IEnumerable<Music> GetAllMusics(int id)
        {
            return database.Table<Music>().Where(x => x.ArtistId==id).ToList();
        }
        public void Init()
        {
            if (!(File.Exists(Path.Combine(FileSystem.AppDataDirectory, "lab3db.db"))))
            {
                
                database = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "lab3db.db"));
                database.CreateTable<Artist>();
                database.CreateTable<Music>();



                var Artist1 = new Artist { Name = "ACDC", typeOfMusic = "Rock", };
                var Artist2 = new Artist { Name = "Sabaton", typeOfMusic = "Metall" };
                database.Insert(Artist1);
                database.Insert(Artist2);


                for (int i = 1; i <= 5; i++)
                {
                    var music1 = new Music { NameOfMusic= $"Музыка №{i} ACDC.", YearOfRelease=$" Год выпуска {1980+i}.", ArtistId = Artist1.Id };
                    var music2 = new Music { NameOfMusic = $"Музыка №{i} Sabaton.", YearOfRelease = $" Год выпуска {2010+i}.", ArtistId = Artist2.Id };
                    database.Insert(music1);
                    database.Insert(music2);
                }
            }
            else
            {
                database = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "lab3db.db"));
            }
        }
    }
}
