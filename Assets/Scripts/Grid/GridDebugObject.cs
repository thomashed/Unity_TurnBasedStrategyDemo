using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CoolBeans.Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        private object gridObject;

        [SerializeField] private TextMeshPro tmp = null;


        private void Awake()
        {
            
        }

        protected virtual void Update()
        {
            tmp.text = gridObject.ToString();
        }

        public virtual void SetGridObject(object gridObject)
        {
            this.gridObject = gridObject;
        }

    }
}

