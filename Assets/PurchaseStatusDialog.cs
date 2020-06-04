using UnityEngine;
using UnityEngine.UI;

public class PurchaseStatusDialog : MonoBehaviour
{
    public Text message;
    
    public void Show(string messageText, bool successMessage)
    {
        message.text = messageText;
    }

}