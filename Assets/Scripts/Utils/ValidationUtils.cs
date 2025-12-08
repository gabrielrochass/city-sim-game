using UnityEngine;

namespace CitySim.Utils
{
    /// <summary>
    /// Classe com funções de validação reutilizáveis.
    /// </summary>
    public static class ValidationUtils
    {
        /// <summary>
        /// Valida se um valor está dentro de uma faixa.
        /// </summary>
        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Valida se uma posição de grid é válida.
        /// </summary>
        public static bool IsValidGridPosition(int x, int y, int maxWidth, int maxHeight)
        {
            return x >= 0 && x < maxWidth && y >= 0 && y < maxHeight;
        }

        /// <summary>
        /// Valida se uma string é válida.
        /// </summary>
        public static bool IsValidString(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Valida se um objeto é nulo.
        /// </summary>
        public static bool IsNotNull<T>(T obj) where T : class
        {
            return obj != null;
        }
    }
}
