
        using UnityEngine;
        public class DeeployInitializer : MonoBehaviour
        {
            void Awake()
            {
                DeeployInitializer[] objs = FindObjectsOfType<DeeployInitializer>();
                if (objs.Length > 1)
                {
                    Destroy(this.gameObject);
                }
                DontDestroyOnLoad(this.gameObject);
                 
            }

        }
      