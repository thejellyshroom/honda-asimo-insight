using UnityEngine;
using TMPro;

public class ChoiceAppear : MonoBehaviour
{
    [SerializeField] GameObject yesNo;

    [SerializeField] TextMeshProUGUI yesTextComponent;
    [SerializeField] TextMeshProUGUI noTextComponent;

    void Awake()
    {
        yesNo.SetActive(false);
    }

    public void ShowChoices(string yesText, string noText)
    {
        yesTextComponent.text = yesText;
        noTextComponent.text = noText;

        yesNo.SetActive(true);
    }

    public void HideChoices()
    {
        yesNo.SetActive(false);
    }
}
