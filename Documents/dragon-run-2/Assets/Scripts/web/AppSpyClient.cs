using System.Net.Http;
using System.Text;
using UnityEngine;

public class JounalEntry : MonoBehaviour
{
    public string app;
    public string action;
    public string data;
}
class AppSpyClient
{
    private static readonly HttpClient client = new HttpClient();

    public static void JornalAction(string action, string data)
    {
        var requestData = new JounalEntry
        {
            app = "Dragon Run 2",
            action = action,
            data = data
        };

        var json = JsonUtility.ToJson(requestData);
        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        //client.PostAsync(Private.APPSPY_URL, stringContent);
    }
}
