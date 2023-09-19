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
    /// <summary>
    /// The <c>AppStateManagerService</c> is used to save and load games from the vending machine into storage.
    /// </summary>
    public static class AppStateManagerService
    {

        /// <summary>
        /// Saves given list of games to disk.
        /// <param name="games">games to save</param> 
        /// </summary>
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

        /// <summary>
        /// Loads and returns saved games from disk.
        /// <returns>
        /// An Object[] array such that the first element is the number of games loaded
        /// and the second is the loaded games.
        /// </returns>
        /// </summary>
        public static async Task<Object[]> LoadAppStateAsync()
        {
            Game[,] games = new Game[3, 5];
            int gameCount = 0;
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
                            Game game = flattenedGames[index];

                            if (game != null)
                            {
                                gameCount++;
                            }

                            games[row, column] = game;
                        }
                    }
                }
            }
            Object[] res = new object[2];
            res[0] = gameCount;
            res[1] = games;

            return res;
        }
    }
}
