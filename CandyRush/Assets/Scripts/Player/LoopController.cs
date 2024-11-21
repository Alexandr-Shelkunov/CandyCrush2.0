using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class LoopController : MonoBehaviour
    {
        private List<IUpdatable> updatables = new List<IUpdatable>();

        // Регистрация объектов, которые должны обновляться
        public void Register(IUpdatable updatable)
        {
            if (!updatables.Contains(updatable))
            {
                updatables.Add(updatable);
            }
        }

        // Вызов обновления для зарегистрированных объектов
        private void Update()
        {
            foreach (var updatable in updatables)
            {
                updatable.DoUpdate();
            }
        }
    }
}