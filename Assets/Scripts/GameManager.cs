using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public Transform spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player.transform, spawnPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
