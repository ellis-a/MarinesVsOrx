using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool IsDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int MaxHealth = 100;

    [SyncVar]
    private int _currentHealth;

    [SerializeField]
    private Behaviour[] _disableOnDeath;
    private bool[] _wasEnabled;

    private bool _isFirstSetup = true;

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            SetSceneCamera();
        }

        CmdBroadcastNewPlayerSetup();
    }

    private void SetSceneCamera()
    {
        //GameManager.Instance.SetSceneCameraActive(false);
        //GetComponent<PlayerSetup>().PlayerUiInstance.SetActive(true);
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (_isFirstSetup)
        {
            _isFirstSetup = false;
            _wasEnabled = new bool[_disableOnDeath.Length];
            for (int i = 0; i < _wasEnabled.Length; i++)
            {
                _wasEnabled[i] = _disableOnDeath[i].enabled;
            }
        }

        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage)
    {
        if (IsDead)
        {
            return;
        }

        _currentHealth -= damage;

        Debug.Log(transform.name + " has " + _currentHealth + " health.");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        //disable components
        for (int i = 0; i < _disableOnDeath.Length; i++)
        {
            _disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log(transform.name + " is dead.");

        //respawn
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.Instance.MatchSettings.RespawnTime);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

        Debug.Log(transform.name + " respawned.");
    }

    public void SetDefaults()
    {
        IsDead = false;
        _currentHealth = MaxHealth;

        for (int i = 0; i < _disableOnDeath.Length; i++)
        {
            _disableOnDeath[i].enabled = _wasEnabled[i];
        }
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }
}
