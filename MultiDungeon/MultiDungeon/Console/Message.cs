using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    enum MessageType
    {
        regular,
        special,
        warning,
        urgent,
        fatal
    }

    class Message
    {
        string content;
        Color color;
        MessageType type;
        bool remove = false;
        bool destroy = false;
        float fade = 0;
        float lifetime = 1.5f;
        float life;

        public bool Remove
        {
            get { return remove; }
            set { remove = value; }
        }

        public void Destroy()
        {
            destroy = true;
        }

        public Message(string c, MessageType t)
        {
            type = t;
            content = c;
            life = lifetime;

            SetColor();
        }

        public void Update(float deltaTime)
        {
            if (!destroy && fade < 1)
            {
                fade = fade > 1 ? 1 : fade + 0.1f;
            }
            else if (destroy && fade > 0)
            {
                fade = fade < 0 ? 0 : fade - 0.1f;
            }
            else if (destroy && fade <= 0)
            {
                remove = true;
            }

            if (life > 0 && !destroy)
            {
                life -= deltaTime / 1000;
            }
            else
            {
                destroy = true;
            }
        }

        public void SetColor()
        {
            switch (type)
            {
                case MessageType.regular:
                    color = Color.LightGray;
                    break;
                case MessageType.special:
                    color = Color.White;
                    break;
                case MessageType.warning:
                    color = Color.Yellow;
                    break;
                case MessageType.urgent:
                    color = Color.Orange;
                    break;
                case MessageType.fatal:
                    color = Color.Red;
                    break;
            }
        }

        public void Draw(SpriteBatch sb, Vector2 v)
        {
            sb.DrawString(TextureManager.Fonts["console"], content, v, color * (float)fade);
        }
    }
}
