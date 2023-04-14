using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private SpawnManager spawnManager;
        private static GameManager _instance = null;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        GameObject newManagerObject = new GameObject();
                        newManagerObject.name = "GameManager";
                        _instance = newManagerObject.AddComponent<GameManager>();
                        DontDestroyOnLoad(newManagerObject);
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private AudioClip _spawnSound;
        [SerializeField] private AudioClip _deleteSound;
        private AudioSource _audioSource;

        private void Awake()
        {
            ////TODO: What's a better way to implement this singleton?

            // If we need more classes that should contain singleton such as audio manager, object pooling etc.
            // We can do generic implementation of singleton without code duplication to solve this problem.
            // But I don't want to overboard it.

            if (_instance != this && _instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                // In case of multiple GameManagers in the scene, there may be errors in events after destroying.
                // So we do subscription on undeleted GameManager
                _audioSource = GetComponent<AudioSource>();
                spawnManager = FindObjectOfType<SpawnManager>();

                if (spawnManager != null)
                {
                    spawnManager.OnBallCreated += PlaySpawnSound;
                    spawnManager.OnBallDeleted += PlayDeletedSound;
                }
            }            
        }

        public void PlaySpawnSound()
        {
            _audioSource.PlayOneShot(_spawnSound);
        }

        public void PlayDeletedSound()
        {
            _audioSource.PlayOneShot(_deleteSound);
        }
    }
}