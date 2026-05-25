using UnityEngine;

public class TestControl : MonoBehaviour
{
    [Header("Contents")]
    [SerializeField] private bool _isOn;
    [SerializeField] private GameObject _source;    
    [SerializeField] private GameObject _target;

    private void Update()
    {
        if (_isOn)
        {
            _isOn = false;
            Raycast();
        }
    }

    private void Raycast()
    {
        var direction = _target.transform.position - _source.transform.position;
        var hit = Physics2D.Raycast(_source.transform.position, direction);
        var a = 1;
    }
}
