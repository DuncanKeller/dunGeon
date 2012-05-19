using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;
using MultiDungeon.Items;

namespace MultiDungeon
{
    struct Upgrade
    {
        int maxHealth;
        int speed;

        int damage;
        int reload;

        int class1;
        int class2;
        int class3;
    }

    abstract class Player
    {
        int id;
        int teamNum;
        Vector2 pos;
        Vector2 velocity;
        float speed;
        float maxSpeed;
        float angle;

        float timer = 0;
        public PlayerIndex playerIndex = PlayerIndex.One;
        GamePadState oldGamePad;

        Item item;
        double health;
        double maxHealth;
        int gold;

        Upgrade upgrade;

        bool alive = true;

        public Color c = Color.White;

        Gun testGun;
        Chest overlappingChest = null;

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public int Gold
        {
            get { return gold; }
            set
            {
                if (value > 0 && value <= 999)
                { gold = value; }
                if (gold < 0)
                { gold = 0; }
            }
        }

        public int Team
        {
            get { return teamNum; }
        }

        public bool Alive
        {
            get { return alive; }
        }

        public double MaxHealth
        {
            get { return maxHealth; }
        }

        public double Health
        {
            get { return health; }
            set
            {
                if (value > 0 && value <= 999)
                { health = value; }
            }
        }

        public Item Item
        {
            get { return item; }
        }

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

        public Rectangle DrawRect
        {
            get { return new Rectangle((int)pos.X + 15, (int)pos.Y + 15, 30, 30); }
        }


        public Player(float x, float y, int id)
        {
            this.id = id;
            pos = new Vector2(x, y);
            speed = 1;
            maxSpeed = 5;
            velocity = new Vector2();
            testGun = new AssaultRifle(World.BulletManager, this);

            maxHealth = 5;
            health = maxHealth;
        }

        public void Init(int t)
        {
            //ServerClient.Send("position" + "\n" + id.ToString() + "\n" + pos.X.ToString() + "\n" + pos.Y.ToString());
            teamNum = t;
            if (teamNum == 0)
            {
                c = Color.Blue;
            }
            else
            {
                c = Color.Red;
            }

        }

        public void SetPos(float x, float y)
        {
            pos.X = x;
            pos.Y = y;
        }

        public void Hit(Bullet b)
        {
            Vector2 veloc = new Vector2((float)Math.Cos(b.Angle),
                (float)Math.Sin(b.Angle));
            veloc.Normalize();
            veloc *= 3;
            velocity += veloc;

            health -= b.Damage;
            
            if (health < 0 && alive)
            {
                Die();
                if (teamNum != World.PlayerHash[b.PlayerID].Team)
                {
                    World.PlayerHash[b.PlayerID].Gold += 50;
                }
                else
                {
                    World.PlayerHash[b.PlayerID].Gold -= 50;
                }
            }
        }

        private void Die()
        {
            alive = false;
            timer = 0;
        }

        public void Spawn()
        {
            alive = true;
            Rectangle room = World.Map.GetTeamRoom(teamNum);
            pos.X = GameConst.rand.Next((room.Width * Tile.TILE_SIZE) - Rect.Width) + (room.X * Tile.TILE_SIZE);
            pos.Y = GameConst.rand.Next((room.Height * Tile.TILE_SIZE) - Rect.Height) + (room.Y * Tile.TILE_SIZE);
            health = maxHealth;
        }

        public virtual void Update(float deltaTime)
        {
            if (!alive)
            {
                timer += deltaTime;

                if (timer > 1000)
                {
                    Spawn();
                    timer = 0;
                }
            }
            
            if (alive)
            {
                if (id == World.gameId)
                {
                    GamePadState gamePad = GamePad.GetState(playerIndex);

                    UpdateInput(gamePad, deltaTime);

                    pos += velocity;
                }

                testGun.Update(deltaTime);
                UpdateChest(World.ItemManager.Chests);
            }
        }

        public void UpdateChest(List<Chest> chests)
        {
            bool overlapping = false;
            foreach (Chest chest in chests)
            {
                if (chest.Rect.Intersects(DrawRect))
                {
                    overlappingChest = chest;
                    overlapping = true;
                }
            }

            if (!overlapping)
            {
                overlappingChest = null;
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

            if (Math.Abs(dir.X) > .25 ||
                Math.Abs(dir.Y) > .25)
            {
                angle = (float)Math.Atan2(dir.X, dir.Y);
            }
            
            if (gamePad.Triggers.Right > 0.25 &&
                oldGamePad.Triggers.Right < 0.25)
            {
                testGun.Shoot();
            }
            else if (gamePad.Triggers.Right > 0.25)
            {
                testGun.RightHeld();
            }

            if (gamePad.Triggers.Left > 0.25 &&
              oldGamePad.Triggers.Left < 0.25)
            {
                testGun.SecondaryFire();
            }
            else if (gamePad.Triggers.Left > 0.25)
            {
                testGun.LeftHeld();
            }

            // chests
            if (overlappingChest != null &&
                item == null
                && gamePad.Buttons.A == ButtonState.Pressed &&
                oldGamePad.Buttons.A == ButtonState.Released)
            {
                item = overlappingChest.Open(this);
                if (item is CoinPurse)
                {
                    item.Use(this);
                    item = null;
                }

            }

            if (item != null && 
                gamePad.Buttons.X == ButtonState.Pressed &&
                oldGamePad.Buttons.X == ButtonState.Released)
            {
                item.Use(this);
                item = null;
            }

          

            oldGamePad = gamePad;
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 origin = new Vector2(TextureManager.Map["circle"].Width / 2, TextureManager.Map["circle"].Height / 2);
            //sb.Draw(TextureManager.Map["blank"], Rect, Color.Red);
            sb.Draw(TextureManager.Map["circle"], DrawRect, null, c, angle, origin, SpriteEffects.None, 0);
           
        }
    }
}
