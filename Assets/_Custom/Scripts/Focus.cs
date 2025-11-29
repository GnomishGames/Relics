using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Focus : MonoBehaviour
{
    public Camera cam;
    public GameObject playerTarget;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // don't click through the UI (guard EventSystem.current which can be null in some scenes)
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //left mouse click sets focus
        if (Input.GetMouseButtonDown(0))
        {
            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null)
                {
                    Debug.LogWarning("Focus: no Camera assigned and Camera.main is null. Cannot raycast.");
                    return;
                }
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                // check if we hit a focusable object
                Item item = hit.collider != null ? hit.collider.GetComponent<Item>() : null;
                    if (item != null)
                    {
                        // assign the clicked item's GameObject to playerTarget
                        playerTarget = item.gameObject;
                        Debug.Log($"Focus: playerTarget set to '{playerTarget.name}'.");
                    }
            }
        }
    }
}
