using UnityEngine;

public class DeathDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "NPC") {SoundManager.Instance.playEffect("Death"); GameManager.Instance.GameOver();}
    }
}
