const express = require("express");
const http = require("http");
const { Server } = require("socket.io");

const app = express();
const server = http.createServer(app);

const io = new Server(server, {
  cors: {
    origin: "*",
  }
});

io.on("connection", (socket) => {
  console.log("Player connected:", socket.id);

  socket.on("joinRoom", (room) => {
    socket.join(room);
    console.log(`Player ${socket.id} joined ${room}`);
  });

  socket.on("move", (data) => {
    socket.to(data.room).emit("move", data);
  });

  socket.on("disconnect", () => {
    console.log("Player disconnected:", socket.id);
  });
});

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log("Server running on port", PORT);
});