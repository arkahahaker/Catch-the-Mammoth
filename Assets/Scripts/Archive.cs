using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archive : MonoBehaviour
{

    public List<Sprite> Obstacles;
    public Sprite FreeCage;
    public Sprite Nothing;
    public Sprite Mammoth;

    void Start()
    {
        if (Game.Archive == null) Game.Archive = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public Sprite RandomObstacle () {
        return Obstacles[new System.Random().Next(0, Obstacles.Count)];
    }

    public Sprite RandomObstacle(System.Random rand) {
        return Obstacles[rand.Next(0, Obstacles.Count)];
    }

}