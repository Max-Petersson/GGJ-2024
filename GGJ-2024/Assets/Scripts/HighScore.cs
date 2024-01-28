using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_shighscoretext;
    // Start is called before the first frame update
    private void OnEnable()
    {
        EventManager.Instance.OnScoreChanged += UpdateHighScore;
    }
    private void OnDisable()
    {
        EventManager.Instance.OnScoreChanged -= UpdateHighScore;
    }
    void UpdateHighScore(int highscore)
    {
        m_shighscoretext.text = highscore.ToString();
    }
}
