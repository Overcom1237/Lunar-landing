using UnityEngine;
using UnityEngine.UI;
using System;

public class go : MonoBehaviour
{
    public GameObject Fi;
    public GameObject pLost;
    public Text text;
    private double speed = 0;
    private double speadb = 0;
    private double speedt = 0;
    private bool game = true;
    private object index;

    void Awake()
    {
    }

    void FixedUpdate()
    {
        if (game)
        {    
            speed += 0.03 + speedt;

            transform.position -= new Vector3(float.Parse(Convert.ToString(speadb)) * Time.deltaTime, float.Parse(Convert.ToString(speed)) * Time.deltaTime, 0);
        }
    }

    public void OnDownUp()
    {
        Fi.SetActive(true);
        speedt = -0.1;
    }

    public void OnDownDw()
    {
        speedt = 0.1;
    }
    public void Stop()
    {
        Fi.SetActive(false);
        speedt = 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        pLost.SetActive(true);
        if (other.tag == "Player" && speed <= 0.7)
        {
            text.text = "You Win";
        }
        else
        {
            text.text = "You Lost";
        }
        game = false;
    }
}
