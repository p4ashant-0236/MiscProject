using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : Button
{
    public bool CanAnimate = true;

    [HideInInspector]
    public bool isPressed;

    private RectTransform rectTransform;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (CanAnimate && IsInteractable())
        {
            if (rectTransform == null)
            {
                rectTransform = transform.GetComponent<RectTransform>();

                //If Pivot is not center, then make it center
                if (rectTransform.pivot.x != 0.5f || rectTransform.pivot.y != 0.5f)
                {
                    SetPivot(rectTransform, new Vector2(0.5f, 0.5f));
                }
            }
            LeanTween.cancel(this.gameObject);
            LeanTween.scale(this.gameObject, new Vector3(0.92f, 0.92f, 1f), 0.07f);
        }
        base.OnPointerDown(eventData);
        isPressed = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (CanAnimate)
        {
            LeanTween.cancel(this.gameObject);
            LeanTween.scale(this.gameObject, Vector3.one, 0.15f).setEaseOutBack();
        }
        base.OnPointerUp(eventData);
        isPressed = false;
    }

    private void SetPivot(RectTransform rectTransform, Vector2 pivot)
    {
        Vector3 deltaPosition = rectTransform.pivot - pivot;    // get change in pivot
        deltaPosition.Scale(rectTransform.rect.size);           // apply sizing
        deltaPosition.Scale(rectTransform.localScale);          // apply scaling
        deltaPosition = rectTransform.rotation * deltaPosition; // apply rotation

        rectTransform.pivot = pivot;                            // change the pivot
        rectTransform.localPosition -= deltaPosition;           // reverse the position change
    }
}