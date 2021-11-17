using System;
using System;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using System.Threading.Tasks;

namespace Form_recognizer
{
    class Program
    {
        private static readonly string endpoint = "https://fundi-form.cognitiveservices.azure.com/";
        private static readonly string apiKey = "d89681d7614c4d03a3380f447e5ffc19";
        private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        static void Main(string[] args)
        {
            FormRecognizerClient recognizerClient = AuthenticateClient();

            Task recognizeContent = RecognizeContent(recognizerClient);
            Task.WaitAll(recognizeContent);
        }

        private static FormRecognizerClient AuthenticateClient()
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new FormRecognizerClient(new Uri(endpoint), credential);
            return client;
        }

        

        private static async Task RecognizeContent(FormRecognizerClient recognizerClient)
        {
            string formUrl = $"C:\\Users\\me\\Downloads\\Documents\\sample-layout.pdf";
            FormPageCollection formPages = await recognizerClient
        .StartRecognizeContentFromUri(new Uri(formUrl))
        .WaitForCompletionAsync();

            foreach (FormPage page in formPages)
            {
                Console.WriteLine($"Form Page {page.PageNumber} has {page.Lines.Count} lines.");

                for (int i = 0; i < page.Lines.Count; i++)
                {
                    FormLine line = page.Lines[i];
                    Console.WriteLine($"    Line {i} has {line.Words.Count} word{(line.Words.Count > 1 ? "s" : "")}, and text: '{line.Text}'.");
                }

                for (int i = 0; i < page.Tables.Count; i++)
                {
                    FormTable table = page.Tables[i];
                    Console.WriteLine($"Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
                    foreach (FormTableCell cell in table.Cells)
                    {
                        Console.WriteLine($"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) contains text: '{cell.Text}'.");
                    }
                }
            }
        }
    }
}
