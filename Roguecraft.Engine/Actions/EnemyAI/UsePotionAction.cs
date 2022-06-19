using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.EnemyAI;

public class UsePotionAction : GameAction
{
    public UsePotionAction(Creature creature) : base(creature)
    {
    }

    public override bool TryPrepare(bool useMouse)
    {
        if (Creature.Inventory.Items.FirstOrDefault(x => x is Potion) is not Potion potion ||
            Creature.Health + potion.HealAction.RestoredHealth > Creature.Stats.MaxHealth)
        {
            return false;
        }
        potion.HealAction.Creature = Creature;

        return potion.HealAction.TryPrepare(useMouse);
    }

    protected override void OnPerform(float deltaTime)
    {
        var potion = Creature.Inventory.Items.FirstOrDefault(x => x is Potion) as Potion;
        if (potion is null)
        {
            return;
        }
        potion?.HealAction.Perform(deltaTime);
        Creature.Inventory.Remove(potion);
    }
}