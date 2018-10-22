using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour {

    [SerializeField]
    GameObject arrow;

    public void MouseOverButton()
    {
        arrow.SetActive(true);
    }

    public void MouseExit()
    {
        arrow.SetActive(false);
    }
}
