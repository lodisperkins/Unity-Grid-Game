using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lodis
{
    public interface IListener
    {
        void Invoke(Object Sender);
    }
}
