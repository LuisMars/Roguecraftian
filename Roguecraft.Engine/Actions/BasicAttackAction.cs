using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions
{
    public class BasicAttackAction : AttackAction
    {
        public BasicAttackAction(Creature actor) : base(actor)
        {
        }

        protected override void OnPerform(float deltaTime)
        {
            Target.Health -= MaxDamage;
        }
    }
}