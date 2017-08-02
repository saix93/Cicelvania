using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    private Canvas _canvas;

    protected virtual void Awake()
    {
        _canvas = this.GetComponent<Canvas>();
    }

    protected Canvas GetCanvas()
    {
        return _canvas;
    } 
    
    public void SetEnabledCanvas(bool enabled)
    {
        _canvas.enabled = enabled;
    }
}
