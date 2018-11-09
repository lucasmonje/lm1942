using UnityEngine;

namespace Core.Domain.Models
{
    public interface Instantiable
    {
        GameObject GetPrefab();
    }
}