// See https://aka.ms/new-console-template for more informati

using System.Net.Http.Json;
using AnythingLLMComunicator;


const string WEB_API_KEY       = "3E1D9MW-3EC4GGA-PJ3ATGK-TGVK6RD";

var        chatUrl       = "api/v1/workspace/berlusconilolbot/chat";
var        speechService = new AzureSpeech();
var        webApiComms   = new WebApiCommunicator("http://localhost:3001",WEB_API_KEY);
ConsoleKey lastSelectedInput;

while (true)
{
    Console.WriteLine("Premi qualsiasi Tasto per procedere con il riconoscimento vocale");
    lastSelectedInput = Console.ReadKey().Key;
    if (lastSelectedInput==ConsoleKey.Q)
    {
        return;
    }
    string inputMessage =await  speechService.GetMessage();
    var    response     =await webApiComms.SendChatRequest(chatUrl, inputMessage);
    Console.WriteLine(response);
    await speechService.SpeakText(response.TextResponse);
}


