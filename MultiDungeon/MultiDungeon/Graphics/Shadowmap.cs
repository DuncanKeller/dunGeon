using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Graphics
{
    static class Shadowmap
    {
        static QuadRenderComponent quadRender;
        static ShadowmapResolver shadowmapResolver;
        static RenderTarget2D screenShadows;
        static LightArea light;

        static GraphicsDevice graphics;

        public static void Init(Game game, GraphicsDevice g, ContentManager c) 
        {
            graphics = g;
            quadRender = new QuadRenderComponent(game);
            game.Components.Add(quadRender);
            shadowmapResolver = new ShadowmapResolver(g, quadRender,
                ShadowmapSize.Size1024, ShadowmapSize.Size1024);
            shadowmapResolver.LoadContent(c);
            light = new LightArea(g, ShadowmapSize.Size1024);
            screenShadows = new RenderTarget2D(g, 
                g.Viewport.Width, g.Viewport.Height);
        }

        public static void Update(Vector2 p)
        {
            light.LightPosition = new Vector2(p.X, p.Y);
        }

        public static void Draw(SpriteBatch sb, Camera cam, Color c)
        {
            // light
            light.BeginDrawingShadowCasters();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                   DepthStencilState.Default, RasterizerState.CullNone, null, cam.getTransformation());
            World.Map.DrawShadowWalls(sb, light.LightPosition);
            //World.Map.DrawSides(sb);
            sb.End();

            light.EndDrawingShadowCasters();
            shadowmapResolver.ResolveShadows(light.RenderTarget, light.RenderTarget,
                light.LightPosition);

            graphics.SetRenderTarget(screenShadows);
            graphics.Clear(c);
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, cam.getTransformation());
            sb.Draw(light.RenderTarget, light.LightPosition - light.LightAreaSize * 0.5f, Color.White);
            sb.End();

            graphics.SetRenderTarget(null);

            graphics.Clear(Color.Black);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone, null, cam.getTransformation());
            World.DrawGroundTiles(sb, light.LightPosition);
            sb.End();

            BlendState blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            sb.Begin(SpriteSortMode.Immediate, blendState, SamplerState.PointClamp,
                    DepthStencilState.Default, RasterizerState.CullNone, null);
            sb.Draw(screenShadows, new Vector2(0, 0), Color.White);
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
               DepthStencilState.Default, RasterizerState.CullNone, null, cam.getTransformation());
            World.DrawWallTiles(sb, light.LightPosition, 2);
            World.Map.DrawSides(sb, light.LightPosition);
            sb.End();

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
               DepthStencilState.Default, RasterizerState.CullNone, null, cam.getTransformation());
            World.DrawSceneBehindPlayer(sb);
            World.DrawPlayers(sb);
            World.DrawWallTiles(sb, light.LightPosition, 1);
            World.DrawSceneInFrontOfPlayer(sb);
            sb.End();

            sb.Begin();
            if (World.inMenu)
            {
                World.menuManager.Draw(sb);
            }
            sb.End();
        }


    }
}
