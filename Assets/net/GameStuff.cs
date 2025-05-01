using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameStuff : NetworkBehaviour {
  public TMP_InputField playerIdInput;

  public void SetPlayerId() {
    var player =
        NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerStuff>();
    player.playerName.Value = playerIdInput.text;
  }
}