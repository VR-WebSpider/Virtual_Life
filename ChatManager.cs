using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public InputField chatInput;
    public Text chatDisplay;

    private ChatClient chatClient;

    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0",
            new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName));
    }

    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void DebugReturn(DebugLevel level, string message) { } 
    public void OnDisconnected() { } 
    public void OnConnected() { } 
    public void OnChatStateChange(ChatState state) { } 
    public void OnGetMessages(string channelName, string[] senders, object[] messages) { } 
    public void OnPrivateMessage(string sender, object message, string channelName) { } 
    public void OnSubscribed(string[] channels, bool[] results) { } 
    public void OnUnsubscribed(string[] channels) { } 
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { } 
    public void OnUserSubscribed(string channel, string user) { } 
    public void OnUserUnsubscribed(string channel, string user) { }
}
