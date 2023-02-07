using System.Collections.Generic;
using TD.Entities.Towers.AttackPattern;
using UnityEngine;

namespace TD.Entities.Enemies
{
    public class AttackPatternOverlapTracker : MonoBehaviour
    {
        public List<Transform> patternCollisions;
        
        //Check if the patttern behaviour in parameter is overlaping enemy, in this case this function will return the pattern
        public PatternBehaviour FindOverlapingPattern(PatternBehaviour pattern)
        {
            foreach(var overlapingPattern in patternCollisions)
            {
                PatternBehaviour patternBehaviour = overlapingPattern.GetComponent<PatternBehaviour>();
                if (patternBehaviour == pattern) return patternBehaviour;
            }

            return null;
        }

        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("AttackPattern")) return;
            patternCollisions.Add(collision.transform);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag("AttackPattern")) return;
            patternCollisions.Remove(collision.transform);
        }
    }
}
