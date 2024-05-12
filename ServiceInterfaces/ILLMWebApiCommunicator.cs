using System.Threading.Tasks;
using ChatDtos;
using Models;

namespace ServiceInterfaces;

public interface ILLMWebApiCommunicator
{
    Task<ChatResponse?> SendChatRequest(string input);
}