using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CoolBeans.Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        private GridObject gridObject;

        [SerializeField] private TextMeshPro tmp = null;


        private void Awake()
        {
            
        }

        private void Update()
        {
            SetGridText();
        }

        public void SetGridObject(GridObject gridObject)
        {
            this.gridObject = gridObject;
        }

        private void SetGridText()
        {
            tmp.text = gridObject.ToString();
        }
    }
}

