using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject LeftButton;
    public GameObject RightButton;

    public TextMeshProUGUI completeText;
    public Image infoText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        bool status = !PlayerController.instance.walking;

        LeftButton.transform.GetComponent<Button>().interactable = status;
        RightButton.transform.GetComponent<Button>().interactable = status;
    }
}
