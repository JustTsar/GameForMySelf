using UnityEngine;

namespace _Game.Scripts.Utility.Extension
{
    public static class MaterialExtension
    {
        public const float PulsateDirectionForward = 1.0f;
        public const float PulsateDirectionBack = -1.0f;
        private static readonly int emissionStrengthId = Shader.PropertyToID("_EmissionStrength");
        
        /// EG/Emissive
        public static float GetEmissionStrength(this Material material)
        {
            return material.GetFloat(emissionStrengthId);
        }

        /// EG/Emissive
        public static void SetEmissionStrength(this Material material, float value)
        {
            material.SetFloat(emissionStrengthId, value);
        }

        public static void Move(this Material material, Vector2 min, Vector2 max, Vector2 delta)
        {
            var offset = material.mainTextureOffset + delta;
            if (offset.x < min.x)
            {
                offset.x = max.x;
            }
            else if (offset.x > max.x)
            {
                offset.x = min.x;
            }
            if (offset.y < min.y)
            {
                offset.y = max.y;
            }
            else if (offset.y > max.y)
            {
                offset.y = min.y;
            }
            material.mainTextureOffset = offset;
        }

        public static void Move(this Material material, string[] textures, Vector2 min, Vector2 max, Vector2 delta)
        {
            var offset = material.GetTextureOffset(textures[0]) + delta;
            if (offset.x < min.x)
            {
                offset.x = max.x;
            }
            else if (offset.x > max.x)
            {
                offset.x = min.x;
            }
            if (offset.y < min.y)
            {
                offset.y = max.y;
            }
            else if (offset.y > max.y)
            {
                offset.y = min.y;
            }

            foreach (var textureName in textures)
            {
                material.SetTextureOffset(textureName, offset);
            }
        }

        public static void Pulsate(this Material[] materials, float min, float max, float delta, ref float direction)
        {
            foreach (var material in materials)
            {
                Pulsate(material, min, max, delta, ref direction);
            }
        }
        
        public static void Pulsate(this Material material, float min, float max, float delta, ref float direction)
        {
            var strength = material.GetEmissionStrength();
            strength += delta * direction;
            if (strength < min)
            {
                strength = min;
                direction = PulsateDirectionForward;
            }
            else if (strength > max)
            {
                strength = max;
                direction = PulsateDirectionBack;
            }
            material.SetEmissionStrength(strength);
        }
    }
}