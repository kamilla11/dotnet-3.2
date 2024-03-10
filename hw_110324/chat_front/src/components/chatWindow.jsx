import { ChatMessage } from "./chatMessage";

export const ChatWindow = ({ setUserMessage, sendMessage, chatMessages }) => {
  return (
    <div>
      <div>
        <h2>Chat:</h2>
        <div>
          {chatMessages &&
            chatMessages.map((m, i) => (
              <ChatMessage
                username={m.user}
                message={m.content}
                key={`message-${i}`}
              />
            ))}
        </div>
      </div>
      <div className="window">
        <label>
          <input
            type="text"
            name="chat-message"
            onChange={(e) => setUserMessage(e.target.value)}
          />
        </label>
        <button onClick={sendMessage}>Send</button>
      </div>
    </div>
  );
};
