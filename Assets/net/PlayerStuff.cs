using System.IO;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerSaveData {
  public float x;
  public float y;
}

public class PlayerStuff : NetworkBehaviour {
  public float moveSpeed = 5f;
  public string playerId;
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

  public void Load() {
    if (!IsServer)
      return;

    string path =
        Path.Combine(Application.persistentDataPath, $"{playerId}.json");
    if (File.Exists(path)) {
      Debug.Log($"found save file for {playerId}");
      string json = File.ReadAllText(path);
      PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
      transform.position.Set(data.x, data.y, 0);
    } else {
      Debug.Log($"No save file found for {playerId}");
    }
  }

  public void Save() {
    if (!IsServer)
      return;

    PlayerSaveData saveData = new PlayerSaveData { x = transform.position.x,
                                                   y = transform.position.y };
    string json = JsonUtility.ToJson(saveData);
    string path =
        Path.Combine(Application.persistentDataPath, $"{playerId}.json");
    File.WriteAllText(path, json);
  }

  public override void OnNetworkSpawn() {
    base.OnNetworkSpawn();
    playerName.OnValueChanged +=
        (FixedString64Bytes oldname, FixedString64Bytes newname) => {
          Debug.Log($"player renamed {oldname} -> {newname}");
          GetComponentInChildren<TMP_Text>().text = newname.ToString();
        };
  }
}