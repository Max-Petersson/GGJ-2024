using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRendererOnColliders : MonoBehaviour
{
    // Start is called before the first frame update

    private Collider2D[] colliders;
    [SerializeField] private bool showColliders = false;
    void Awake()
    {
        colliders = GetComponentsInChildren<Collider2D>();
        ShowColliders(showColliders);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowColliders(!showColliders);
        }
    }

    [ContextMenu("Show Colliders")]
    public void ShowColliders()
    {
        ShowColliders(true);
    }

    [ContextMenu("Hide Colliders")]
    public void HideCollider()
    {
        ShowColliders(false);
    }

    public void ShowColliders(bool isVisible)
    {
        foreach (Collider2D collider in colliders)
        {
            if (!collider.TryGetComponent(out Renderer renderer))
                return;

            renderer.enabled = isVisible;

        }
        showColliders = isVisible;
    }
}
