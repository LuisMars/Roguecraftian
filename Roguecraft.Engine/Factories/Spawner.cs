using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Factories;

public class Spawner
{
    private readonly DecorationFactory _decorationFactory;
    private readonly DoorFactory _doorFactory;
    private readonly EnemyFactory _enemyFactory;
    private readonly FloorDecorationFactory _floorDecorationFactory;
    private readonly HeroFactory _heroFactory;
    private readonly MoveableDecorationFactory _moveableDecorationFactory;
    private readonly PotionFactory _potionFactory;
    private readonly WallFactory _wallFactory;
    private readonly WeaponFactory _weaponFactory;

    public Spawner(HeroFactory heroFactory,
                   EnemyFactory enemyFactory,
                   WallFactory wallFactory,
                   DoorFactory doorFactory,
                   PotionFactory potionFactory,
                   WeaponFactory weaponFactory,
                   DecorationFactory decorationFactory,
                   MoveableDecorationFactory moveableDecorationFactory,
                   FloorDecorationFactory floorDecorationFactory)
    {
        _heroFactory = heroFactory;
        _enemyFactory = enemyFactory;
        _wallFactory = wallFactory;
        _doorFactory = doorFactory;
        _potionFactory = potionFactory;
        _weaponFactory = weaponFactory;
        _decorationFactory = decorationFactory;
        _moveableDecorationFactory = moveableDecorationFactory;
        _floorDecorationFactory = floorDecorationFactory;
    }

    public void Add<T>(Vector2 position) where T : Actor
    {
        var t = typeof(T);
        if (t == typeof(Hero))
        {
            _heroFactory.Add(position);
        }
        else if (t == typeof(Enemy))
        {
            _enemyFactory.Add(position);
        }
        else if (t == typeof(Wall))
        {
            _wallFactory.Add(position);
        }
        else if (t == typeof(Door))
        {
            _doorFactory.Add(position);
        }
        else if (t == typeof(Potion))
        {
            _potionFactory.Add(position);
        }
        else if (t == typeof(Weapon))
        {
            _weaponFactory.Add(position);
        }
    }

    public void AddBarrel(Vector2 position, string name)
    {
        _moveableDecorationFactory.AddBarrel(position, name);
    }

    public void AddBed(Vector2 position, Vector2 vector2, SpriteEffects spriteEffect, TextureRotation rotation)
    {
        _decorationFactory.AddBed(position, vector2, spriteEffect, rotation);
    }

    public void AddBookshelf(Vector2 position, Vector2 size, SpriteEffects spriteEffect, TextureRotation rotation)
    {
        _decorationFactory.AddBookshelf(position, size, spriteEffect, rotation);
    }

    public void AddChair(Vector2 position)
    {
        _moveableDecorationFactory.AddChair(position);
    }

    public void AddFloorDecoration(Vector2 position, Vector2 size, string name)
    {
        _floorDecorationFactory.Add(position, size, name);
    }

    public void AddTable(Vector2 position, Vector2 size, string name)
    {
        _moveableDecorationFactory.AddTable(position, size, name);
    }
}