using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BF.Demo.ComputerVisionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("================================================================================");
                Console.WriteLine("== Baufest Demo - Computer Vision API                                         ==");
                Console.WriteLine("================================================================================");

                Console.Write(">> Choose an option [ A | B | C | D | E | F | G | H ]: ");
                var option = Console.ReadLine().ToUpperInvariant();

                switch (option)
                {
                    case "A":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to analyze a selfie...");
                        AnalyzeImageAsync(option).Wait();
                        break;
                    case "B":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to detect a monument...");
                        AnalyzeImageAsync(option).Wait();
                        break;
                    case "C":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to detect a celebrity...");
                        AnalyzeImageAsync(option).Wait();
                        break;
                    case "D":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to detect a drawing...");
                        AnalyzeImageAsync(option).Wait();
                        break;
                    case "E":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to describe an image...");
                        DescribeImageAsync(option).Wait();
                        break;
                    case "F":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to get text from a printed page...");
                        OCRAsync(option).Wait();
                        break;
                    case "G":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize text from a printed page...");
                        RecognizeTextAsync(option, "Printed").Wait();
                        break;
                    case "H":
                        Console.WriteLine($"{Environment.NewLine}>> Trying to recognize text from a handwritting note...");
                        RecognizeTextAsync(option, "Handwritten").Wait();
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

        private static async Task AnalyzeImageAsync(string option)
        {
            var imageFilePath = GetImageFullPath($"Vision-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Computer Vision API, please wait a moment for the results...");

            var client = new ComputerVisionAPIClient();
            var results = await client.AnalyzeImageAsync(byteData, "Categories,Tags,Description,Faces,ImageType,Color,Adult", "Celebrities,Landmarks");

            ShowResults(results);
        }

        private static async Task DescribeImageAsync(string option)
        {
            var imageFilePath = GetImageFullPath($"Vision-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Computer Vision API, please wait a moment for the results...");

            var client = new ComputerVisionAPIClient();
            var results = await client.DescribeImageAsync(byteData);

            ShowResults(results);
        }

        private static async Task OCRAsync(string option)
        {
            var imageFilePath = GetImageFullPath($"Vision-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Computer Vision API, please wait a moment for the results...");

            var client = new ComputerVisionAPIClient();
            var results = await client.OCRAsync(byteData);

            ShowResults(results);
        }

        private static async Task RecognizeTextAsync(string option, string mode)
        {
            var imageFilePath = GetImageFullPath($"Vision-{option}.jpg");
            var byteData = GetImageAsByteArray(imageFilePath);

            Console.WriteLine($"{Environment.NewLine}>> Calling Computer Vision API, please wait a moment for the results...");

            var client = new ComputerVisionAPIClient();
            var results = await client.RecognizeTextAsync(byteData, mode);

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
