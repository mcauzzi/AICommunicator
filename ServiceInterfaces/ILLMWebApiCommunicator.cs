using System.Threading.Tasks;
using ChatDtos;
using InternalDtos;

namespace ServiceInterfaces;

public interface ILLMWebApiCommunicator
{
    Task<ChatResponse?> SendChatRequest(string input);
}