using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

// import namespaces
    using Azure;
    using Azure.AI.Translation.Text;


namespace translate_text
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Set console encoding to unicode
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;

                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string translatorRegion = configuration["TranslatorRegion"];
                string translatorKey = configuration["TranslatorKey"];


                // Create client using endpoint and key

// Create client using endpoint and key
AzureKeyCredential credentials = new AzureKeyCredential(aiSvcKey);
Uri endpoint = new Uri(aiSvcEndpoint);
TextAnalyticsClient aiClient = new TextAnalyticsClient(endpoint, credentials);


                // Choose target language rams.cs

// Choose target language
Response<GetLanguagesResult> languagesResponse = await client.GetLanguagesAsync(scope:"translation").ConfigureAwait(false);
GetLanguagesResult languages = languagesResponse.Value;
Console.WriteLine($"{languages.Translation.Count} languages available.\n(See https://learn.microsoft.com/azure/ai-services/translator/language-support#translation)");
Console.WriteLine("Enter a target language code for translation (for example, 'en'):");
string targetLanguage = "xx";
bool languageSupported = false;
while (!languageSupported)
{
    targetLanguage = Console.ReadLine();
    if (languages.Translation.ContainsKey(targetLanguage))
    {
        languageSupported = true;
    }
    else
    {
        Console.WriteLine($"{targetLanguage} is not a supported language.");
    }

}


                // Translate text


                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}
