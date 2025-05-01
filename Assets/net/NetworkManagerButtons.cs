using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerButtons : MonoBehaviour {
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
}
