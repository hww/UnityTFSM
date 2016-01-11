using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Display : MonoBehaviour {

    public Text displayText;

    public string text
    {
        get { return displayText.text; }
        set { displayText.text = value; }
    }

    static Display instance;
    public static Display I {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<Display>();
            return instance;
        }
    }
}
