using TD.PlayerLife;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TD.SceneLoader
{
    public class SceneLoader : MonoBehaviour
    {
        private void OnEnable()
        {
            LifeManager.OnPlayerLoseLife += ReloadScene;
        }
        private void OnDisable()
        {
            LifeManager.OnPlayerLoseLife -= ReloadScene;
        }

        public void ReloadScene(int currentLife)
        {
            if (currentLife <= 0) SceneManager.LoadScene(0);
        }
    }
}
