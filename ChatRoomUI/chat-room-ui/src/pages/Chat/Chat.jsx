import React, { useEffect, useState, useRef  } from 'react';
import { getMessagesByChatId } from '../../api/messages'
import { getAllChats } from '../../api/chatRooms'
import AuthService from '../../services/AuthService';

const Chat = (prop) => {
  const [messages, setMessages] = useState([]);
  const chatHistoryRef = useRef(null);

  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const data = await getMessagesByChatId(prop.id);
        setMessages(data)
      } catch (error) {
        console.error("Error fetching chats:", error);
      }
    };

    fetchMessages();
  }, [prop.id]);

  
  useEffect(() => {
      if (chatHistoryRef.current) {
          chatHistoryRef.current.scrollTop = chatHistoryRef.current.scrollHeight;
      }
  }, [messages]);

  return (
    <div className="chat">
        <div className="chat-header clearfix">
            <div className="row">
                <div className="col-lg-6" id="header-user"></div>
                                <div className="col-lg-6 hidden-sm text-end">
                                    <a href="#" className="btn btn-outline-secondary"><i className="fa fa-camera"></i></a>
                                    <a href="#" className="btn btn-outline-primary"><i className="fa fa-image"></i></a>
                                    <a href="#" className="btn btn-outline-info"><i className="fa fa-cogs"></i></a>
                                    <a href="#" className="btn btn-outline-warning"><i className="fa fa-question"></i></a>
                </div>
            </div>
        </div>
        <div className="chat-history" ref={chatHistoryRef}>
            <ul className="m-b-0">
                {messages.map((value, index) => (
                    value.sender === AuthService.getUsername()
                        ?
                    <li key={index} className="clearfix">
                        <div className="message-data text-end">
                            <span className="message-data-time">{new Date(value.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}, You</span>
                        </div>
                        <div className="message other-message float-right">{value.content}</div>
                    </li>
                        : 
                    <li key={index} className="clearfix">
                        <div className="message-data">
                            <span className="message-data-time">{value.sender}, {new Date(value.sentAt).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</span>
                        </div>
                        <div className="message my-message">{value.content}</div>                                    
                    </li>
                ))}
            </ul>
        </div>
        <div className="chat-message clearfix">
            <div className="input-group mb-3">
                <span className="input-group-text"><i className="fa fa-send"></i></span>
                <input type="text" className="form-control" placeholder="Enter text here..." id="messageInput"/>
            </div>
        </div>
    </div>
  );
};

export default Chat;
