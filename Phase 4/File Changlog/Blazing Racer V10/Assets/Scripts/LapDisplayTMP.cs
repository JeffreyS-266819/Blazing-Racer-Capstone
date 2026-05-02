using UnityEngine;
using TMPro;

public class LapDisplayTMP : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI lapText;
    public RaceManager raceManager;

    void Start()
    {
        // Optional safety check
        if (raceManager == null)
        {
            raceManager = FindObjectOfType<RaceManager>();
        }
    }

    void Update()
    {
        if (raceManager == null || lapText == null) return;

        lapText.text = $"Lap {raceManager.currnetLapP} / {raceManager.totalLapsP}";
    }
}