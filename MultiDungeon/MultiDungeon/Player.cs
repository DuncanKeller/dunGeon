using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Player
    {
        int id;
        Vector2 pos;
        Vector2 velocity;
        float speed;
        float maxSpeed;

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

        public Player(float x, float y, int id = 0)
        {
            this.id = id;
            pos = new Vector2(x, y);
            speed = 1;
            maxSpeed = 5;
            velocity = new Vector2();
        }

        public void SetPos(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Update(float deltaTime)
        {
            if (id == World.gameId)
            {
                GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

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

                pos += velocity;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.Map["circle"], new Rectangle((int)pos.X, (int)pos.Y, 30, 30), c);
        }
    }
}
