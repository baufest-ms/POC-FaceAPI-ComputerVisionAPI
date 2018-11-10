using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BF.Demo.FaceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("================================================================================");
                Console.WriteLine("== Baufest Demo - Face API                                                    ==");
                Console.WriteLine("================================================================================");

                Console.Write(">> Choose an option [ A | B | C | D | E ]: ");
                var option = Console.ReadLine().ToUpperInvariant();

                switch (option)
                {
                    case "A":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize a single face...");
                        DetectFacesAsync(option).Wait();
                        break;
                    case "B":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize multiple faces in the image...");
                        DetectFacesAsync(option).Wait();
                        break;
                    case "C":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize happiness...");
                        DetectFaceEmotionAsync(option).Wait();
                        break;
                    case "D":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize happiness and neutral...");
                        DetectFaceEmotionAsync(option).Wait();
                        break;
                    case "E":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize face attributes...");
                        DetectFaceAttributesAsync(option).Wait();
                        break;
                    default:
                        throw new ApplicationException("Invalid option");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Environment.NewLine}>> ERROR: {ex.Message}");
            }
        }

        private static async Task DetectFacesAsync(string option)
        {
            var imageFilePath = GetImageFullPath($"Face-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Face API, please wait a moment for the results...");

            var client = new FaceAPIClient();
            var results = await client.DetectAsync(byteData);

            ShowResults(results);
        }

        private static async Task DetectFaceEmotionAsync(string option)
        {
            var imageFilePath = GetImageFullPath($"Face-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Face API, please wait a moment for the results...");

            var client = new FaceAPIClient();
            var results = await client.DetectAsync(byteData, "emotion");

            ShowResults(results);
        }

        private static async Task DetectFaceAttributesAsync(string option)
        {
            var imageFilePath = GetImageFullPath($"Face-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Face API, please wait a moment for the results...");

            var client = new FaceAPIClient();
            var results = await client.DetectAsync(byteData, "age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise");

            ShowResults(results);
        }

        #region -- Auxiliar Methods --

        private static string GetImageFullPath(string imageFileName)
        {
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "Images", imageFileName));
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                return new BinaryReader(fileStream).ReadBytes((int)fileStream.Length);
            }
        }

        private static void ShowResults(string results)
        {
            Console.WriteLine($"{Environment.NewLine}>> API Response:");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine(JsonConvert.DeserializeObject(results));
            Console.WriteLine("--------------------------------------------------------------------------------");
        }

        #endregion -- Auxiliar Methods --

    }
}