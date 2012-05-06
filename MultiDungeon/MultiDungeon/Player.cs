using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon
{
    class Player
    {
        int id;
        Vector2 pos;
        Vector2 velocity;
        float speed;
        float maxSpeed;
        float angle;

        float timer = 0;

        public Color c = new Color(0,255,0);

        public Vector2 Position
        {
            get { return pos; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public Rectangle Rect
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y, 30, 30); }
        }

        public Player(float x, float y, int id = 0)
        {
            this.id = id;
            pos = new Vector2(x, y);
            speed = 1;
            maxSpeed = 5;
            velocity = new Vector2();

            Init();
        }

        public void Init()
        {
            //ServerClient.Send("position" + "\n" + id.ToString() + "\n" + pos.X.ToString() + "\n" + pos.Y.ToString());
        }

        public void SetPos(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Update(float deltaTime)
        {
            timer += deltaTime;

            if (timer > 100)
            {
                timer = 0;
            }

            if (id == World.gameId)
            {
                GamePadState gamePad = GamePad.GetState(PlayerIndex.One);


                UpdateInput(gamePad, deltaTime);


                pos += velocity;
            }
        }

        public void UpdateCollisions(List<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                if (t.Type == TileType.wall)
                {
                    if (Rect.Intersects(t.Rect))
                    {
                        Vector2 colVect = Vector2.Zero;
                        colVect.X = Rect.Center.X > t.Rect.Center.X ? t.Rect.Right - Rect.Left : -(Rect.Right - t.Rect.Left);
                        colVect.Y = Rect.Center.Y > t.Rect.Center.Y ? t.Rect.Bottom - Rect.Top : -(Rect.Bottom - t.Rect.Top);

                        if (Math.Abs(colVect.X) > Math.Abs(colVect.Y))
                        {
                            pos.Y += colVect.Y;
                        }
                        else
                        {
                            pos.X += colVect.X;
                        }
                    }
                }
            }

        }

        public void UpdateInput(GamePadState gamePad, float deltaTime)
        {
            Vector2 input = gamePad.ThumbSticks.Left;

            velocity.X += input.X * speed * (deltaTime / 100);
            velocity.Y += -input.Y * speed * (deltaTime / 100);

            if (velocity.X > input.X * maxSpeed)
            {
                velocity.X = input.X * maxSpeed;
            }
            else if (velocity.X < input.X * maxSpeed)
            {
                velocity.X = input.X * maxSpeed;
            }
            if (velocity.Y > -input.Y * maxSpeed)
            {
                velocity.Y = -input.Y * maxSpeed;
            }
            if (velocity.Y < -input.Y * maxSpeed)
            {
                velocity.Y = -input.Y * maxSpeed;
            }

            Vector2 dir = gamePad.ThumbSticks.Right;

            angle = (float)Math.Atan2(dir.X, dir.Y);
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 origin = new Vector2(TextureManager.Map["circle"].Width / 2, TextureManager.Map["circle"].Height / 2);
            sb.Draw(TextureManager.Map["circle"], Rect, null, c, angle, origin, SpriteEffects.None, 0);
        }
    }
}
