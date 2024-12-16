import React, { useEffect, useState } from 'react';
import Chat from "./Chat"
import { useNavigate } from 'react-router-dom';
import AuthService from '../../services/AuthService'
import { getAllChats } from '../../api/chatRooms'

const ChatRoom = () => {
  const navigate = useNavigate();
  const [chatRooms, setChatRooms] = useState([]);
  const [selectedChat, setSelectedChat] = useState(null);

  useEffect(() => {
    if (!AuthService.getAccessToken()) {
      navigate('/login');
    }
  }, [navigate]);

  useEffect(() => {
    const fetchChats = async () => {
      try {
        const data = await getAllChats();
        setChatRooms(data)
      } catch (error) {
        console.error("Error fetching chats:", error);
      }
    };

    fetchChats();
  }, []);

  return (
    <div className="container">
        <div className="row clearfix">
            <div className="col-lg-12">
                <div className="card chat-app">
                    <div id="plist" className="people-list">
                        <div className="input-group">
                            <div className="input-group mb-3 mt-3">
                                <span className="input-group-text"><i className="fa fa-search"></i></span>
                                <input type="text" className="form-control" placeholder="Search rooms" id="groupInput"/>
                            </div>
                        </div>

                        <ul className="list-unstyled chat-list mt-2 mb-0">
                            {chatRooms.map((value, index) => (
                                <li key={index}
                                    className={`clearfix ${selectedChat === value.id ? "active" : ""}`}
                                    onClick={() => setSelectedChat(value.id)}
                                >
                                    <img src={value.imageUrl} alt="avatar"/>
                                    <div className="about">
                                        <div className="name">{value.name}</div>
                                        <div className="status"> <i className="fa fa-circle online"></i> online </div>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    </div>
                    {selectedChat ? <Chat id={selectedChat}></Chat> : null}
                </div>
            </div>
        </div>
    </div>
  );
};

export default ChatRoom;
