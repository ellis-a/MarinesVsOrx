using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] _disabledComponents;
    [SerializeField]
    private string _localLayerName = "LocalPlayer";
    [SerializeField]
    private string _remoteLayerName = "RemotePlayer";
    [SerializeField]
    private string _dontDrawLayerName = "DontDraw";
    [SerializeField]
    private GameObject _playerGraphics;
    [SerializeField]
    private GameObject _playerUiPrefab;
    private GameObject _playerUiInstance;


    private Camera _sceneCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            _sceneCamera = Camera.main;
            if (_sceneCamera != null)
            {
                _sceneCamera.gameObject.SetActive(false);
            }

            //disable player graphics for local player
            Helpers.SetLayerRecursively(_playerGraphics, LayerMask.NameToLayer(_dontDrawLayerName));

            //ui
            _playerUiInstance = Instantiate(_playerUiPrefab);
            _playerUiInstance.name = _playerUiPrefab.name;

            GetComponent<Player>().SetupPlayer();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }

    void DisableComponents()
    {
        for (int i = 0; i < _disabledComponents.Length; i++)
        {
            _disabledComponents[i].enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        Helpers.SetLayerRecursively(gameObject, LayerMask.NameToLayer(_remoteLayerName), LayerMask.NameToLayer(_localLayerName));
    }

    void OnDisable()
    {
        Destroy(_playerUiInstance);

        if (_sceneCamera != null)
        {
            _sceneCamera.gameObject.SetActive(true);
        }

        GameManager.DeregisterPlayer(transform.name);
    }

}
