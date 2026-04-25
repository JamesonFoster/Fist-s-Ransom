using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerButtonSupport : MonoBehaviour
{
    public int activeIfIDEqual = 0;
    public MainMenuButtons buttonActivator;
    private UpgradeButton ub;
    public ButtonCodeControllerSupport bccs;
    private ShopButtons sB;
    private MapButtons mB;

    public int buttonCode;
    public int leftButtonCode = 999;
    public int rightButtonCode = 999;
    public int upButtonCode = 999;
    public int downButtonCode = 999;

    public int buttonAction = 0;

    public Vector3 baseScale;
    private int lastH = 0; 
    private int lastV = 0;
    private bool wasSelected = false;

    void Awake()
    {
        baseScale = transform.localScale;
        ub = GetComponent<UpgradeButton>();
        mB = GetComponent<MapButtons>();
        sB = GetComponent<ShopButtons>();
    }

    void Update()
    {
        if (Gamepad.current != null)
    {
        if (bccs == null) return;

        bool isSelected = (bccs.currentButtonCode == buttonCode);

        // Detect when selection moves onto this button
        if (isSelected && !wasSelected)
        {
            if (sB != null)
                sB.PlaySounds();
            else if (ub != null)
                ub.PlaySounds();
        }

        wasSelected = isSelected;

        HandleScaling();
        HandleInput();
    }
    }

    void HandleScaling()
    {
        if (Gamepad.current != null && bccs.currentButtonCode == buttonCode)
            transform.localScale = baseScale * 1.5f;
        else
            transform.localScale = baseScale;
    }

    void HandleInput()
    {
        if (activeIfIDEqual == 0 || activeIfIDEqual == GlobalPlayerVars.playerLocationID)
        {
        // PRESS (unchanged)
        if (Input.GetButtonDown("LeftAttack") && bccs.currentButtonCode == buttonCode)
        {
            if (buttonAction == 0)
                buttonActivator.StartRun();
            else if (buttonAction == 1)
                buttonActivator.QuitGame();
            else if (buttonAction == 2)
            {
                if (ub != null) ub.OnClick();
                if (mB != null) mB.OnMouseDown();
                if (sB != null) sB.OnClick();
            }
        }
    
        // Only active button can move
        if (bccs.currentButtonCode != buttonCode) return;
    
        // Global cooldown check
        if (!bccs.CanMove()) return;
    
        float rawH = Input.GetAxisRaw("Horizontal");
        float rawV = Input.GetAxisRaw("Vertical");
    
        int h = Mathf.Abs(rawH) > 0.5f ? (int)Mathf.Sign(rawH) : 0;
        int v = Mathf.Abs(rawV) > 0.5f ? (int)Mathf.Sign(rawV) : 0;
    
        // Detect fresh press
        if (h == 1 && lastH != 1 && rightButtonCode != 999)
        {
            bccs.currentButtonCode = rightButtonCode;
            wasSelected = false;
            bccs.RegisterMove();
        }
        else if (h == -1 && lastH != -1 && leftButtonCode != 999)
        {
            bccs.currentButtonCode = leftButtonCode;
            wasSelected = false;
            bccs.RegisterMove();
        }
        else if (v == -1 && lastV != -1 && downButtonCode != 999)
        {
            bccs.currentButtonCode = downButtonCode;
            wasSelected = false;
            bccs.RegisterMove();
        }
        else if (v == 1 && lastV != 1 && upButtonCode != 999)
        {
            bccs.currentButtonCode = upButtonCode;
            wasSelected = false;
            bccs.RegisterMove();
        }
    
        lastH = h;
        lastV = v;
        }
    }
}