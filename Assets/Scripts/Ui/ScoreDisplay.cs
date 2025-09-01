using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Update()
    {
        scoreText.text = $"Score: {GameControlller.Instance.score}";
    }
}
