using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableLook : MonoBehaviour
{
    public List<PlayerMotor> models;
    public PlayerMotor CurrentModel { get; private set; }

    void Awake()
    {
        // Init current character look
        var index = PlayerPrefs.GetInt("characterLook");
        CurrentModel = models[index];
        MakeAnotherModelsInactive(index);
    }

    private void MakeAnotherModelsInactive(int currentModel)
    {
        for (var i = 0; i < models.Count; i++)
        {
            models[currentModel].gameObject.SetActive(false);
        }
        models[currentModel].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
