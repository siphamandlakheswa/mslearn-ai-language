﻿using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

using Azure.AI.Language.QuestionAnswering;
using Azure;

namespace qna_app
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string aiSvcEndpoint = configuration["AIServicesEndpoint"];
                string aiSvcKey = configuration["AIServicesKey"];
                string projectName = configuration["QAProjectName"];
                string deploymentName = configuration["QADeploymentName"];

                // Create client using endpoint and key
                var aiClient = new QuestionAnsweringClient(new Uri(aiSvcEndpoint), new AzureKeyCredential(aiSvcKey));

                // Submit a question and display the answer
                string user_question = "";
                while (user_question.ToLower() != "quit")
                {
                    Console.Write("Question: ");
                    user_question = Console.ReadLine();
                    QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);
                    Response<AnswersResult> response = aiClient.GetAnswers(user_question, project);
                    
                    foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                    {
                        Console.WriteLine($"Answer: {answer.Answer}");
                        Console.WriteLine($"Confidence: {answer.Confidence:P2}");
                        Console.WriteLine($"Source: {answer.Source}");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
