using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public interface IUpdatable
    {
        void DoUpdate();
    }

    public class LoopController : MonoBehaviour
    {
        private List<IUpdatable> updatables = new List<IUpdatable>();

        // ����������� ��������, ������� ������ �����������
        public void Register(IUpdatable updatable)
        {
            if (!updatables.Contains(updatable))
            {
                updatables.Add(updatable);
            }
        }

        // ����� ���������� ��� ������������������ ��������
        private void Update()
        {
            foreach (var updatable in updatables)
            {
                updatable.DoUpdate();
            }
        }
    }
}