using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace ChatGrpc.Services;

[Authorize]
public class ChatService : Chat.ChatBase
{
    private readonly IChatHelper _chatHelper;
    private static readonly List<ChatMessageResponse> ChatHistory = new();

    public ChatService(IChatHelper chatHelper)
    {
        _chatHelper = chatHelper;
    }

    public override Task<ChatHistory> JoinChat(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new ChatHistory
        {
            Messages = { ChatHistory }
        });
    }

    public override async Task StartReceivingMessages(Empty request, IServerStreamWriter<ChatMessageResponse> responseStream, ServerCallContext context)
    {
        _chatHelper.Subscribe(responseStream);
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500);
        }
    
        _chatHelper.Unsubscribe(responseStream);
    }

    public override async Task<Empty> SendChatMessage(ChatMessageRequest request, ServerCallContext context)
    {
        var messageResponse = new ChatMessageResponse
        {
            User = context.GetHttpContext().User.Identity?.Name,
            Content = request.Content
        };
        ChatHistory.Add(messageResponse);
        await _chatHelper.SendMessageAsync(messageResponse);
        return new Empty();
    }
}