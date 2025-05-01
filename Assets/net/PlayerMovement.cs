using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour {
  public float moveSpeed = 5f;
  private Vector2 moveInput;

  public void OnMove(InputAction.CallbackContext context) {
    moveInput = context.ReadValue<Vector2>();
  }

  [Rpc(SendTo.Server)]
  public void MoveRpc(Vector2 move2d) {
    Vector3 move3d = new Vector3(move2d.x, move2d.y, 0f);
    transform.Translate(move3d * moveSpeed * Time.deltaTime);
  }

  void Update() {
    if (IsOwner) {
      MoveRpc(moveInput);
    }
  }
}