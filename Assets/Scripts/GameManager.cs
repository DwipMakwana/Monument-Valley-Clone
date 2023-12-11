using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player;
    public List<PathCondition> pathConditions = new List<PathCondition>();
    public List<Transform> pivots;
    public List<Transform> stairs;
    public Transform door;
    public List<Transform> cameras;

    public Transform[] objectsToHide;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        foreach(PathCondition pc in pathConditions)
        {
            int count = 0;
            for (int i = 0; i < pc.conditions.Count; i++)
            {
                if (pc.conditions[i].conditionObject.eulerAngles == pc.conditions[i].eulerAngle)
                {
                    count++;
                }
            }
            foreach(SinglePath sp in pc.paths)
                sp.block.possiblePaths[sp.index].active = (count == pc.conditions.Count);
        }

        if (player.walking)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int multiplier = Input.GetKey(KeyCode.RightArrow) ? 1 : -1;
            RotateMainPlatdform(multiplier);
        }

        foreach(Transform t in objectsToHide)
        {
            t.gameObject.SetActive(pivots[0].eulerAngles.y > 45 && pivots[0].eulerAngles.y < 90 + 45);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }

    public void RotateMainPlatdform(int multiplier)
    {
        SoundManager.instance.PlaySound(5);

        pivots[0].DOComplete();
        pivots[0].DORotate(new Vector3(0, 90 * multiplier, 0), .6f, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack);
    }

    public void RotateRightPivot()
    {
        SoundManager.instance.PlaySound(2);
     
        pivots[1].DOComplete();
        pivots[1].DORotate(new Vector3(0, 0, 90), 2.5f).SetEase(Ease.OutBack);
    }

    public void MoveStairs()
    {
        stairs[0].DOComplete();
        stairs[0].DOMove(new Vector3(-4, 5, -1), .6f).SetEase(Ease.OutBack);

        stairs[1].DOComplete();
        stairs[1].DOMove(new Vector3(-4, 5, -2), .6f).SetEase(Ease.OutBack);

        stairs[2].DOComplete();
        stairs[2].DOMove(new Vector3(-4, 6, -1), .6f).SetEase(Ease.OutBack);

        GameObject.Find("9").GetComponent<Walkable>().possiblePaths[1].active = true;
    }

    public void OpenDoor()
    {
        SoundManager.instance.PlaySound(3);

        door.DOComplete();
        door.DOMove(new Vector3(-3.4f, 13, 0), 3f).SetEase(Ease.OutBack);

        PlayerController.instance.isDoorOpened = true;
    }

    public void FinalTransition()
    {
        PlayerController.instance.gamecomplete = true;

        SoundManager.instance.PlaySound(1);

        UIManager.Instance.completeText.DOColor(new Color(1, 1, 1, 1), 1);        
        DOTween.Sequence().SetDelay(3).Append(UIManager.Instance.infoText.DOColor(new Color(1, 1, 1, 1), 1));

        UIManager.Instance.LeftButton.GetComponent<Button>().interactable = false;
        UIManager.Instance.LeftButton.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1);
        UIManager.Instance.LeftButton.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1);

        UIManager.Instance.RightButton.GetComponent<Button>().interactable = false;
        UIManager.Instance.RightButton.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1);
        UIManager.Instance.RightButton.transform.GetChild(0).GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1);

        cameras[0].DOMove(new Vector3(16, 35f, -20), 10f).SetEase(Ease.OutBack);
        cameras[1].DOMove(new Vector3(16, 35f, -20), 10f).SetEase(Ease.OutBack);

        DOVirtual.Float(13, 6, 10, v => cameras[0].GetComponent<Camera>().orthographicSize = v);
        DOVirtual.Float(13, 6, 10, v => cameras[1].GetComponent<Camera>().orthographicSize = v);
    }
}

[System.Serializable]
public class PathCondition
{
    public string pathConditionName;
    public List<Condition> conditions;
    public List<SinglePath> paths;
}
[System.Serializable]
public class Condition
{
    public Transform conditionObject;
    public Vector3 eulerAngle;

}
[System.Serializable]
public class SinglePath
{
    public Walkable block;
    public int index;
}
