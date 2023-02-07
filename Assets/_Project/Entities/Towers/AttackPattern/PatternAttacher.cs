using UnityEngine;

namespace TD.Entities.Towers.AttackPattern
{
    public class PatternAttacher : MonoBehaviour
    {
        public void AttachPaternsToParent()
        {
            AttackPatternsStorer dashPatternBehaviour = GetComponent<AttackPatternsStorer>();
            foreach (PatternBehaviour pattern in dashPatternBehaviour.Patterns)
            {
                pattern.AttachToParent();
            }
        }

    }
}

