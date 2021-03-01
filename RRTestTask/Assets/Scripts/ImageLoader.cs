using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private Image artwork;
    private string url = "https://picsum.photos/300";

    void Start()
    {
        StartCoroutine(DownloadImage(url));
    }

    private IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log($"{ request.error }, URL:{ request.url }");
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite spr = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(300f, 300f)), Vector2.zero);
            artwork.sprite = spr;
        }
    }
}