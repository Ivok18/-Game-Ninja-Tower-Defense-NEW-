using UnityEngine;

namespace TD.Entities.Towers.AttackPattern
{
    public class PatternDetacher : MonoBehaviour
    {
        [SerializeField] private Transform dashPatternObejct;
        public void DetachPaternsFromParent()
        {
            dashPatternObejct.DetachChildren(); 
        }
    }

}
