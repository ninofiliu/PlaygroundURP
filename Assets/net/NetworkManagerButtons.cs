using Unity.Netcode;
using UnityEngine;

public class NetworkManagerButtons : MonoBehaviour {

  public void Host() {
    Debug.Log("Starting host...");
    NetworkManager.Singleton.StartHost();
  }

  public void Join() {
    Debug.Log("Joining...");
    NetworkManager.Singleton.StartClient();
  }
}
