using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
    public List<Action<bool>> OnChange = new List<Action<bool>>();
    public bool isOn = false;
    [SerializeField] private GameObject Filled;
    [SerializeField] private TextMeshProUGUI Label;
    [SerializeField] private GameObject LabelBackground;
    [SerializeField] private Button Icon;

    private Coroutine handle;

    private void OnEnable()
    {
        handle = StartCoroutine(Handler());
    }

    private void Awake()
    {
        Icon.onClick.AddListener(() =>
        {
            isOn = !isOn;
            Refresh();
            foreach (var action in OnChange)
            {
                action(isOn);
            }
        });
    }

    private IEnumerator Handler()
    {
        while (gameObject)
        {
            float width = Label.preferredWidth;
            float height = Label.preferredHeight;
            LabelBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 10, height + 10);
            Filled.GetComponent<RectTransform>().sizeDelta = LabelBackground.GetComponent<RectTransform>().sizeDelta;
            Refresh();
            yield return new WaitForEndOfFrame();
        }
    }

    void Refresh()
    {
        Filled.SetActive(isOn);
        Label.gameObject.SetActive(!isOn);
        LabelBackground.SetActive(!isOn);
        Icon.transform.localPosition = isOn
            ? new Vector3(
                Filled.transform.localPosition.x + Filled.transform.GetComponent<RectTransform>().sizeDelta.x / 2 -
                Icon.transform.GetComponent<RectTransform>().sizeDelta.x / 2, 0, 0)
            : new Vector3(
                Filled.transform.localPosition.x - Filled.transform.GetComponent<RectTransform>().sizeDelta.x / 2, 0,
                0);
    }
}