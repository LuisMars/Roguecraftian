using Microsoft.Xna.Framework;
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

    public void AddBarrel(Vector2 position)
    {
        _moveableDecorationFactory.AddBarrel(position);
    }

    public void AddBed(Vector2 position, Vector2 vector2, TextureRotation rotation)
    {
        _decorationFactory.AddBed(position, vector2, rotation);
    }

    public void AddBookshelf(Vector2 position, TextureRotation rotation)
    {
        _decorationFactory.AddBookshelf(position, rotation);
    }

    public void AddChair(Vector2 position)
    {
        _moveableDecorationFactory.AddChair(position);
    }

    public void AddCouch(Vector2 position, Vector2 vector2, TextureRotation rotation)
    {
        _decorationFactory.AddCouch(position, vector2, rotation);
    }

    public void AddFloorDecoration(Vector2 position, Vector2 size)
    {
        _floorDecorationFactory.AddRitual(position, size);
    }

    public void AddPlant(Vector2 position, TextureRotation rotation)
    {
        _decorationFactory.AddPlant(position, Vector2.One, rotation);
    }

    public void AddTable(Vector2 position, Vector2 size)
    {
        _moveableDecorationFactory.AddTable(position, size);
    }

    internal void AddPew(Vector2 position, Vector2 size, TextureRotation rotation)
    {
        _decorationFactory.AddPew(position, size, rotation);
    }

    internal void AddPodium(Vector2 position, Vector2 size, TextureRotation rotation)
    {
        _decorationFactory.AddPodium(position, size, rotation);
    }

    internal void AddStatue(Vector2 position, TextureRotation rotation)
    {
        _decorationFactory.AddStatue(position, rotation);
    }

    internal void AddTorch(Vector2 position, TextureRotation rotation)
    {
        _floorDecorationFactory.AddTorch(position, rotation);
    }

    internal void AddWheelchair(Vector2 position, TextureRotation rotation)
    {
        _moveableDecorationFactory.AddWheelchair(position, rotation);
    }
}