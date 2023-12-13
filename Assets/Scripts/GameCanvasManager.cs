/*AUTHOR: YUSEF*/

using TMPro;
using UnityEngine;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateText;
    void Update()
    {
        string text = "Score: " + GameManager.Instance.score + "\nEnemy Speed: " + GameManager.Instance.enemySpeed;
        stateText.text = text;
    }
}