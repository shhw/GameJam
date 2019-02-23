using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// Entity父类。
    /// </summary>
    class BaseEntity : MonoBehaviour
    {
        protected object mInitParam;

        protected virtual void OnEnable()
        {

        }

        protected virtual void Awake()
        {

        }

        public virtual void Init(object param)
        {
            mInitParam = param;
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {

        }
    }
}
