using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [RequireComponent(typeof(Main))]
    [DefaultExecutionOrder(ExecutionOrder.Injector)]
    public class Injector : MonoBehaviour
    {
        [SerializeField]
        private Object[] m_Instances;

        private void Awake()
        {
            Main main = GetComponent<Main>();

            for (int i = 0; i < m_Instances.Length; i++)
            {
                if (m_Instances[i] == null)
                {
                    continue;
                }

                if (m_Instances[i] is IInstance)
                {
                    main.RegisterInstance(m_Instances[i] as IInstance);
                }
                else
                {
                    Debug.LogError(string.Format("Object {0} is not instance!", m_Instances[i]), m_Instances[i]);
                }
            }

            Destroy(this);
        }
    }
}
