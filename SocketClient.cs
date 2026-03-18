using UnityEngine;
using SocketIOClient;
using System;
using System.Threading.Tasks;

public class SocketClient : MonoBehaviour
{
  private SocketIO client;

  async void Start()
  {
    // 🔥 Replace with your Railway URL
    client = new SocketIO("https://ludopvpserver-production.up.railway.app", new SocketIOOptions
    {
      Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
    });

    /*client = new SocketIO("https://ludopvpserver-production.up.railway.app", new SocketIOOptions
    {
      Transport = SocketIOClient.Transport.TransportProtocol.Polling, // 🔥 CHANGE THIS
      EIO = 4
    });*/

    // ✅ Connected
    client.OnConnected += (sender, e) =>
    {
      Debug.Log("✅ Connected to server");

      // Join room after connect
      client.EmitAsync("joinRoom", "room1");
    };

    // 🎮 Receive move from opponent
    client.On("move", response =>
    {
      var data = response.GetValue<string>();
      Debug.Log("Opponent Move: " + data);

      // TODO: Apply move in game
    });

    // ❌ Disconnected
    client.OnDisconnected += (sender, e) =>
    {
      Debug.Log("❌ Disconnected");
    };

    // 🎮 Receive move from opponent
    client.On("joinRoom", response =>
    {
      Debug.Log("joinRoom");
    });

    await client.ConnectAsync();
  }

  // 🎯 Send move to server
  public async void SendMove(string moveData)
  {
    await client.EmitAsync("move", new
    {
      room = "room1",
      move = moveData
    });

    Debug.Log("📤 Sent Move: " + moveData);
  }

  private async void OnApplicationQuit()
  {
    if (client != null)
    {
      await client.DisconnectAsync();
    }
  }

  [ContextMenu("Test-Send-Move")]
  public void TestSendMove()
  {
    SendMove("dice:6");
    //FindObjectOfType<SocketClient>().SendMove("dice:6");
  } 
}