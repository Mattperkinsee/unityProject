using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    private int enemiesKilled = 0;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        UpdateEnemyCounterText();
    }

    public void IncrementEnemyCount()
    {
        enemiesKilled++;
        UpdateEnemyCounterText();
    }

    public int GetEnemiesKilled(){
        return enemiesKilled; 
    }

    private void UpdateEnemyCounterText()
    {
        textMeshPro.text = "Enemies Killed: " + enemiesKilled.ToString();
    }
}
