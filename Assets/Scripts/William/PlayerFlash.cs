using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class PlayerFlash : MonoBehaviour
    {
        public void playerFlash(SpriteRenderer sprite, float duration, Color flashColor)
        {
            StartCoroutine(DoPlayerFlash(sprite, duration, flashColor));
        }

        private IEnumerator DoPlayerFlash(SpriteRenderer sprite, float duration, Color flashColor)
        {
            Color originColor = sprite.color;
            sprite.color = flashColor;

            yield return new WaitForSeconds(duration);

            sprite.color = originColor;

        }
    }
}
