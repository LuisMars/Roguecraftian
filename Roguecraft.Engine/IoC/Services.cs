using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Roguecraft.Engine.Cameras;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Render;
using Roguecraft.Engine.Simulation;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.IoC;

public class Services
{
    private readonly IKernel _kernel;

    public Services(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, ContentManager content)
    {
        _kernel = new StandardKernel();
        _kernel.Bind<GraphicsDevice>().ToConstant(graphicsDevice);
        _kernel.Bind<GraphicsDeviceManager>().ToConstant(graphics);
        _kernel.Bind<ContentManager>().ToConstant(content);
        _kernel.Bind<CollisionService>().ToSelf().InSingletonScope();
        _kernel.Bind<ActorPool>().ToSelf().InSingletonScope();
        _kernel.Bind<VisibilityService>().ToSelf().InSingletonScope();
        _kernel.Bind<CameraService>().ToSelf().InSingletonScope();
        _kernel.Bind<InputManager>().ToSelf().InSingletonScope();
        _kernel.Bind<RandomGenerator>().ToSelf().InSingletonScope();
        _kernel.Bind<TimeManager>().ToSelf().InSingletonScope();
        _kernel.Bind<FrameCounter>().ToSelf().InSingletonScope();
        _kernel.Bind<RenderService>().ToSelf().InSingletonScope();
        _kernel.Bind<ParticleRenderer>().ToSelf().InSingletonScope();
    }

    public Level Level => _kernel.Get<Level>();
}