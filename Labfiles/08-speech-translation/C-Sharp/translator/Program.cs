﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;

// Import namespaces
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;

// Alternatively, use audio input from a file
using System.Media;

namespace speech_translation
{
    class Program
    {
        private static SpeechConfig speechConfig;
        private static SpeechTranslationConfig translationConfig;

        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string aiSvcKey = configuration["SpeechKey"];
                string aiSvcRegion = configuration["SpeechRegion"];

                // Set console encoding to unicode
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;
                
                // Configure translation
                translationConfig = SpeechTranslationConfig.FromSubscription(aiSvcKey, aiSvcRegion);
                translationConfig.SpeechRecognitionLanguage = "en-US";
                translationConfig.AddTargetLanguage("fr");
                translationConfig.AddTargetLanguage("es");
                translationConfig.AddTargetLanguage("hi");
                translationConfig.AddTargetLanguage("zu"); // Add Zulu as a target language
                Console.WriteLine("Ready to translate from " + translationConfig.SpeechRecognitionLanguage);

                // Configure speech
                speechConfig = SpeechConfig.FromSubscription(aiSvcKey, aiSvcRegion);

                string targetLanguage = "";
                while (targetLanguage != "quit")
                {
                    // Below, add the target language to the list of languages, as ZULU.
                    Console.WriteLine("\nEnter a target language\n fr = French\n es = Spanish\n hi = Hindi \n zu = Zulu \n Enter anything else to stop\n");
                    targetLanguage=Console.ReadLine().ToLower();
                    if (translationConfig.TargetLanguages.Contains(targetLanguage))
                    {
                        await Translate(targetLanguage);
                    }
                    else
                    {
                        targetLanguage = "quit";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task Translate(string targetLanguage)
        {
            string translation = "";

            // Translate speech
            using AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using TranslationRecognizer translator = new TranslationRecognizer(translationConfig, audioConfig);

            Console.WriteLine("Speak now...");
            TranslationRecognitionResult result = await translator.RecognizeOnceAsync();

            Console.WriteLine($"Translating '{result.Text}'");
            translation = result.Translations[targetLanguage];

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(translation);

             // Translate speech, from Audio file
             string audioFile = "station.wav";
            SoundPlayer wavPlayer = new (audioFile);
            wavPlayer.Play();

            using AudioConfig audioConfig2 = AudioConfig.FromWavFileInput(audioFile);
            using TranslationRecognizer translator2 = new TranslationRecognizer(translationConfig, audioConfig2);

            Console.WriteLine("Getting speech from file...");
            TranslationRecognitionResult result2 = await translator2.RecognizeOnceAsync();

            Console.WriteLine($"Translating '{result2.Text}'");
            translation = result2.Translations[targetLanguage];

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(translation);

             // Synthesize translation
             var voices = new Dictionary<string, string>
                 {
                     ["fr"] = "fr-FR-HenriNeural",
                     ["es"] = "es-ES-ElviraNeural",
                     ["hi"] = "hi-IN-MadhurNeural",
                     ["zu"] = "zu-ZA-BernardaNeural" // Add Zulu voice.
                 };                 
                 
            speechConfig.SpeechSynthesisVoiceName = voices[targetLanguage];
            using SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig);
            SpeechSynthesisResult speak = await speechSynthesizer.SpeakTextAsync(translation);
            if (speak.Reason != ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine(speak.Reason);
            }
        }

    }
}
