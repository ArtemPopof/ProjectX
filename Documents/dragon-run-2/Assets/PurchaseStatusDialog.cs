using UnityEngine;
using UnityEngine.UI;

public class PurchaseStatusDialog : MonoBehaviour
{
    public Text message;
    
    public void Show(string messageText, bool successMessage)
    {
        gameObject.SetActive(true);
        message.text = messageText;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}