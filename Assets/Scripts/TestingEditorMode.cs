using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestingEditorMode : MonoBehaviour
{
    private void Awake()
    {
        print("EditorListenForKeyboardInput is awake");
    }

    private void Start()
    {
        print("EditorListenForKeyboardInput has started");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("EditorListenForKeyboardInput is updating");
        }
    }
}
