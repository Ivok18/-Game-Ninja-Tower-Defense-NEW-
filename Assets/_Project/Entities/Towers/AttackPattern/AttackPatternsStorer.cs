using UnityEngine;

namespace TD.Entities.Towers.AttackPattern
{
    public class AttackPatternsStorer : MonoBehaviour
    {
        public PatternBehaviour[] Patterns;
        
        public PatternBehaviour FindNextPattern()
        {
            foreach(PatternBehaviour pattern in Patterns)
            {
                if(!pattern.HasBeenReached)
                {
                    return pattern;
                }
            }

            return null;
        }

        public void ResetPatternsState()
        {
            foreach(PatternBehaviour pattern in Patterns)
            {
                pattern.HasBeenReached = false;
            }
        }

       

    }

}

