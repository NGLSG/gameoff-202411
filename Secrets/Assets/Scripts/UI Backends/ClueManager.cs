using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueManager : Singleton<ClueManager>
{
    [SerializeField] private DialogueSystem.ClueData[] ClueDatas;
    [SerializeField] private GameObject ClueButtonPrefab;
    [SerializeField] private GameObject ClueButtonParent;
    [SerializeField] private GameObject CluePrefab;
    [SerializeField] private GameObject ClueParent;

    private void OnEnable()
    {
        ClueDatas = DialogueSystem.Instance.LoadAllCluesFromJson();
        Utils.RemoveAllChildren(ClueButtonParent.transform);
        Utils.RemoveAllChildren(ClueParent.transform);
        foreach (var cds in ClueDatas)
        {
            var go = Instantiate(ClueButtonPrefab, ClueButtonParent.transform);
            go.GetComponent<ClueBTN>().Text.text = cds.title;
            go.GetComponent<ClueBTN>().ClueIndex = Array.IndexOf(ClueDatas, cds);
        }
    }

    public void ChangeCurrentClue(int index)
    {
        Utils.RemoveAllChildren(ClueParent.transform);
        foreach (var cc in ClueDatas[index].content)
        {
            var go = Instantiate(CluePrefab, ClueParent.transform);
            go.GetComponentInChildren<ChatContent>().content.text = Utils.StringCombine(cc.Speaker, ": ", cc.Text);
        }
        var goes = Instantiate(CluePrefab, ClueParent.transform);
        goes.GetComponentInChildren<ChatContent>().content.text = Utils.StringCombine("");
    }
}