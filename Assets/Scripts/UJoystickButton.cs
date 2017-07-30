using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UJoystickButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform canvas;
    [SerializeField]
    private RectTransform joystickBase;

    public Action OnBeginDragevent = delegate () { };
    public Action<Vector2> OnDragEvent = delegate (Vector2 vect) { };
    public Action OnEndDragEvent = delegate () { };

    private RectTransform rect;

    private float radius;
    private float radiusSqr;

    private Vector2 pointerOffset;

    private void Start ()
    {
        rect = GetComponent<RectTransform>();
        radius = joystickBase.sizeDelta.x / 2;
        radiusSqr = radius * radius;
        rect.localPosition = Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out pointerOffset);
        OnBeginDragevent();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pointerPosition = eventData.position;

        Vector3[] canvasCorners = new Vector3[4];
        canvas.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(pointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
        float clampedY = Mathf.Clamp(pointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

        pointerPosition = new Vector2(clampedX, clampedY);

        Vector2 localPointerOffset;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBase, pointerPosition, eventData.pressEventCamera, out localPointerOffset))
            return;

        Vector2 newLovalPosition = localPointerOffset - pointerOffset;

        if(newLovalPosition.sqrMagnitude > radiusSqr)
        {
            float angle = Vector2.Angle(Vector2.right, newLovalPosition);

            if (newLovalPosition.y < 0)
                angle = -angle;

            newLovalPosition = radius * new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        rect.localPosition = newLovalPosition;
        OnDragEvent(newLovalPosition/radius);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rect.localPosition = Vector2.zero;
        OnEndDragEvent();
    }
}
