using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _timeRecordTMPro;
    private void Start()
    {
        _timeRecordTMPro.text = "Time: " + GameManager.gameTime.ToString("F2");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("BulletHell");
        }
    }
}
