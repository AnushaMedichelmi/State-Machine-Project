using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public sealed class  GameController
    {
     private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameController();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
            }

            return instance;
        }
    }

    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints {get {return checkpoints;} }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
