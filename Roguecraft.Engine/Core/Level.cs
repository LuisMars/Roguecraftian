using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Roguecraft.Engine.Cameras;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Factories;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Render;
using Roguecraft.Engine.Simulation;
using Roguecraft.Engine.Sound;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.Core
{
    public class Level
    {
        private readonly ActorPool _actorPool;
        private readonly CameraService _cameraService;
        private readonly CollisionService _collisionService;
        private readonly Configuration _configuration;
        private readonly ContentManager _content;
        private readonly ContentRepository _contentRepository;
        private readonly DoorFactory _doorFactory;
        private readonly DungeonService _dungeonService;
        private readonly EnemyFactory _enemyFactory;
        private readonly FrameCounter _frameCounter;
        private readonly GameLoop _gameLoop;
        private readonly GraphicsDeviceManager _graphics;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly HeroFactory _heroFactory;
        private readonly HudRenderer _hudRenderer;
        private readonly ParticleRenderer _particleRenderer;
        private readonly ShapeRenderer _shapeRenderer;
        private readonly SoundService _soundService;
        private readonly SpriteBatch _spriteBatch;
        private readonly TextureRenderer _textureRenderer;
        private readonly VisibilityRenderer _visibilityRenderer;
        private readonly VisibilityService _visibilityService;
        private readonly WallFactory _wallFactory;

        public Level(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _graphics = graphics;
            _content = content;
            _contentRepository = new ContentRepository(_content);
            _configuration = new Configuration();
            _frameCounter = new FrameCounter();
            _actorPool = new ActorPool();

            _collisionService = new CollisionService(_actorPool);

            _heroFactory = new HeroFactory(_configuration, _actorPool, _collisionService, _contentRepository);
            _enemyFactory = new EnemyFactory(_configuration, _actorPool, _collisionService, _contentRepository);
            _wallFactory = new WallFactory(_configuration, _actorPool, _collisionService, _contentRepository);
            _doorFactory = new DoorFactory(_configuration, _actorPool, _collisionService, _contentRepository);

            _dungeonService = new DungeonService(_configuration, _collisionService, _heroFactory, _enemyFactory, _wallFactory, _doorFactory);

            _spriteBatch = new SpriteBatch(_graphicsDevice);

            _cameraService = new CameraService(_actorPool, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            _visibilityService = new VisibilityService(_collisionService);
            _visibilityRenderer = new VisibilityRenderer(_actorPool, _graphicsDevice, _cameraService, _contentRepository, _visibilityService, _configuration);
            _gameLoop = new GameLoop(_actorPool, _collisionService, _visibilityService);

            _textureRenderer = new TextureRenderer(_actorPool);
            _shapeRenderer = new ShapeRenderer(_actorPool, _visibilityService);
            _particleRenderer = new ParticleRenderer(_configuration, _contentRepository, _actorPool);
            _hudRenderer = new HudRenderer(_actorPool, _contentRepository, _configuration, _frameCounter);

            _soundService = new SoundService(_actorPool, _contentRepository);
            _dungeonService.Initialize();
        }

        public void Draw(float deltaTime)
        {
            _cameraService.Update(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, deltaTime);
            _graphicsDevice.Clear(_configuration.BackgroundColor.ToColor());

            _spriteBatch.Begin(transformMatrix: _cameraService.GetViewTransformationMatrix(),
                               sortMode: SpriteSortMode.FrontToBack,
                               samplerState: SamplerState.LinearWrap,
                               blendState: BlendState.AlphaBlend);

            _textureRenderer.Render(_spriteBatch);
            _particleRenderer.Render(_spriteBatch);
            //_shapeRenderer.Render(_spriteBatch);
            _spriteBatch.End();

            _visibilityRenderer.Render(_spriteBatch);

            _hudRenderer.Render(_spriteBatch);

            _frameCounter.Update();
        }

        public void Update(float deltaTime)
        {
            _gameLoop.Update(deltaTime);
            _soundService.Play();
            _particleRenderer.Update(deltaTime);
            _gameLoop.UpdateTimers(deltaTime);
        }
    }
}