using UnityEngine;
using System;
using UnityEngine.UI;

public class Retro : MonoBehaviour
{
    public Text tex;
    double h_i, x_i, v_i, u_i, mf_i, ms, a_lim, d_mf, t, ac, g, nf, ls, a, q, v_i1, u_i1;
    public int FlightMode = 1;
    public bool WillReverseOfControl = false;
    public bool WillQuitProgram = false;

    public void Star()
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

        do
        {

            if (h_i < 0)
            {
                t = 2 * h_i / (Math.Sqrt(Math.Pow(u_i, 2) + 2 * h_i * (g - a * Math.Cos(ac))) - u_i);
            }
            else if (h_i == 0)
            {
                if (-u_i < 2.5)
                {
                    Console.WriteLine(" *** Отлично!  Мягкая посадка. ***");
                }
                else if (-u_i < 5.0)
                {
                    Console.WriteLine(" *** Хорошо!  Мягкая посадка. ***");
                }
                else if (-u_i < 7.5)
                {
                    Console.WriteLine(" *** Посадка. ***");
                }
                else if (-u_i < 10)
                {
                    Console.WriteLine(" *** Кораблю требуется ремонт. ***");
                }
                else if (-u_i < 12.5)
                {
                    Console.WriteLine(" *** У корабля вышел из строя двигатель. ***");
                }
                else
                {
                    Console.WriteLine(" *** Смертельный исход. ***");
                }
                break;
            }
            else
            {
                if ((h_i == 0) | ((a < a_lim) & (mf_i != 0)))
                {
                    OutInfo();
                    EnterManeuver(WillReverseOfControl, WillQuitProgram);
                    if (WillQuitProgram)
                    {
                        break;
                    }
                    CalcAcceleration(WillReverseOfControl);
                    Console.WriteLine("Acceleration: " + a + " m/sec2");
                }
                else
                {
                    d_mf = 0;
                    if (a >= a_lim)
                    {
                        Console.WriteLine("!!! Acceleration exceeded");
                        t = a - a_lim;
                        Console.WriteLine("Waiting " + t + " sec");
                    }
                    if (mf_i <= 0)
                    {
                        Console.WriteLine("!!! Fuel exceeded");
                        t = nf;
                    }
                    CalcAcceleration(false);
                }
            }

            DoStep();
        }
        while (!WillQuitProgram);
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

    public void OutInfo()
    {
        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("Altitude             : " + h_i + " m");
        Console.WriteLine("Velocity (vertical)  : " + u_i + " m/sec");
        Console.WriteLine("Fuel                 : " + mf_i + " kg");
        Console.WriteLine("Life support         : " + ls + " sec");
        Console.WriteLine("-------------------------------------------");
    }
    public void EnterManeuver(bool WillReverseOfControl, bool WillQuitProgram)
    {
        do
        {
            Console.WriteLine("Enter in-flight maneuver");
            Console.Write("Fuel consumption, kg: ");
            d_mf = Convert.ToInt32(Console.ReadLine());
            WillQuitProgram = d_mf < 0;
            if (WillQuitProgram)
            {
                break;
            }
            Console.Write("Time, sec           : ");
            t = Convert.ToInt32(Console.ReadLine());
            if (FlightMode != 2)
            {
                ac = 0;
            }
            else
            {
                Console.Write("Climb angle, °      : ");
                ac = Convert.ToInt32(Console.ReadLine());
                ac = ac * 0.017;
                ac = ac * (3.1415926535 * 180);
            }
        }
        while (t == 0);

        WillReverseOfControl = t < 0;
        t = Math.Abs(t);
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
}
