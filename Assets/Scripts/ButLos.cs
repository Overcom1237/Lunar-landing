using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButLos : MonoBehaviour
{

    public void Restart()
    {
        Application.LoadLevel("N_Level_1");
    }

    public void Home()
    {
        Application.LoadLevel("main");
    }

}
