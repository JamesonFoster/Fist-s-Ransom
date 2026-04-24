using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerButtonSupport : MonoBehaviour
{
    public MainMenuButtons buttonActivator;
    public ButtonCodeControllerSupport bccs;
    public int buttonCode;
    public int leftButtonCode = 999;
    public int rightButtonCode = 999;
    public int upButtonCode = 999;
    public int downButtonCode = 999;
    public int buttonAction = 0;
    public Vector3 baseScale;
    void Awake()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LeftAttack") && bccs.currentButtonCode == buttonCode)
        {
            if (buttonAction == 0)
                buttonActivator.StartRun();
            else if (buttonAction == 1)
                buttonActivator.QuitGame();
        }

        if (Input.GetAxis("Horizontal") > 0.9f && bccs.currentButtonCode == buttonCode && rightButtonCode != 999)
        {
            bccs.currentButtonCode = rightButtonCode;
        }
        if (Input.GetAxis("Horizontal") < -0.9f && bccs.currentButtonCode == buttonCode && rightButtonCode != 999)
        {
            bccs.currentButtonCode = leftButtonCode;
        }
        if (Input.GetAxis("Vertical") < -0.9f && bccs.currentButtonCode == buttonCode && rightButtonCode != 999)
        {
            bccs.currentButtonCode = downButtonCode;
        }
        if (Input.GetAxis("Vertical") > 0.9f && bccs.currentButtonCode == buttonCode && rightButtonCode != 999)
        {
            bccs.currentButtonCode = upButtonCode;
        }

        if (Gamepad.current != null)
        {
            if (bccs.currentButtonCode == buttonCode)
            {
                transform.localScale = new Vector3(3f, 3f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(2f, 2f, 1f);
            }
        }
        else
        {
            transform.localScale = baseScale;
        }
    }
}
