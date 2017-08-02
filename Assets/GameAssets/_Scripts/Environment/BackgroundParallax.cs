using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public float movementSpeed = 1;
    public float speedMultiplier;

    private Material _mat;

    private float _time;

    private void Awake()
    {
        _mat = this.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void Update()
    {
        _time += Time.deltaTime * speedMultiplier;

        _mat.SetTextureOffset("_MainTex", new Vector2(_time, 0) * movementSpeed);
    }
}
