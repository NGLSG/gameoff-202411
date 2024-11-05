using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening; // Import DOTween namespace

public class CloseOnClickOutside : MonoBehaviour
{
    [SerializeField] private float shrinkDuration = 0.5f;
    [SerializeField] private Vector3 targetScale = Vector3.zero;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                ShrinkAndClosePhone();
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    private void ShrinkAndClosePhone()
    {
        transform.DOScale(targetScale, shrinkDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}