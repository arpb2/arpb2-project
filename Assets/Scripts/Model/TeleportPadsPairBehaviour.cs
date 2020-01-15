using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPB2
{

    public class TeleportPadsPairBehaviour : MonoBehaviour
    {
        public GameObject TeleportPad;


        public void SetPlatforms(DetectedPlatform plat1, DetectedPlatform plat2)
        {
            _InstantiatePads(plat1, plat2);
        }


        private void _InstantiatePads(DetectedPlatform platform1, DetectedPlatform platform2)
        { 
            Vector3? pad1Position = DetectedPlatformHelper.FindTeleportPadPosition(platform1, platform2);
            Vector3? pad2Position = DetectedPlatformHelper.FindTeleportPadPosition(platform2, platform1);
            if (pad1Position != null && pad2Position != null)
            {
                var pad1 = Instantiate(TeleportPad, pad1Position.Value, Quaternion.identity).GetComponent<TeleportPadBehaviour>();
                var pad2 = Instantiate(TeleportPad, pad2Position.Value, Quaternion.identity).GetComponent<TeleportPadBehaviour>();
                pad1.SetPair(pad2);
                pad2.SetPair(pad1);
                pad1.Platform = platform1;
                pad2.Platform = platform2;
            }
        }
    }

}