using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    static class Console
    {
        const float SPACING = 10;
        const int MAX_QUEUE = 10;

        static float height = GameConst.SCREEN_HEIGHT;
        static List<Message> messages = new List<Message>();
        static Queue<Message> toAdd = new Queue<Message>();

        static bool enabled = true;

        public static bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        static int GetNumMessages()
        {
            int num = 0;

            foreach (Message m in messages)
            {
                num += m.Remove ? 1 : 0;
            }

            return num;
        }

        public static void Update(float deltaTime)
        {
            if (!enabled)
            { return; }
            // update toAdd
            while (toAdd.Count > 0)
            {
                messages.Insert(0, toAdd.Dequeue());
            }

            // remove old messages
            List<Message> readMessages = Console.messages.ToList<Message>();
            readMessages.Reverse();
            int remove = 0;
            foreach (Message m in readMessages)
            {
                if (m.Remove)
                {
                    remove++;
                }
                else
                {
                    m.Update(deltaTime);
                }
            }

            for (int i = 0; i < remove; i++)
            {
                messages.RemoveAt(messages.Count - 1);
            }

            // mark old messages for deletion
            int numToDelete = Math.Max(GetNumMessages() - MAX_QUEUE, 0);

            for (int i = 1; i <= numToDelete; i++)
            {
                messages[messages.Count - i].Destroy();
            }

            // update letter height
            float letterHieght = TextureManager.Fonts["console"].MeasureString("I").Y;
            int idealHeight = GameConst.SCREEN_HEIGHT - (int)((messages.Count * SPACING) + (messages.Count * letterHieght));

            float interp = (height - idealHeight) / 10;
            height -= interp;
        }

        public static void Write(string m, MessageType t = MessageType.regular)
        {
            if (!enabled)
            { return; }
            Message message = new Message(m, t);
            toAdd.Enqueue(message);
        }

        public static void Draw(SpriteBatch sb)
        {
            if (!enabled)
            { return; }
            float letterHieght = TextureManager.Fonts["console"].MeasureString("I").Y;
            List<Message> messages = Console.messages.ToList<Message>();

            for (int i = 0; i < messages.Count; i++)
            {
                int drawHeight = (int)(height + (i * SPACING) + (i * letterHieght));
                messages[i].Draw(sb, new Vector2(15, drawHeight));
            }
        }
    }
}
