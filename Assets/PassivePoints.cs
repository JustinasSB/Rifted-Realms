using TMPro;
using UnityEngine;

public class PassivePoints : MonoBehaviour
{
    [SerializeField] TMP_Text passivePoints;
    void Start()
    {
        LevelManager.level.OnPassivePointChange += HandleChange;
    }
    private void HandleChange(int points)
    {
        passivePoints.text = points.ToString();
    }
    
}
