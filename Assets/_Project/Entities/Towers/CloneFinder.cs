using UnityEngine;

namespace TD.Entities.Towers
{
    public class CloneFinder : MonoBehaviour
    {
        public bool FindClone(Transform transform)
        {
            ListOfTargets listOfTargets = GetComponent<ListOfTargets>();
            foreach (Transform obj in listOfTargets.EnemiesToAttack)
            {
                if (obj == transform)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
