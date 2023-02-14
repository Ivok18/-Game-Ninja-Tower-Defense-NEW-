using UnityEngine;

namespace TD.Entities.Towers.AttackPattern
{
    public class AttackPatternsStorer : MonoBehaviour
    {
        public PatternBehaviour[] Patterns;
        
        public PatternBehaviour GetPatternAt(int index)
        {
            return Patterns[index];
        }

        public void ResetPatternsState()
        {
            foreach(PatternBehaviour pattern in Patterns)
            {
                pattern.HasBeenReached = false;
            }
        }

        public bool HaveAllPatternsBeenReached()
        {
            foreach (PatternBehaviour pattern in Patterns)
            {
                if (!pattern.HasBeenReached)
                    return false;

            }

            return true;
        }

       

    }

}

