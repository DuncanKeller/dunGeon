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
    public struct Upgrade
    {
        public double maxHealth;
        public float maxSpeed;
        public float speed;

        public int damage;
        public float reload;

        public int class1;
        public int class2;
        public int class3;

        public bool power1;
        public bool power2;
    }

    public enum StatusEffect
    {
        none,
        vampire,
        curse,
        cursed
    }

    public abstract class Player
    {
        int id;
        int teamNum;
        Vector2 pos;
        Vector2 velocity;
        float angle;

        float timer = 0;
        public PlayerIndex playerIndex = PlayerIndex.One;
        GamePadState oldGamePad;

        double health;
        double weakness;

        Item item;

        double itemTimer;
        double itemTime = 5; // seconds
        RestoreAction restore = null;
        Chest overlappingChest = null;

        int gold;
        public Upgrade upgrade;
        StatusEffect statusEffect = StatusEffect.none;

        bool alive = true;

        public Color color = Color.White;
        public Color statusColor = Color.White;

        protected List<Gun> guns = new List<Gun>();
        int gunIndex = 0; 

        protected Texture2D characterTest;

        #region Properties

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
                if (value >= 0 && value <= 999)
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
            get { return upgrade.maxHealth; }
        }

        public double Weakness
        {
            get { return weakness; }
            set { weakness = value; }
        }

        public Upgrade Upgrade
        {
            get { return upgrade; }
            set { upgrade = value; }
        }

        public float Speed
        {
            get { return upgrade.maxSpeed; }
            set { upgrade.maxSpeed = value; }
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

        public StatusEffect StatusEffect
        {
            get { return statusEffect; }
            set { statusEffect = value; }
        }

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        public Vector2 Position
        {
            get { return pos; }
        }

        public Vector2 LightPosition
        {
            get { return new Vector2(pos.X + Rect.Width / 2, pos.Y + Rect.Height / 2); }
        }
            
        public Gun CurrentGun
        {
            get { return guns[gunIndex]; }
        }

        public List<Gun> Guns
        {
            get { return guns; }
        }

        public Vector2 CenterPosition
        {
            get
            {
                return new Vector2( pos.X + Rect.Width / 2,
                    pos.Y + Rect.Height / 2);
            }
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

        #endregion

        public Player(float x, float y, int id)
        {
            this.id = id;
            pos = new Vector2(x, y);
            upgrade.maxSpeed = 1;
            velocity = new Vector2();

            upgrade.maxHealth = 6;
            health = upgrade.maxHealth;
        }

        public virtual void Init(int t)
        {
            //ServerClient.Send("position" + "\n" + id.ToString() + "\n" + pos.X.ToString() + "\n" + pos.Y.ToString());
            teamNum = t;
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

            health -= b.Damage + (Weakness * b.Damage);
            // special potions
            if (World.PlayerHash[b.PlayerID].statusEffect == StatusEffect.vampire)
            {
                World.PlayerHash[b.PlayerID].Health += b.Damage / 3;
            }
            else if (World.PlayerHash[b.PlayerID].statusEffect == StatusEffect.curse)
            {
                statusColor = Color.DarkGreen;
                statusEffect = MultiDungeon.StatusEffect.cursed;
                restore = RestoreCurse;
                itemTime = 10; // seconds
            }
            
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

        private void RestoreCurse(Player p)
        {
            p.statusColor = Color.White;
            p.statusEffect = StatusEffect.none;
        }

        private void Die()
        {
            alive = false;
            timer = 0;
            statusEffect = MultiDungeon.StatusEffect.none;
        }

        public void Spawn()
        {
            alive = true;
            Rectangle room = World.Map.GetTeamRoom(teamNum);
            pos.X = GameConst.rand.Next((room.Width * Tile.TILE_SIZE) - Rect.Width) + (room.X * Tile.TILE_SIZE);
            pos.Y = GameConst.rand.Next((room.Height * Tile.TILE_SIZE) - Rect.Height) + (room.Y * Tile.TILE_SIZE);
            health = upgrade.maxHealth;
            foreach (Gun gun in Guns)
            {
                gun.Reset();
            }
            item = null;
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
            else
            {
                // status effect, cursed
                if (statusEffect == StatusEffect.cursed)
                {
                    health -= deltaTime / 3200;

                    if (health < 0 && alive)
                    {
                        Die();
                    }
                }

                if (id == World.gameId)
                {
                    GamePadState gamePad = GamePad.GetState(playerIndex);

                    UpdateInput(gamePad, deltaTime);

                    pos += velocity;
                }

                CurrentGun.Update(deltaTime);
                UpdateChest(World.ItemManager.Chests);
                if (itemTimer > 0)
                {
                    itemTimer -= deltaTime / 1000;
                }
                else
                {
                    itemTimer = 0;
                    if (restore != null)
                    {
                        restore(this);
                        restore = null;
                    }
                }
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
            if (World.inMenu)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return;
            }

            Vector2 input = gamePad.ThumbSticks.Left;

            velocity.X += input.X * upgrade.maxSpeed * (deltaTime / 100);
            velocity.Y += -input.Y * upgrade.maxSpeed * (deltaTime / 100);

            if (velocity.X > input.X * upgrade.maxSpeed)
            {
                velocity.X = input.X * upgrade.maxSpeed;
            }
            else if (velocity.X < input.X * upgrade.maxSpeed)
            {
                velocity.X = input.X * upgrade.maxSpeed;
            }
            if (velocity.Y > -input.Y * upgrade.maxSpeed)
            {
                velocity.Y = -input.Y * upgrade.maxSpeed;
            }
            if (velocity.Y < -input.Y * upgrade.maxSpeed)
            {
                velocity.Y = -input.Y * upgrade.maxSpeed;
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
                CurrentGun.Shoot();
            }
            else if (gamePad.Triggers.Right > 0.25)
            {
                CurrentGun.RightHeld();
            }

            if (gamePad.Triggers.Left > 0.25 &&
              oldGamePad.Triggers.Left < 0.25)
            {
                CurrentGun.SecondaryFire();
            }
            else if (gamePad.Triggers.Left > 0.25)
            {
                CurrentGun.LeftHeld();
            }

            // chests
            if (overlappingChest != null &&
                item == null &&
                !(overlappingChest is TeamChest) &&
                gamePad.Buttons.A == ButtonState.Pressed &&
                oldGamePad.Buttons.A == ButtonState.Released)
            {
                item = overlappingChest.Open(this);
                if (item is CoinPurse)
                {
                    item.Use(this);
                    item = null;
                }
               
                Client.Send("chest" + "\n" + overlappingChest.ID + "!");
            }
            else if (overlappingChest != null &&
              overlappingChest is TeamChest &&
              overlappingChest.ID == teamNum && 
              gamePad.Buttons.A == ButtonState.Pressed &&
              oldGamePad.Buttons.A == ButtonState.Released)
            {
                overlappingChest.Open(this);
            }

            if (item != null && 
                gamePad.Buttons.X == ButtonState.Pressed &&
                oldGamePad.Buttons.X == ButtonState.Released)
            {
                if (restore == null)
                {
                    if (item.EffectTime == 0)
                    {
                        itemTimer = itemTime;
                    }
                    else
                    {
                        itemTimer = item.EffectTime;
                    }
                    restore = item.Use(this);
                    item = null;
                }
            }

            if (item != null &&
                gamePad.Buttons.Back == ButtonState.Pressed &&
                oldGamePad.Buttons.Back == ButtonState.Released)
            {
                item = null;
            }

            if (gamePad.Buttons.B == ButtonState.Pressed &&
                oldGamePad.Buttons.B == ButtonState.Released)
            {
                CurrentGun.Reload();
            }

            if (!CurrentGun.reloading &&
                gamePad.Buttons.RightShoulder == ButtonState.Pressed &&
                oldGamePad.Buttons.RightShoulder == ButtonState.Released)
            {
                gunIndex++;
                if (gunIndex > guns.Count - 1)
                { gunIndex = 0; }
            }
            else if (!CurrentGun.reloading &&
                gamePad.Buttons.LeftShoulder == ButtonState.Pressed &&
                oldGamePad.Buttons.LeftShoulder == ButtonState.Released)
            {
                gunIndex--;
                if (gunIndex < 0)
                { gunIndex = guns.Count - 1; }
            }

            oldGamePad = gamePad;
        }

        public void Draw(SpriteBatch sb)
        {
            if (alive)
            {
                Color c = statusColor == Color.White ? color : statusColor; 
                Vector2 origin = new Vector2(TextureManager.Map["circle"].Width / 2, TextureManager.Map["circle"].Height / 2);
                //sb.Draw(TextureManager.Map["blank"], Rect, Color.Red);
                Rectangle charRect = new Rectangle(Rect.X - 5, Rect.Y - 30, Rect.Width + 10, Rect.Height + 20);

                sb.Draw(TextureManager.Map["circle"], DrawRect, null, color, angle, origin, SpriteEffects.None, 0);
                sb.Draw(characterTest, charRect, c);
            }
        }
    }
}
