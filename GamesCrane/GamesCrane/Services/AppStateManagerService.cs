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

                LoadAppStateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static async Task LoadAppStateAsync()
        {
            Game[,] result = new Game[3, 5];
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile justchecking = await localFolder.GetFileAsync("appgames.xml");

            using (Stream stream = await justchecking.OpenStreamForReadAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    XmlSerializer check = new XmlSerializer(typeof(Game[]));
                    Game[] flat = (Game[])check.Deserialize(reader);

                    for (int row = 0; row < 3; row++)
                    {
                        for (int column = 0; column < 5; column++)
                        {
                            // Calculate the index in the flattened array based on row and column.
                            int index = row * 5 + column;

                            // Assign the corresponding element from the flattened array to the 2D array.
                            result[row, column] = flat[index];
                        }
                    }
                }
            }

            foreach (var game in result)
            {
                if (game != null)
                {
                    Debug.WriteLine(game.Title);
                }
            }
        }
    }
}
