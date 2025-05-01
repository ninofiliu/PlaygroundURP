using Cinemachine;
using StarterAssets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetMove : NetworkBehaviour {

  public PlayerInput playerInput;
  public StarterAssetsInputs starterAssetsInputs;
  public ThirdPersonController thirdPersonController;
  public Transform follow;

  private void Awake() {
    print("awake");
    playerInput.enabled = false;
    starterAssetsInputs.enabled = false;
    thirdPersonController.enabled = false;
  }

  public override void OnNetworkSpawn() {
    base.OnNetworkSpawn();
    Debug.Log($"network spawn {IsOwner} {IsServer}");
    if (IsOwner) {
      playerInput.enabled = true;
      starterAssetsInputs.enabled = true;
      var cam = FindObjectsByType<CinemachineVirtualCamera>(
          FindObjectsSortMode.None)[0];
      cam.Follow = follow;
    }
    if (IsServer) {
      thirdPersonController.enabled = true;
    }
  }

  [Rpc(SendTo.Server)]
  private void UpdateInputServerRpc(Vector2 move, Vector2 look, bool jump,
                                    bool sprint) {
    Debug.Log("rpc");
    starterAssetsInputs.MoveInput(move);
    starterAssetsInputs.LookInput(look);
    starterAssetsInputs.JumpInput(jump);
    starterAssetsInputs.SprintInput(sprint);
  }

  private void LateUpdate() {
    if (!IsOwner)
      return;
    UpdateInputServerRpc(starterAssetsInputs.move, starterAssetsInputs.look,
                         starterAssetsInputs.jump, starterAssetsInputs.sprint);
  }
}
