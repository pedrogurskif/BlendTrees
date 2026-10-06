using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset;
    public GameObject player;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
