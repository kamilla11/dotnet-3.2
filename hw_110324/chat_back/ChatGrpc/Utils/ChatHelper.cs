using Grpc.Core;

namespace ChatGrpc.Services;

public interface IChatHelper
{
    public void Subscribe(IServerStreamWriter<ChatMessageResponse> stream);

    public void Unsubscribe(IServerStreamWriter<ChatMessageResponse> stream);

    public Task SendMessageAsync(ChatMessageResponse message);
}
public class ChatHelper: IChatHelper
{
    private readonly List<IServerStreamWriter<ChatMessageResponse>> _streams = new();

    public void Subscribe(IServerStreamWriter<ChatMessageResponse> stream) => _streams.Add(stream);
    
    public void Unsubscribe(IServerStreamWriter<ChatMessageResponse> stream) => _streams.Remove(stream);

    public async Task SendMessageAsync(ChatMessageResponse message)
    {
        await Parallel.ForEachAsync(_streams, async (stream, ctx) =>
        {
            await stream.WriteAsync(message, ctx);
        });
    }
}