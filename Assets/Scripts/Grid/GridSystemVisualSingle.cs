using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoolBeans.Grid
{
    public class GridSystemVisualSingle : MonoBehaviour
    {

        [SerializeField] private MeshRenderer meshRenderer = null;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Show(Material material)
        {
            meshRenderer.enabled = true;
            meshRenderer.material = material;
        }

        public void Hide()
        {
            meshRenderer.enabled = false;
        }

    }
}
