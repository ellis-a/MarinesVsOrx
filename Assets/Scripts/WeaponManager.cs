using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon _primaryWeapon;
    [SerializeField]
    private string _weaponLayerName = "Weapon";
    [SerializeField]
    private Transform _weaponHolder;

    private PlayerWeapon _currentWeapon;
    private WeaponGraphics _currentGraphics;

    void Start()
    {
        EquipWeapon(_primaryWeapon);
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return _currentGraphics;
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return _currentWeapon;
    } 

    void EquipWeapon (PlayerWeapon nextWeapon)
    {
        _currentWeapon = nextWeapon;

        GameObject weaponInstance = Instantiate(nextWeapon.Graphics, _weaponHolder.position, _weaponHolder.rotation);
        weaponInstance.transform.SetParent(_weaponHolder);
        _currentGraphics = weaponInstance.GetComponent<WeaponGraphics>();

        if (_currentGraphics == null)
        {
            Debug.LogError("No WeaponGraphics component on " + weaponInstance.name);
        }

        if (isLocalPlayer)
        {
            Helpers.SetLayerRecursively(weaponInstance, LayerMask.NameToLayer(_weaponLayerName));
        }
    }
}
