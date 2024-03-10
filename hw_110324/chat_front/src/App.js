import "./App.css";
import { ChatClient } from "./protos/chat_grpc_web_pb";
import { JwtServiceClient } from "./protos/jwt_grpc_web_pb";
import { useState } from "react";
import { Request } from "./protos/jwt_pb";
import { ChatMessageRequest } from "./protos/chat_pb";
import google_protobuf_empty_pb from "google-protobuf/google/protobuf/empty_pb";
import { EnterWindow } from "./components/enterWindow";
import { ChatWindow } from "./components/chatWindow";

const chatClient = new ChatClient("http://localhost:8080");
const jwtClient = new JwtServiceClient("http://localhost:8080");

function App() {
  const [userName, setUserName] = useState("");
  const [accessToken, setAccessToken] = useState("");
  const [joined, setJoined] = useState(false);
  const [chatMessages, setChatMessages] = useState([]);
  const [userMessage, setUserMessage] = useState("");

  const messages = [];

  const authorize = () => {
    const request = new Request();
    request.setUsername(userName);
    jwtClient.getJwt(request, {}, (err, response) => {
      if (err) {
        console.log("err: ", err);
        return;
      }
      setAccessToken(response.toObject().token);
    });
  };

  const loadHistory = () => {
    const metadata = { Authorization: `Bearer ${accessToken}` };
    chatClient.joinChat(
      new google_protobuf_empty_pb.Empty(),
      metadata,
      (err, response) => {
        if (err) {
          console.log("err: ", err);
          return;
        }
        const history = response.toObject().messagesList;
        console.log(history);
        messages.push(...history);
        setChatMessages([...messages]);
      }
    );
  };

  const joinChat = () => {
    setJoined(true);
    loadHistory();
    receiveMessages();
  };

  const receiveMessages = () => {
    const metadata = { Authorization: `Bearer ${accessToken}` };
    const responseStream = chatClient.startReceivingMessages(
      new google_protobuf_empty_pb.Empty(),
      metadata
    );
    responseStream.on("data", (response) => {
      const dto = response.toObject();
      console.log(dto);
      messages.push(dto);
      setChatMessages([...messages]);
    });

    responseStream.on("end", () => {
      console.log("bye");
    });
  };

  const sendMessage = () => {
    const request = new ChatMessageRequest();
    request.setContent(userMessage);
    const metadata = { Authorization: `Bearer ${accessToken}` };
    chatClient.sendChatMessage(request, metadata, (err) => {
      if (err) {
        console.log("err: ", err);
      }
    });
  };

  return (
    <div className="mainPage">
      {!joined && (
        <EnterWindow
          accessToken={accessToken}
          authorize={authorize}
          joinChat={joinChat}
          setUserName={setUserName}
        />
      )}
      {joined && (
        <ChatWindow
          setUserMessage={setUserMessage}
          sendMessage={sendMessage}
          chatMessages={chatMessages}
        />
      )}
    </div>
  );
}

export default App;
