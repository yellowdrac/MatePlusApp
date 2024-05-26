using UnityEngine;

public class ZoneEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.SetTargetScreen(eScreen.Final);
            SceneLoader.Instance.ChangeScreen(eScreen.Loading, true);
        }
    }
}