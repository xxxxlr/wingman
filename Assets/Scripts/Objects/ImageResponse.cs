using UnityEngine;

[System.Serializable]
public class ImageResponse
{
    public string name;
    public float score;

    public static ImageResponse CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ImageResponse>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.

}