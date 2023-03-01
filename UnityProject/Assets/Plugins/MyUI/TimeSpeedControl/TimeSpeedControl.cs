using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeSpeedControl : MonoBehaviour
{
    public static TimeSpeedControl inst;

    public Button speedInc;
    public Button speedDec;
    public Toggle speedPause;
    public TimeIncEvent timeIncEvent;

    public int speed { get; set; }
    public bool isSysPause { get; set; }

    public bool isUserPause => speedPause.isOn;

    public bool isPause => isSysPause || isUserPause;

    public int MAX_SPEED => 4;
    public int MIN_SPEED => 1;

    private int currUpdateCount = 0;

    internal static GameObject CreateGameObject(GameObject parent)
    {
        GameObject instance = (GameObject)Instantiate(Resources.Load("TimeSpeedControl"), parent.transform);
        return instance;
    }


    void Start()
    {
        inst = this;

        isSysPause = false;

        speed = 1;

        UpdateSpeedControl();

        speedInc.onClick.AddListener(() =>
        {
            speed++;
            UpdateSpeedControl();
        });

        speedDec.onClick.AddListener(() =>
        {
            speed--;
            UpdateSpeedControl();
        });

        speedPause.onValueChanged.AddListener((value) =>
        {
            Time.timeScale = value ? 0 : 1;

            UpdateSpeedControl();
        });
    }

    void Update()
    {
        if(isPause)
        {
            return;
        }

        currUpdateCount++;

        if(currUpdateCount == speed*100)
        {
            timeIncEvent?.Invoke();

            currUpdateCount = 0;
        }
    }

    private void UpdateSpeedControl()
    {
        speedInc.interactable = !speedPause.isOn && speed < MAX_SPEED;
        speedDec.interactable = !speedPause.isOn && speed > MIN_SPEED;
    }
}

[System.Serializable]
public class TimeIncEvent : UnityEvent
{

}
