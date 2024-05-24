using UnityEngine;
using UnityEngine.UIElements;

public class Minimap : MonoBehaviour
{
    public UIDocument minimapUIDoc;
    public RenderTexture minimapRenderTexture;

    void Start()
    {
        var rootVisualElement = minimapUIDoc.rootVisualElement;
        var minimapView = rootVisualElement.Q<Image>("minimapView");
        minimapView.image = minimapRenderTexture;
    }
}