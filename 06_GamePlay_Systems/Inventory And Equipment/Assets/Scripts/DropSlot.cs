using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DragItem draggedItem = eventData.pointerDrag.GetComponent<DragItem>();
        if (draggedItem == null) return;

        Transform draggedSlot = draggedItem.originalParent;
        Transform targetSlot = transform;

        // Get the Icon images directly
        Image draggedIcon = draggedItem.GetComponent<Image>();
        Image targetIcon = targetSlot.Find("Icon").GetComponent<Image>();

        if (targetIcon.enabled)
        {
            // Swap sprites
            Sprite tempSprite = targetIcon.sprite;
            targetIcon.sprite = draggedIcon.sprite;
            draggedIcon.sprite = tempSprite;

            // Swap enabled states
            bool tempEnabled = targetIcon.enabled;
            targetIcon.enabled = draggedIcon.enabled;
            draggedIcon.enabled = tempEnabled;
        }
        else
        {
            // Just move item here
            targetIcon.sprite = draggedIcon.sprite;
            targetIcon.enabled = true;

            draggedIcon.sprite = null;
            draggedIcon.enabled = false;
        }

        // Always snap dragged item back into its slot
        draggedItem.transform.SetParent(draggedSlot, false);
        draggedItem.transform.localPosition = Vector3.zero;
    }
}
