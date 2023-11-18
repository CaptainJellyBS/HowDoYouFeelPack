using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CancelHandler : MonoBehaviour
{
    void OnCancel()
    {
        FindObjectOfType<CancelButton>()?.OnCancel();
    }
}
