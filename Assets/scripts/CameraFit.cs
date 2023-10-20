using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    float newAspectRatio = 9f / 16f;
    public CanvasScaler[] canvas;

    private void Awake()
    {
        Camera.main.rect = new Rect(0, 0, 1, 1);
    }

    void Start()
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].GetComponent<RectTransform>().anchorMax = Vector2.one / 2f;
            canvas[i].GetComponent<RectTransform>().anchorMin = Vector2.one / 2f;
        }
        float variance = newAspectRatio / Camera.main.aspect;
        if (Camera.main.aspect > newAspectRatio)
            for (int i = 0; i < canvas.Length; i++)
                canvas[i].matchWidthOrHeight = 1;
        if (variance < 1.0f)
            Camera.main.rect = new Rect((1.0f - variance) / 2.0f, 0, variance, 1.0f);
        else
        {
            variance = 1.0f / variance;
            Camera.main.rect = new Rect(0, (1.0f - variance) / 2.0f, 1.0f, variance);
        }
    }

    public void CambiaModalità(RenderMode mode)
    {
        for (int i = 0; i < canvas.Length; i++)
            canvas[i].GetComponent<Canvas>().renderMode = mode;
    }
}