using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class but : MonoBehaviour
{
    public void Play()
    {
        Application.LoadLevel("Choice");
    }

    public void Normal()
    {
        Application.LoadLevel("N_Level_1");
    }

    public void Classic()
    {
        Application.LoadLevel("C_Level_1");
    }

    public void Retro()
    {
        Application.LoadLevel("R_Level_1");
    }

}
