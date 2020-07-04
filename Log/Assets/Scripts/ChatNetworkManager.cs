using UnityEngine;
using Mirror;

namespace ChatForStrategy
{
    [AddComponentMenu("")]
    public class ChatNetworkManager : NetworkManager
    {
        [SerializeField]
        private ChatWindow chatWindow = null;
        [SerializeField]
        private GameObject buttons = null;

        public string PlayerName { get; set; }

        public void SetHostname(string hostname)
        {
            networkAddress = hostname;
        }

        public class CreatePlayerMessage : MessageBase
        {
            public string name;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            // Tell the server to create a player with this name.
            conn.Send(new CreatePlayerMessage { name = PlayerName });
        }

        private void OnCreatePlayer(NetworkConnection connection, CreatePlayerMessage createPlayerMessage)
        {
            // Create a gameobject using the name supplied by client.
            GameObject playergo = Instantiate(playerPrefab);
            playergo.GetComponent<Player>().playerName = createPlayerMessage.name;

            // Set it as the player.
            NetworkServer.AddPlayerForConnection(connection, playergo);

            chatWindow.gameObject.SetActive(true);
            buttons.SetActive(true);
        }
    }
}
