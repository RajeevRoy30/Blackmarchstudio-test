using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;

    public void SetPosition(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
