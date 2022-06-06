using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.VectorDraw;
using Roguecraft.Engine.Cameras;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.Render;

public class VisibilityRenderer
{
    private readonly ActorPool _actorPool;

    private readonly CameraService _cameraService;
    private readonly Configuration _configuration;
    private readonly ContentRepository _contentRepository;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly BlendState _lightBlend;
    private readonly TextureRegion2D _line;
    private readonly PrimitiveBatch _primitiveBatch;
    private readonly PrimitiveDrawing _primitiveDrawing;
    private readonly VisibilityService _visibilityService;

    public VisibilityRenderer(ActorPool actorPool,
                              GraphicsDevice graphicsDevice,
                              CameraService cameraService,
                              ContentRepository contentRepository,
                              VisibilityService visibilityService,
                              Configuration configuration)
    {
        _actorPool = actorPool;
        _graphicsDevice = graphicsDevice;
        _cameraService = cameraService;
        _contentRepository = contentRepository;
        _visibilityService = visibilityService;
        _configuration = configuration;
        _primitiveBatch = new PrimitiveBatch(_graphicsDevice);
        _primitiveDrawing = new PrimitiveDrawing(_primitiveBatch);

        _lightBlend = new BlendState
        {
            ColorBlendFunction = BlendFunction.Subtract,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero
        };

        _line = _contentRepository.Line;
    }

    public void DrawSolidCircle(Vector2 center, float radius, Color color)
    {
        if (!_primitiveBatch.IsReady())
        {
            throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
        }

        double num = 0.0;
        Vector2 vertex = center + radius * new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
        num += Math.PI / 16.0;
        for (int i = 1; i < 31; i++)
        {
            Vector2 vertex2 = center + radius * new Vector2((float)Math.Cos(num), (float)Math.Sin(num));
            Vector2 vertex3 = center + radius * new Vector2((float)Math.Cos(num + Math.PI / 16.0), (float)Math.Sin(num + Math.PI / 16.0));
            _primitiveBatch.AddVertex(vertex, color, PrimitiveType.TriangleList);
            _primitiveBatch.AddVertex(vertex2, color, PrimitiveType.TriangleList);
            _primitiveBatch.AddVertex(vertex3, color, PrimitiveType.TriangleList);
            num += Math.PI / 16.0;
        }
    }

    public void DrawSolidRectangle(Vector2 location, float width, float height, Color color)
    {
        Vector2[] vertices = new Vector2[4]
        {
                new Vector2(0f, 0f),
                new Vector2(width, 0f),
                new Vector2(width, height),
                new Vector2(0f, height)
        };
        _primitiveDrawing.DrawSolidPolygon(location, vertices, color, false);
    }

    public void Render(SpriteBatch spriteBatch)
    {
        var projection = Matrix.CreateOrthographicOffCenter(0f, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0f, 0f, 1f);
        var view = _cameraService.GetViewTransformationMatrix();

        using var renderTarget = new RenderTarget2D(_graphicsDevice, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
        _graphicsDevice.SetRenderTarget(renderTarget);

        var color = _configuration.BackgroundColor.ToColor();
        color = Color.Lerp(color, Color.White, 0.25f);
        _graphicsDevice.Clear(color);
        //_graphicsDevice.Clear(Color.White);

        _primitiveBatch.Begin(ref projection, ref view);
        var count = _visibilityService.Count;
        var current = 0f;
        var points = new List<Vector2>();
        var sorted = _visibilityService.Sorted;
        foreach (var triangle in sorted)
        {
            if (triangle.VertexA.EqualsWithTolerence(triangle.VertexB))
            {
                continue;
            }
            _primitiveDrawing.DrawSolidPolygon(Vector2.Zero, new[] { triangle.VertexA, triangle.VertexB, triangle.VertexC }, Color.White, false);
            current++;
        }
        foreach (var entity in _actorPool.Actors.Where(a => a.Visibility.IsVisibleByHero))
        {
            var collidableComponent = entity.Collision;
            if (collidableComponent is null)
            {
                continue;
            }
            var bounds = collidableComponent.Bounds;
            if (bounds is CircleF circle)
            {
                DrawSolidCircle(circle.Center, circle.Radius, Color.White);
            }
            if (bounds is RectangleF rectangle)
            {
                DrawSolidRectangle(rectangle.TopLeft, rectangle.Width, rectangle.Height, Color.White);
            }
        }

        _primitiveBatch.End();

        spriteBatch.Begin(transformMatrix: view);

        var last = sorted.First();
        var origin = sorted.First().VertexC;
        foreach (var triangle in sorted)
        {
            var len = (last.VertexB - triangle.VertexA).Length();
            if (!origin.AreCollinear(last.VertexB, triangle.VertexA) || len <= 150)
            {
                last = triangle;
                continue;
            }
            DrawVisibilityLine(spriteBatch, len, triangle.VertexA, last.VertexB);

            last = triangle;
        }
        foreach (var triangle in sorted)
        {
            var len = (triangle.VertexB - triangle.VertexA).Length();

            DrawVisibilityLine(spriteBatch, len, triangle.VertexA, triangle.VertexB);
        }

        spriteBatch.End();

        _graphicsDevice.SetRenderTarget(null);

        spriteBatch.Begin(blendState: _lightBlend);
        spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
        spriteBatch.End();
    }

    private void DrawVisibilityLine(SpriteBatch spriteBatch, float len, Vector2 lineStart, Vector2 lineEnd)
    {
        var angle = (lineStart - lineEnd).ToAngle();

        var steps = len / 150;
        for (var i = 0f; i < (int)steps; i++)
        {
            var step = i / steps;
            var start = Vector2.Lerp(lineStart, lineEnd, step);
            spriteBatch.Draw(_line,
                              start,
                              Color.White,
                              angle,
                              new Vector2(16, 0),
                              new Vector2(1, 1),
                              SpriteEffects.None,
                              0f);
        }
        var step2 = (int)steps / steps;

        var start2 = Vector2.Lerp(lineStart, lineEnd, step2);
        spriteBatch.Draw(_line,
                          start2,
                          Color.White,
                          angle,
                          new Vector2(16, 0),
                          new Vector2(1, steps - (int)steps),
                          SpriteEffects.None,
                          0f);
    }
}