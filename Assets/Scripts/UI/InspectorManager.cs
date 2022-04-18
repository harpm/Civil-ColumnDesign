using TMPro;
using UnityEngine;

public class InspectorManager : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField]
    private TextMeshProUGUI _length;

    [SerializeField]
    private GameObject _forces;

    [SerializeField]
    private GameObject _inertia;

    [SerializeField]
    private TMP_InputField _aliveForceInp;

    [SerializeField]
    private TMP_InputField _deadForceInp;

    [SerializeField]
    private TMP_InputField _inertiaInp;

    [SerializeField]
    private TMP_Dropdown _hConnection;

    [SerializeField]
    private TMP_Dropdown _lConnection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
