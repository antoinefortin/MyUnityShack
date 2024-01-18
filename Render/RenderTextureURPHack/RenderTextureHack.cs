using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Back in the legacy pipeline days,we could simply flag a render texture to "Not clear". 
    URP handling this differently, it automatically clears the RT at every frame. Make it a bit shit 
    for things like trails, or whatsoever effect you want to achieve 
    This script follow a raycast to a plane and put it at the correct UV of the texture. 
    But feel free to modify it to your needs 
    ANtoine.
 */

public class RenderTextureHack : MonoBehaviour
{
    public RenderTexture renderTexture;
    private Texture2D brushTexture; // Feel free to put whatever you want
    private Material drawMaterial;

    void Start()
    {
        // Create a 5x5 white brush texture
        brushTexture = new Texture2D(5, 5);
        Color[] brushColors = new Color[25];
        for (int i = 0; i < brushColors.Length; i++)
        {
            brushColors[i] = Color.white;
        }
        brushTexture.SetPixels(brushColors);
        brushTexture.Apply();

        drawMaterial = new Material(Shader.Find("Hidden/BlitCopy")); // This is a basic shader to copy textures
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if hit object is our plane
                if (hit.collider.CompareTag("DrawingPlane"))
                {

                    Vector2 uv = hit.textureCoord;
                    DrawToRenderTexture(uv);
                    Debug.Log(uv);
                }
            }
        }
    }

    void DrawToRenderTexture(Vector2 uv)
    {
        // Convert UV to renderTexture coordinates
        int x = (int)(uv.x * renderTexture.width) - (brushTexture.width / 2);
        int y = (int)(uv.y * renderTexture.height) - (brushTexture.height / 2);

        // Draw on renderTexture
        Graphics.SetRenderTarget(renderTexture);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);
        Graphics.DrawTexture(new Rect(x, y, brushTexture.width, brushTexture.height), brushTexture, drawMaterial);
        GL.PopMatrix();
        Graphics.SetRenderTarget(null);


    }
    private void OnApplicationQuit()
    {
        ClearRenderTexture();
    }

    private void ClearRenderTexture()
    {
        Graphics.SetRenderTarget(renderTexture);
        GL.Clear(true, true, Color.clear); // Clear with transparent color
        Graphics.SetRenderTarget(null);
    }
}