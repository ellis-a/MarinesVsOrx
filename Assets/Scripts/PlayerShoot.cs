using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    private PlayerWeapon _currentWeapon;
    private WeaponManager _weaponManager;
    private float _lastFired = 0;

    [SerializeField]
    private Camera _cam;
    [SerializeField]
    private LayerMask _mask;

    void Start()
    {
        if (_cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
        _weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        _currentWeapon = _weaponManager.GetCurrentWeapon();

        if(_currentWeapon.FireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time - _lastFired > 1 / _currentWeapon.FireRate)
            {
                _lastFired = Time.time;
                Shoot();
            }
        }
    }

    //called on server when player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //called on all clients when doing a shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        _weaponManager.GetCurrentGraphics().MuzzleFlash.Play();
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        //TODO use object pooling
        GameObject hitEffect = Instantiate(_weaponManager.GetCurrentGraphics().HitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }

    [Client]
    private void Shoot()
    { 
        if (!isLocalPlayer)
        {
            return;
        }

        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _currentWeapon.Range, _mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, _currentWeapon.Damage);
            }
            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    private void CmdPlayerShot(string playerId, int damage)
    {
        Debug.Log(playerId + " has been shot!");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);

    }
}
