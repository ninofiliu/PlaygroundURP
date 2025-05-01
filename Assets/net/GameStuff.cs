using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameStuff : NetworkBehaviour {
  public TMP_InputField playerIdInput;

  void Start() {
    if (Application.isBatchMode) {
      Debug.Log("Server env detected, starting NGO server");
      NetworkManager.Singleton.StartServer();
    }
  }

  public void Host() {
    Debug.Log("Starting host...");
    NetworkManager.Singleton.StartHost();
  }

  public void Join() {
    Debug.Log("Joining...");
    NetworkManager.Singleton.StartClient();
  }

  public void SetPlayerId() {
    var player =
        NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerStuff>();
    player.playerName.Value = playerIdInput.text;
  }
}