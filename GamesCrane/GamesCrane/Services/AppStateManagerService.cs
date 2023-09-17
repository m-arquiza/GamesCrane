using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using GamesCrane.Model;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Diagnostics;

namespace GamesCrane.Services
{
    public static class AppStateManagerService
    {
        public static async Task SaveAppStateAsync(Game[,] games)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile savedGamesFile = await localFolder.CreateFileAsync("appgames.xml", CreationCollisionOption.ReplaceExisting);
                
                Game[] flattenedGames = games.Cast<Game>().ToArray();


                using (var stream = await savedGamesFile.OpenStreamForWriteAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Game[]));
                    serializer.Serialize(stream, flattenedGames);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static async Task<Game[,]> LoadAppStateAsync()
        {
            Game[,] games = new Game[3, 5];
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile savedGamesFile = await localFolder.GetFileAsync("appgames.xml");

            using (Stream stream = await savedGamesFile.OpenStreamForReadAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    XmlSerializer check = new XmlSerializer(typeof(Game[]));
                    Game[] flattenedGames = (Game[])check.Deserialize(reader);

                    for (int row = 0; row < 3; row++)
                    {
                        for (int column = 0; column < 5; column++)
                        {
                            int index = row * 5 + column;

                            games[row, column] = flattenedGames[index];
                        }
                    }
                }
            }
            return games;
        }
    }
}
