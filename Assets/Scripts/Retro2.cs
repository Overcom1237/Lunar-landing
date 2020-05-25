using UnityEngine;
using System;
using UnityEngine.UI;

public class Retro2 : MonoBehaviour
{
    public Text tex;
    public Text wal;
    public GameObject Input;
    double h_i, x_i, v_i, u_i, mf_i, ms, a_lim, d_mf, t, ac, g, nf, ls, a, q, v_i1, u_i1;
    public int FlightMode = 1;
    public bool WillReverseOfControl = false;
    public bool WillQuitProgram = false;
    public int i = 0;

    void Start()
    {
        tex.text = "Lunar ship simple simulator";

        g = 1.62;
        ms = 2250;
        nf = 3660;
        a = 0;
        a_lim = 3 * 9.81;
        v_i = 0;
        u_i = 0;
        h_i = 1000;
        x_i = 0;
        mf_i = 400;
        ls = 3600;

    }

    public void But()
    {
        if (i == 0)
        {
            if (h_i < 0)
            {
                t = 2 * h_i / (Math.Sqrt(Math.Pow(u_i, 2) + 2 * h_i * (g - a * Math.Cos(ac))) - u_i);

                DoStep();
                i = 0;
                But();
            }
            else if (h_i == 0)
            {
                Win();
            }
            else
            {
                if ((h_i == 0) | ((a < a_lim) & (mf_i != 0)))
                {
                    OutInfo();
                    tex.text +=("\n Enter in-flight maneuver");
                    tex.text +=("\n Fuel consumption, kg: ");
                    Input.SetActive(true);
                    i++;
                }
                else
                {
                    i = 2;
                }
            }
        }
        else if (i == 1)
        {
            d_mf = Convert.ToDouble(wal.text);
            
            wal.text = "";
            WillQuitProgram = d_mf < 0;
            tex.text +=("\n Time, sec           : ");
            i++;
        }
        else if (i == 2)
        {
            if ((h_i == 0) | ((a < a_lim) & (mf_i != 0)))
            {
                Input.SetActive(false);
                t = Convert.ToDouble(wal.text);
                wal.text = "";

                if (FlightMode != 2)
                {
                    ac = 0;
                }
                WillReverseOfControl = t < 0;
                t = Math.Abs(t);
                CalcAcceleration(WillReverseOfControl);
                tex.text +=("\n Acceleration: " + a + " m/sec2");
            }
            else
            {
                d_mf = 0;
                if (a >= a_lim)
                {
                    tex.text =("\n !!! Acceleration exceeded");
                    t = a - a_lim;
                    tex.text =("\n Waiting " + t + " sec");
                }
                if (mf_i <= 0)
                {
                    tex.text =("\n !!! Fuel exceeded");
                    t = nf;
                }
                CalcAcceleration(false);
            }
            DoStep();
            i = 0;
            But();
        }
    }
    public void OutInfo()
    {
        tex.text =(" -------------------------------------------");
        tex.text +=("\n Altitude          : " + String.Format("{0,1:0.0000}", h_i) + " m");
        tex.text +=("\n Velocity          : " + String.Format("{0,1:0.0000}", u_i) + " m/sec");
        tex.text +=("\n Fuel              : " + mf_i + " kg");
        tex.text +=("\n Life support      : " + ls + " sec");
        tex.text +=("\n -------------------------------------------");
    }
    public void CalcAcceleration(bool WillReverseOfControl)
    {
        q = d_mf / t;
        a = q * nf / (ms + mf_i);
        if (WillReverseOfControl)
        {
            a = -a;
            WillReverseOfControl = false;
        }
    }
    public void DoStep()
    {
        do
        {
            v_i1 = v_i;
            u_i1 = u_i;
            v_i = v_i + a * t * Math.Sin(ac);
            x_i = x_i + (v_i1 + v_i) / 2 * t;
            u_i = u_i + (a * Math.Cos(ac) - g) * t;
            h_i = h_i + (u_i1 + u_i) / 2 * t;
            mf_i = mf_i - q * t;

            ls = ls - t;

            if (mf_i < 0)
            {
                t = mf_i / q;
            }
        }
        while (mf_i < 0);

    }

    public void Win()
    {
        if (h_i == 0)
        {
            if (-u_i < 2.5)
            {
                tex.text =(" *** Отлично!  Мягкая посадка. ***");
            }
            else if (-u_i < 5.0)
            {
                tex.text =(" *** Хорошо!  Мягкая посадка. ***");
            }
            else if (-u_i < 7.5)
            {
                tex.text =(" *** Посадка. ***");
            }
            else if (-u_i < 10)
            {
                tex.text =(" *** Кораблю требуется ремонт. ***");
            }
            else if (-u_i < 12.5)
            {
                tex.text =(" *** У корабля вышел из строя двигатель. ***");
            }
            else
            {
                tex.text =(" *** Смертельный исход. ***");
            }
        }
    }
}
