using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;
    public Quaternion attackDirection;
    public Rect checkerSpawnArea;
    public int checkerCount = 0;

    internal void Destroy()
    {
        Game.GetInstance().playerManager.CheckRemainingPlayers();
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
