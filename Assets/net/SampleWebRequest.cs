using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class SampleWebRequest : MonoBehaviour {
  async void Start() {
    await SendGetRequestAsync();
    await SendPostRequestAsync();
  }

  async Task SendGetRequestAsync() {
    string url = "https://httpbin.org/get";
    Debug.Log($"GET Request started: {url}");

    using (UnityWebRequest request = UnityWebRequest.Get(url)) {
      await request.SendWebRequest();

      if (request.result != UnityWebRequest.Result.Success)
        Debug.LogError($"GET Failed: {request.error}");
      else
        Debug.Log($"GET Response: {request.downloadHandler.text}");
    }
  }

  async Task SendPostRequestAsync() {
    string url = "https://httpbin.org/post";
    string jsonData = "Hello from Unity";
    Debug.Log($"POST Request started: {url}");

    using (UnityWebRequest request = new UnityWebRequest(url, "POST")) {
      byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
      request.uploadHandler = new UploadHandlerRaw(bodyRaw);
      request.downloadHandler = new DownloadHandlerBuffer();
      request.SetRequestHeader("Content-Type", "application/json");

      await request.SendWebRequest();

      if (request.result != UnityWebRequest.Result.Success)
        Debug.LogError($"POST Failed: {request.error}");
      else
        Debug.Log($"POST Response: {request.downloadHandler.text}");
    }
  }
}