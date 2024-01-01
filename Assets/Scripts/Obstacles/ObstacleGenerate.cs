using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerate : MonoBehaviour
{
    public ObstacleManager obstacleManager;
    private Animator animator;
    private GameObject obsChild;
    private List<GameObject> objects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        obsChild = GetComponent<GameObject>();
        foreach(Obstacle obstacle in obstacleManager.obstacles)
        {
            if (gameObject.tag == obstacle.type)
                objects.Add(obstacle.obs);
                
        }
        int randomIndex = Random.Range(0, objects.Count);
        GameObject go = Instantiate(objects[randomIndex]);
        go.transform.position = gameObject.transform.position;
        try
        {
            animator = go.GetComponent<Animator>();
            animator.SetTrigger("TeslaOpen");
        }
        catch
        {
            
        }
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
