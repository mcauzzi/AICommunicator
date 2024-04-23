using System.Threading.Tasks;
using ChatDtos;

namespace ServiceInterfaces;

public interface IWebApiCommunicator
{
    Task<ChatResponse?> SendChatRequest( string input);
}