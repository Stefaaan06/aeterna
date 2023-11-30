using UnityEngine;

namespace CameraShaker
{

    public static class CameraShakePresets
    {
        /// <summary>
        /// [Sustained] A continuous, rough shake.
        /// </summary>
        public static CameraShakeInstance Earthquake
        {
            get
            {
                CameraShakeInstance c = new CameraShakeInstance(1f, 3.5f, 10f, 10f);
                c.PositionInfluence = Vector3.one * 0.25f;
                c.RotationInfluence = new Vector3(1, 1, 4);
                return c;
            }
        }
    }
}