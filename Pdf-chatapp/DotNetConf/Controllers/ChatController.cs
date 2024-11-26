using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetConf.Models;
using Azure.AI.OpenAI;
using Azure;
using iText.Kernel.Pdf;
using System.Text;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Forms.Xfdf;

namespace DotNetConf.Controllers
{
    public class ChatController : Controller
    {
        string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
        string key = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY");
        string model = Environment.GetEnvironmentVariable("AZURE_OPENAI_MODEL");
      

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse(string userMessage)
        {
            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(model))
            {
                return BadRequest("Azure OpenAI environment variables are not properly configured.");
            }

            OpenAIClient client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

            var chatCompletionOptions = new ChatCompletionsOptions()
            {
                Messages = {
                    new ChatMessage(ChatRole.System, "You are a helpful Assistant"),
                    new ChatMessage(ChatRole.User, "Does Azure OpenAI support GPT-4?"),
                    new ChatMessage(ChatRole.Assistant, "Yes it does"),
                    new ChatMessage(ChatRole.User, userMessage),
                },
                MaxTokens = 400
            };

            Response<ChatCompletions> response = await client.GetChatCompletionsAsync(deploymentOrModelName: model, chatCompletionOptions);
            var botResponse = response.Value.Choices.First().Message.Content;

            return Json(new { Response = botResponse });
        }
    

    [HttpPost]
    public async Task<IActionResult> GetResponseFromPdf(string userMessage)
    {
        OpenAIClient client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
        string pdfText =  GetText("Data/maahi.pdf");

        var chatCompletionOptions = new ChatCompletionsOptions(){
            Messages ={
                    new ChatMessage(ChatRole.System, "You are a helpful Assistant"),
                    new ChatMessage(ChatRole.User, "The following information is from the PDF text" + pdfText),
                    new ChatMessage(ChatRole.User, userMessage),
            },
            MaxTokens = 1000,
            Temperature = 0
        };

        Response<ChatCompletions> response = client.GetChatCompletions(deploymentOrModelName: model, chatCompletionOptions);

        var botResponse = response.Value.Choices.First().Message.Content;

        return Json(new {Response = botResponse});
    }

    private static string GetText(string pdfFilePath)
    {
        PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfFilePath));

        StringBuilder text = new StringBuilder();

        for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
        {
            PdfPage pdfPage = pdfDoc.GetPage(page);

            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            string currentText = PdfTextExtractor.GetTextFromPage(pdfPage, strategy);
            text.Append(currentText);
        }
        pdfDoc.Close();
        return text.ToString();
    }
    }
}