using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ball : MonoBehaviour
    {
        public Color Color { get => _spriteRenderer.color; set => _spriteRenderer.color = value; }
        protected SpriteRenderer _spriteRenderer;

        //TODO: Implement 2 types of balls which has red and blue colors.
        //OPTIONAL: Think outside the box.

        // I thought random color picking shouldn't be here. Because all options are should be able to choose from spawner.
        // This class should only assign given values if appropriate and do its own task.

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }
}
