using UnityEngine;

[System.Serializable]
public class MyUser {
  public string name;
  public int age;
}

public class SampleJson : MonoBehaviour {
  void Start() {
    MyUser user = new MyUser() { name = "Nino", age = 27 };
    Debug.Log($"{user.name} {user.age}");
    var stringified = JsonUtility.ToJson(user);
    Debug.Log(stringified);
    var parsed = JsonUtility.FromJson<MyUser>(stringified);
    Debug.Log($"{parsed.name} {parsed.age}");
  }
}