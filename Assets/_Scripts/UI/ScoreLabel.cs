using TMPro;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;


    private void Update() => scoreLabel.SetText($"{ScoreManager.Instance.GetScore()}");
}
