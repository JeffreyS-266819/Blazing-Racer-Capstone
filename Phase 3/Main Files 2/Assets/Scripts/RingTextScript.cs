using TMPro;
using UnityEngine;

public class RingTextScript : MonoBehaviour
{
    public TMP_Text TimerTimeText;
    public TMP_Text CheckPointText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckPointText.text != TimerTimeText.text)
        {
            CheckPointText.text = TimerTimeText.text;
        }
    }
}
