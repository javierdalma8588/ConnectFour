using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFields : MonoBehaviour
{
    public int column;

    private void OnMouseDown()
    {
        GameManager._instance.SelectColumn(column);
    }
}
