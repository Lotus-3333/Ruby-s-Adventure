using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float displayTimeSpan = 4.0f;
    public GameObject dialogBox;

    private float _displayTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_displayTimer > 0)
        {
            _displayTimer -= Time.deltaTime;

            if (_displayTimer < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        dialogBox.SetActive(true);
        _displayTimer = displayTimeSpan;
    }
}
