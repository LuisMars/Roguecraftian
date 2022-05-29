using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions
{
    public class BasicAttack : Attack
    {
        public BasicAttack(Creature actor) : base(actor)
        {
        }

        protected override void OnPerform(float deltaTime)
        {
            Target.Health -= MaxDamage;
        }
    }
}