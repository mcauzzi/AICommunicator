using System.Threading.Tasks;
using ChatDtos;

namespace ServiceInterfaces;

public interface ILLMWebApiCommunicator
{
    Task<ChatResponse?> SendChatRequest( string input);
}