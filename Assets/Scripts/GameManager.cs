using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController player;
    public Transform playerTransform;
    public Transform spawnPosition;
    // Start is called before the first frame update
    //void Awake()
    //{
    //    playerTransform = Instantiate(player.transform, spawnPosition.position, Quaternion.identity);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
