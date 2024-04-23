public class ChatResponse
{
    public string Id           { get; set; }
    public string Type         { get; set; }
    public bool   Close        { get; set; }
    public object Error        { get; set; }
    public int    ChatId       { get; set; }
    public string TextResponse { get; set; }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(Type)}: {Type}, {nameof(Close)}: {Close}, {nameof(Error)}: {Error}, {nameof(ChatId)}: {ChatId}, {nameof(TextResponse)}: {TextResponse}";
    }
}