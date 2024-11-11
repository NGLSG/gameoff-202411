using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ClueBTN : MonoBehaviour
{
    public int ClueIndex;
    public TextMeshProUGUI Text;

    public void OnClick()
    {
        ClueManager.Instance.ChangeCurrentClue(ClueIndex);
    }
}