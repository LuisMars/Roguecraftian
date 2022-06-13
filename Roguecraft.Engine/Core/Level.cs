using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Render;

namespace Roguecraft.Engine.Core
{
    public class Level
    {
        private readonly DungeonService _dungeonService;
        private readonly RenderService _renderService;
        private readonly UpdateService _updateService;

        public Level(DungeonService dungeonService, RenderService renderService, UpdateService updateService)
        {
            _dungeonService = dungeonService;
            _renderService = renderService;
            _updateService = updateService;

            _dungeonService.Initialize();
        }

        public void Draw(float deltaTime)
        {
            _renderService.Draw(deltaTime);
        }

        public void Update(float deltaTime)
        {
            _updateService.Update(deltaTime);
        }
    }
}