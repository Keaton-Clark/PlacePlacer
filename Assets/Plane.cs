using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Plane : MonoBehaviour
{
    public GameObject Object;
    private MeshRenderer Renderer;
    private int PreviousFrame;
    // Start is called before the first frame update
    void Start()
    {
        Renderer = Object.GetComponent<MeshRenderer>();
        Show(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(int frame)
    {
        if (frame > 0 && frame <= 1280 && frame != PreviousFrame)
        {
            Renderer.material.mainTexture = Resources.Load("scene" + frame.ToString("D5")) as Texture;
        }
        PreviousFrame = frame;
    }
}
