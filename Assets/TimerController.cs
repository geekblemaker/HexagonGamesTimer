using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    [SerializeField] private GameObject parentObj;
    [SerializeField] private GameObject pausePanelObj;
    [SerializeField] private GameObject[] panelPrefabs;
    [SerializeField] private Vector3[] orderVecs;
    [SerializeField] private float totalTime;
    
    private GameObject[] panelObjs = new GameObject[3];
    private TextMeshProUGUI[] timerTexts = new TextMeshProUGUI[3];
    private float[] currentTime = new float[3];
    
    private int orderNum = 0;
    private bool isStart = false;
    private bool isPause = false;

    void Start()
    {
        for (int i = 0; i < panelPrefabs.Length; i++)
        {
            GameObject tempObj = Instantiate(panelPrefabs[ButtonController.orderNum[i]], parentObj.transform);
            panelObjs[i] = tempObj;

            timerTexts[i] = panelObjs[i].transform.GetChild(0).
                GetComponent<TextMeshProUGUI>();
            
            currentTime[i] = totalTime;
            SetTimer(timerTexts[i], currentTime[i]);
        }
    }

    void Update()
    {
        ReturnMenu();
        SwitchTimer();
        PauseTime();
        ChangeTime();
    }

    private void ReturnMenu()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
    }

    private void SwitchTimer()
    {
        if(isPause) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isStart)
                isStart = true;
            else
            {
                orderNum++;
                if (orderNum > 2) orderNum = 0;
                Debug.Log(orderNum);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!isStart)
                isStart = true;
            else
            {
                orderNum--;
                if (orderNum < 0) orderNum = 2;
                // Debug.Log(orderNum);
            }
        }
    }

    private void PauseTime()
    {
        if (!isStart) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPause = !isPause;
            pausePanelObj.SetActive(isPause);
        }

    }

    private void ChangeTime()
    {
        if (!isStart || isPause) return;
        
        currentTime[orderNum] -= Time.deltaTime;
        if (currentTime[orderNum] < 0)
        {
            currentTime[orderNum] = 0;
            SetTimer(timerTexts[orderNum], currentTime[orderNum]);
            
            orderNum++;
            if (orderNum > 2) orderNum = 0;
            // Debug.Log(orderNum);
        }
        else
            SetTimer(timerTexts[orderNum], currentTime[orderNum]);
    }

    private void SetTimer(TextMeshProUGUI _timerText, float _time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(_time);
        int milisSeconds = timespan.Milliseconds / 10;
        string timeString = string.Format("{0:00}:{1:00}:{2:00}",
            timespan.Minutes, timespan.Seconds, milisSeconds);

        _timerText.text = timeString;
    }
}
