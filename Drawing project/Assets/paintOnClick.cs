using UnityEngine;

public class paintOnClick : MonoBehaviour
{
    Camera cam;
    public float thickness;
    Renderer rend;
    Texture2D tex;

    public Texture2D paintTexture;

    public Color paintColor;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            return;

        if (!hit.transform.CompareTag("paintable"))
            return;

        rend = hit.transform.GetComponent<Renderer>();

        // Now draw a pixel where we hit the object

        if (rend.material.GetTexture("_MainTex"))
            tex = rend.material.GetTexture("_MainTex") as Texture2D;

        else
        {
            Material material = new(rend.material.shader);

            var fillColorArray = paintTexture.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = Color.white;
            }

            paintTexture.SetPixels(fillColorArray);
            paintTexture.Apply();
            material.CopyPropertiesFromMaterial(rend.material);
            material.SetTexture("_MainTex", paintTexture);
            rend.material = material;
            tex = rend.material.GetTexture("_MainTex") as Texture2D;
        }

        Vector2 pixelUV = hit.textureCoord;

        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        for (int i = 0; i < thickness; i++)
        {
            for (int j = 0; j < thickness; j++)
            {
                if (tex.GetPixel(Mathf.FloorToInt(pixelUV.x) + i, Mathf.FloorToInt(pixelUV.y) + j) != paintColor)
                    tex.SetPixel(Mathf.FloorToInt(pixelUV.x) + i, Mathf.FloorToInt(pixelUV.y) + j, paintColor);
            }
        }

        tex.Apply();
    }
}