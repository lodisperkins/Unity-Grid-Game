using Lodis;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Steffan
{
    public class PlayerHPTextBehaviour : MonoBehaviour
    {
        [SerializeField]
        private HealthBehaviour hp;

        [SerializeField]
        private Text hpText;

        // Update is called once per frame
        void Update()
        {
            hpText.text = "HP: " + hp.Health.Val;
        }
    }
}
