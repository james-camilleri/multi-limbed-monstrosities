using System;
using UnityEngine;
using UnityEngine.Events;

public class WebCamInput : MonoBehaviour
{
  // Unused
  string webCamName;
  [SerializeField] Vector2 webCamResolution = new Vector2(1920, 1080);
  [SerializeField] Texture staticInput;

  public UnityEvent<Texture> OnFrameReady;

  // Provide input image Texture.
  public Texture inputImageTexture => staticInput != null ? staticInput : inputRT;


  WebCamTexture webCamTexture;
  RenderTexture inputRT;

  void Start()
  {
    if (staticInput == null)
    {
      if (webCamName == string.Empty)
      {
        webCamTexture = new WebCamTexture((int)webCamResolution.x, (int)webCamResolution.y);
      }
      else
      {
        webCamTexture = new WebCamTexture(webCamName, (int)webCamResolution.x, (int)webCamResolution.y);
      }

      Debug.Log(webCamTexture.deviceName);

      webCamTexture.Play();
    }

    inputRT = new RenderTexture((int)webCamResolution.x, (int)webCamResolution.y, 0);
  }

  void Update()
  {
    if (staticInput != null)
      return;

    if (!webCamTexture.didUpdateThisFrame)
      return;

    var aspect1 = (float)webCamTexture.width / webCamTexture.height;
    var aspect2 = (float)inputRT.width / inputRT.height;
    var aspectGap = aspect2 / aspect1 * -1;

    var vMirrored = webCamTexture.videoVerticallyMirrored;
    var scale = new Vector2(aspectGap, vMirrored ? -1 : 1);
    var offset = new Vector2((1 - aspectGap) / 2, vMirrored ? 1 : 0);

    Graphics.Blit(webCamTexture, inputRT, scale, offset);

    OnFrameReady.Invoke(inputImageTexture);
  }

  void OnDestroy()
  {
    if (webCamTexture != null) Destroy(webCamTexture);
    if (inputRT != null) Destroy(inputRT);
  }
}
