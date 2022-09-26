using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal enum InputType { keyboard,mobile}
    [SerializeField] private InputType input;
    [HideInInspector] public float vertical, horizontal,jump;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(input == InputType.mobile)
        {

        }
        else
        {
            keyboardInput();
        }
    }
    void keyboardInput()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
    }
}
