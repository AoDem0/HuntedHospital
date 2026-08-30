using UnityEngine;

public class SpawnMusicMan : MonoBehaviour
{
    [SerializeField] private GameObject musicManPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (musicManPrefab != null)
        {
            Instantiate(musicManPrefab);
            Destroy(gameObject);
        }
    }
}
