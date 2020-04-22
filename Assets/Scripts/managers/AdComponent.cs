using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdComponent : MonoBehaviour
{
    public void ShowAdToResume()
    {
        if (!Advertisement.IsReady())
        {
            return;
        }

        Advertisement.Show();
    }
}
