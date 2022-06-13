using Microsoft.Xna.Framework.Graphics;
using Roguecraft.Engine.Cameras;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render
{
    public class RenderService
    {
        private readonly CameraService _cameraService;
        private readonly Configuration _configuration;
        private readonly FrameCounter _frameCounter;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly HudRenderer _hudRenderer;
        private readonly ParticleRenderer _particleRenderer;

        private readonly SpriteBatch _spriteBatch;
        private readonly TextureRenderer _textureRenderer;

        private readonly TimeManager _timeManager;
        private readonly VisibilityRenderer _visibilityRenderer;

        public RenderService(TimeManager timeManager,
                             CameraService cameraService,
                             GraphicsDevice graphicsDevice,
                             Configuration configuration,
                             TextureRenderer textureRenderer,
                             ParticleRenderer particleRenderer,
                             VisibilityRenderer visibilityRenderer,
                             HudRenderer hudRenderer,
                             FrameCounter frameCounter)
        {
            _timeManager = timeManager;
            _cameraService = cameraService;
            _graphicsDevice = graphicsDevice;
            _configuration = configuration;
            _textureRenderer = textureRenderer;
            _particleRenderer = particleRenderer;
            _visibilityRenderer = visibilityRenderer;
            _hudRenderer = hudRenderer;
            _frameCounter = frameCounter;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public void Draw(float deltaTime)
        {
            deltaTime = _timeManager.DeltaTime;
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
    }
}