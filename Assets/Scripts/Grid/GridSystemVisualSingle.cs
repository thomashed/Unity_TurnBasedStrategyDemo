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

        public void Show()
        {
            meshRenderer.enabled = true;       
        }

        public void Hide()
        {
            meshRenderer.enabled = false;
        }

    }
}
