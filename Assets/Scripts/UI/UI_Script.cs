using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public TMP_Text txt;

    void Update()
    {
        
    }

    public void TextChange(string a)
    {
        txt.text = a;
    }
}
