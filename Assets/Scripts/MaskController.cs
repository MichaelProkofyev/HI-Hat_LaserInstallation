using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MaskController : SingletonComponent<MaskController> {

    public bool useMask;

    public RectTransform[] masks;
    Vector3[] corners = new Vector3[4];

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        //mask.
        Vector3[] corners = new Vector3[4];
    }

    public bool Masked(Vector2 point) {
        if (!useMask) return false;

        for (int i = 0; i < masks.Length; i++){
            masks[i].GetWorldCorners(corners);
            bool masked = corners[0].x < point.x
                          && point.x < corners[2].x
                          && corners[0].y < point.y
                          && point.y < corners[2].y;
            if (masked) return true;
        }
        return false;
    }






}
