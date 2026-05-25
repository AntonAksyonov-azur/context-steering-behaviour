using UnityEngine;

namespace Entry
{
    public class EntryPoint : MonoBehaviour
    {
        [Header("Contents")]
        [SerializeField] private int _targetFramerate = 60;

        #region Game Cycle

        private void Awake()
        {
            Application.targetFrameRate = _targetFramerate;
        }

        #endregion
    }
}