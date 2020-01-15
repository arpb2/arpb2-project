using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPB2
{

    public class TeleportPadBehaviour : PadBehaviour
    {
        private TeleportPadBehaviour PairPad;
        private DetectedPlatform platform;
        public DetectedPlatform Platform { get => platform; set => platform = value; }

        public override void Activate(GameObject obj)
        {
            obj.transform.position = PairPad.transform.position;
            obj.GetComponent<MainCharacterBehaviour>().ChangePlatform(PairPad.Platform);
        }

        public void SetPair(TeleportPadBehaviour other)
        {
            PairPad = other;
        }

    }

}