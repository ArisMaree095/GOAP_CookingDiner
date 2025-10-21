using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldVariables : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI worldtext;

    // Update is called once per frame
    void LateUpdate()
    {
        updateText();
    }
    void updateText()
    {
        string s = "";
        Dictionary<string, int> dic = GWorld.Instance.GetWorld().GetStates();
        foreach (KeyValuePair<string, int> d in dic)
        {
            s += d.Key + ": " + d.Value + ". \n";
        }
        worldtext.text = s;
    }
}
