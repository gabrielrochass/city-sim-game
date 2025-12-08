using UnityEngine;

namespace CitySim.Utils
{
    /// <summary>
    /// Extensões de método para classes do Unity.
    /// </summary>
    public static class UnityExtensions
    {
        /// <summary>
        /// Destrói todos os filhos de um Transform.
        /// </summary>
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Obtém um componente, criando-o se não existir.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        /// <summary>
        /// Reseta a posição local de um Transform.
        /// </summary>
        public static void ResetLocalTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
