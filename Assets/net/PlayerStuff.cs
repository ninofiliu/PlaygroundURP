using System.IO;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerData {
  public float x;
  public float y;
}

public class PlayerStuff : NetworkBehaviour {
  public float moveSpeed = 5f;
  public NetworkVariable<FixedString64Bytes> playerName =
      new NetworkVariable<FixedString64Bytes>(
          readPerm: NetworkVariableReadPermission.Everyone,
          writePerm: NetworkVariableWritePermission.Owner);

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

  [Rpc(SendTo.Server)]
  public void LoadRpc() {
    if (!IsServer)
      return;

    string path = Path.Combine(Application.persistentDataPath,
                               $"{playerName.Value}.json");
    if (File.Exists(path)) {
      Debug.Log($"found save file for {playerName.Value}");
      string json = File.ReadAllText(path);
      PlayerData data = JsonUtility.FromJson<PlayerData>(json);
      Debug.Log(
          $"current {transform.position}, will update to {data.x},{data.y}");
      transform.position = new Vector3(data.x, data.y, 0);
      Debug.Log($"updated position {transform.position}");
    } else {
      Debug.Log($"No save file found for {playerName.Value}");
    }
  }

  [Rpc(SendTo.Server)]
  public void SaveRpc() {
    if (!IsServer)
      return;

    Debug.Log("Saving...");
    PlayerData saveData =
        new PlayerData { x = transform.position.x, y = transform.position.y };
    string json = JsonUtility.ToJson(saveData);
    string path = Path.Combine(Application.persistentDataPath,
                               $"{playerName.Value}.json");
    File.WriteAllText(path, json);
    Debug.Log($"file saved to {path}");
  }

  public override void OnNetworkSpawn() {
    base.OnNetworkSpawn();
    Debug.Log($"network spawn {playerName.Value}");
    playerName.OnValueChanged +=
        (FixedString64Bytes oldname, FixedString64Bytes newname) => {
          Debug.Log($"player renamed {oldname} -> {newname}");
          GetComponentInChildren<TMP_Text>().text = newname.ToString();
        };
    GetComponentInChildren<TMP_Text>().text = playerName.Value.ToString();
  }

  public void Start() { Debug.Log($"start {playerName.Value}"); }
}