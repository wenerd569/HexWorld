using Unity.VisualScripting;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    private Camera _mainCamera;
    public IWeapon Weapon; 

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && Weapon != null)
        {
            Weapon.Attack(this, _mainCamera.transform.forward); 
        }
    }
}
