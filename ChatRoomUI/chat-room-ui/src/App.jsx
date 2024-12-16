import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import Login from "./pages/Login/Login";
import Signup from "./pages/Login/Signup";
import ChatRoom from "./pages/Chat/ChatRoom"
import PageContainder from "./container/PageContainder"


const App = () => {
  return (
    <>
      <Router>
        <PageContainder>
          <Routes>
            <Route path="/" element={<ChatRoom />} />
            <Route path="/login" element={<Login />} />
            <Route path="/signup" element={<Signup />} />
          </Routes>
        </PageContainder>
        
        <ToastContainer />
      </Router>
    </>
  );
};

export default App;