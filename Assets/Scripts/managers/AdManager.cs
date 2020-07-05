using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager
{
    public static string AFTER_DEATH_ADS_SHOWN = "after_death_ads";
    public static int MAX_RESUME_AD_COUNT = 2;

    public AdManager()
    {
        PlayerPrefs.SetInt(AFTER_DEATH_ADS_SHOWN, 0);
    }

    public static bool CantShowAnotherResumeAd()
    {
        var shownCount = PlayerPrefs.GetInt(AFTER_DEATH_ADS_SHOWN);
        shownCount++;

        if (shownCount >= MAX_RESUME_AD_COUNT)
        {
            return true;
        }

        // update shown count
        PlayerPrefs.SetInt(AFTER_DEATH_ADS_SHOWN, shownCount);

        return false;
    }
}
