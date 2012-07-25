using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;
using MultiDungeon.Items;
using MultiDungeon.Effects;

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
        cursed,
        invinsible,
        midas,
        homing,
        confuse,
        speed,
        health
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
        KeyboardState oldKeyboard;
        MouseState oldMouse;

        double health;
        double weakness;

        Item item;
        double itemTimer;
        double itemTime = 5; // seconds
        RestoreAction restore = null;
        Chest overlappingChest = null;

        int gold = 0;
        public Upgrade upgrade;
        StatusEffect statusEffect = StatusEffect.none;

        bool alive = true;

        public Color color = Color.White;
        public Color statusColor = Color.White;

        protected List<Weapon> guns = new List<Weapon>();
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
            set { pos = value; }
        }

        public Vector2 LightPosition
        {
            get { return new Vector2(pos.X + Rect.Width / 2, pos.Y + Rect.Height / 2); }
        }

        public Weapon CurrentGun
        {
            get { return guns[gunIndex]; }
        }

        public List<Weapon> Guns
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


            if (World.PlayerHash[b.PlayerID].statusEffect == StatusEffect.invinsible)
            {
                b.Angle += (float)(Math.PI + ((GameConst.rand.NextDouble() * (Math.PI / 7)) - Math.PI / 14));
            }
            else
            {
                health -= b.Damage + (Weakness * b.Damage);
            }

            if (health < 0 && alive)
            {
                Die();
                if (teamNum != World.PlayerHash[b.PlayerID].Team)
                {
                    int bonus = World.PlayerHash[b.PlayerID].StatusEffect == MultiDungeon.StatusEffect.midas ? 50 : 0;
                    World.PlayerHash[b.PlayerID].Gold += 50 + bonus;
                }
                else
                {
                    World.PlayerHash[b.PlayerID].Gold -= 50;
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    int x = DrawRect.Left + GameConst.rand.Next(DrawRect.Width);
                    int y = DrawRect.Top + GameConst.rand.Next(DrawRect.Height);
                    World.BulletManager.AddParticle(new BloodParticle(new
                        Vector2(x, y)));
                }
            }
        }

        private void RestoreCurse(Player p)
        {
            p.statusColor = Color.White;
            p.statusEffect = StatusEffect.none;
        }

        public void Die()
        {
            if (alive)
            {
                alive = false;
                timer = 0;
                statusEffect = MultiDungeon.StatusEffect.none;
                item = null;
                gold -= 50;
                if (gold < 0)
                {
                    gold = 0;
                }
                itemTimer = 0;
                if (restore != null)
                {
                    restore(this);
                    restore = null;
                }
                for (int i = 0; i < 50; i++)
                {
                    int x = DrawRect.Left + GameConst.rand.Next(DrawRect.Width);
                    int y = DrawRect.Top + GameConst.rand.Next(DrawRect.Height);
                    World.BulletManager.AddParticle(new BloodParticle(new
                        Vector2(x, y)));
                }
                World.BulletManager.AddParticle(new SkullParticle(new
                       Vector2(DrawRect.Center.X, DrawRect.Top)));
            }
        }

        public void Spawn()
        {
            alive = true;
            Rectangle room = World.Map.GetTeamRoom(teamNum);
            pos.X = GameConst.rand.Next((room.Width * Tile.TILE_SIZE) - Rect.Width) + (room.X * Tile.TILE_SIZE);
            pos.Y = GameConst.rand.Next((room.Height * Tile.TILE_SIZE) - Rect.Height) + (room.Y * Tile.TILE_SIZE);
            health = upgrade.maxHealth;
            foreach (Weapon wep in Guns)
            {
                if (wep is Gun)
                {
                    (wep as Gun).Reset();
                }
            }
        }

        public virtual void Update(float deltaTime)
        {
            if (!alive)
            {
                timer += deltaTime / 1000;

                if (timer > 1.2)
                {
                    Spawn();
                    timer = 0;
                }
            }
            else
            {
                switch (statusEffect)
                {
                    case MultiDungeon.StatusEffect.curse:
                        health -= deltaTime / 3200;
                        if (health < 0 && alive)
                        { Die(); }
                        EffectManager.Update(deltaTime, typeof(PoisinParticle), this);
                        break;
                    case MultiDungeon.StatusEffect.invinsible:
                        EffectManager.Update(deltaTime, typeof(StarParticle), this);
                        break;
                    case MultiDungeon.StatusEffect.speed:
                        EffectManager.Update(deltaTime, typeof(SpeedParticle), this);
                        break;
                    case MultiDungeon.StatusEffect.health:
                        EffectManager.Update(deltaTime, typeof(HealthParticle), this);
                        break;
                }


                if (id == World.gameId)
                {
                    GamePadState gamePad = GamePad.GetState(playerIndex);
                    KeyboardState keyboard = Keyboard.GetState();
                    MouseState mouse = Mouse.GetState();

                    UpdateInput(gamePad, keyboard, mouse, deltaTime);

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


        public void UpdateInput(GamePadState gamePad, KeyboardState keyboard, 
            MouseState mouse, float deltaTime)
        {
            if (World.inMenu || !World.ReadyToPlay)
            {
                velocity.X = 0;
                velocity.Y = 0;
                return;
            }

            Vector2 input = gamePad.ThumbSticks.Left;

            velocity.X += input.X * upgrade.maxSpeed * (deltaTime / 100);
            velocity.Y += -input.Y * upgrade.maxSpeed * (deltaTime / 100);

            // keyboard input
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += upgrade.maxSpeed * (deltaTime / 100);
                input.X = 1;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= upgrade.maxSpeed * (deltaTime / 100);
                input.X = -1;
            }
            if (keyboard.IsKeyDown(Keys.W))
            {
                velocity.Y += upgrade.maxSpeed * (deltaTime / 100);
                input.Y = 1;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                velocity.Y -= upgrade.maxSpeed * (deltaTime / 100);
                input.Y = -1;
            }

            // cap max speed
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
            else if (velocity.Y < -input.Y * upgrade.maxSpeed)
            {
                velocity.Y = -input.Y * upgrade.maxSpeed;
            }

            Vector2 dir = gamePad.ThumbSticks.Right;

            if (Math.Abs(dir.X) > .25 ||
                Math.Abs(dir.Y) > .25)
            {
                angle = (float)Math.Atan2(dir.X, dir.Y);
            }
            else
            {
                angle = (float)Math.Atan2(mouse.X - (GameConst.SCREEN_WIDTH / 2),
                     (GameConst.SCREEN_HEIGHT / 2) - mouse.Y);
            }

            if (CurrentGun is Gun)
            {
                if (gamePad.Triggers.Right > 0.25 &&
                    oldGamePad.Triggers.Right < 0.25 ||
                    mouse.LeftButton == ButtonState.Pressed &&
                     oldMouse.LeftButton == ButtonState.Released)
                {
                    Gun g = (Gun)CurrentGun;
                    g.Shoot();
                }
                else if (gamePad.Triggers.Right > 0.25 ||
                    mouse.LeftButton == ButtonState.Pressed )
                {
                    Gun g = (Gun)CurrentGun;
                    g.RightHeld();
                }

                if (gamePad.Triggers.Left > 0.25 &&
                    oldGamePad.Triggers.Left < 0.25||
                    mouse.RightButton == ButtonState.Pressed &&
                    oldMouse.RightButton == ButtonState.Released)
                {
                    Gun g = (Gun)CurrentGun;
                    g.SecondaryFire();
                }
                else if (gamePad.Triggers.Left > 0.25 ||
                    mouse.RightButton == ButtonState.Pressed)
                {
                    Gun g = (Gun)CurrentGun;
                    g.LeftHeld();
                }
            }
            // chests
            if (gamePad.Buttons.A == ButtonState.Pressed &&
                oldGamePad.Buttons.A == ButtonState.Released ||
                keyboard.IsKeyDown(Keys.E) &&
                oldKeyboard.IsKeyUp(Keys.E))
            {
                if (overlappingChest != null &&
                    item == null &&
                    !(overlappingChest is TeamChest))
                {
                    item = overlappingChest.Open(this);
                    if (item is CoinPurse)
                    {
                        item.Use(this);
                        item = null;
                    }

                    Client.Send("chest" + "\n" + overlappingChest.ID);
                }
                else if (overlappingChest != null &&
                    overlappingChest is TeamChest &&
                    overlappingChest.ID == teamNum)
                {
                    overlappingChest.Open(this);
                }
            }
            

            if (item != null)
            {
                if (gamePad.Buttons.X == ButtonState.Pressed &&
                    oldGamePad.Buttons.X == ButtonState.Released ||
                    keyboard.IsKeyDown(Keys.Space) &&
                    oldKeyboard.IsKeyUp(Keys.Space))
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
            }

            if (item != null &&
                gamePad.Buttons.Back == ButtonState.Pressed &&
                oldGamePad.Buttons.Back == ButtonState.Released ||
                item != null &&
                keyboard.IsKeyDown(Keys.Back) &&
                oldKeyboard.IsKeyUp(Keys.Back))
            {
                item = null;
            }
            if (CurrentGun is Gun)
            {
                Gun g = (Gun)CurrentGun;
                if (gamePad.Buttons.B == ButtonState.Pressed &&
                    oldGamePad.Buttons.B == ButtonState.Released ||
                    keyboard.IsKeyDown(Keys.R) &&
                    oldKeyboard.IsKeyUp(Keys.R))
                {
                    g.Reload();
                }
            }
            bool ableToSwitch = true;
            if (CurrentGun is Gun)
            {
                if ((CurrentGun as Gun).reloading)
                {
                    ableToSwitch = false;
                }
            }
            if (ableToSwitch)
            {
                if (gamePad.Buttons.RightShoulder == ButtonState.Pressed &&
                    oldGamePad.Buttons.RightShoulder == ButtonState.Released ||
                    mouse.ScrollWheelValue > oldMouse.ScrollWheelValue)
                {
                    gunIndex++;
                    if (gunIndex > guns.Count - 1)
                    { gunIndex = 0; }
                }
                else if (gamePad.Buttons.LeftShoulder == ButtonState.Pressed &&
                    oldGamePad.Buttons.LeftShoulder == ButtonState.Released ||
                    mouse.ScrollWheelValue < oldMouse.ScrollWheelValue)
                {
                    gunIndex--;
                    if (gunIndex < 0)
                    { gunIndex = guns.Count - 1; }
                }
            }
            if (CurrentGun is Weapons.Sword)
            {
                Weapons.Sword sword = CurrentGun as Weapons.Sword;
                if (gamePad.Triggers.Right > 0.25 &&
                    oldGamePad.Triggers.Right < 0.25 ||
                    mouse.LeftButton == ButtonState.Pressed &&
                    oldMouse.LeftButton == ButtonState.Released)
                {
                    sword.Slice(angle, pos);
                    Client.Send("slice\n" + World.gameId + "!");
                }
            }

            //Vector2 newPos = World.Camera.ToLocalLocation(this.pos);
            //Mouse.SetPosition((int)newPos.X, (int)newPos.Y);

            oldKeyboard = keyboard;
            oldMouse = mouse;
            oldGamePad = gamePad;
        }

        public void Draw(SpriteBatch sb)
        {
            if (alive)
            {
                Color c = statusColor == Color.White ? color : statusColor;
                Vector2 origin = new Vector2(TextureManager.Map["arrow"].Width / 2, TextureManager.Map["arrow"].Height / 2);
                //sb.Draw(TextureManager.Map["blank"], Rect, Color.Red);
                Rectangle charRect = new Rectangle(Rect.X - 5, Rect.Y - 30, Rect.Width + 10, Rect.Height + 20);

                sb.Draw(TextureManager.Map["arrow"], DrawRect, null, color, angle, origin, SpriteEffects.None, 0);
                if (statusEffect == MultiDungeon.StatusEffect.confuse)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        byte rand = (byte)GameConst.rand.Next(255);
                        c.A = rand; c.B = rand; c.R = rand; c.G = rand;
                        Rectangle r = charRect;
                        int range = 160;
                        r.X += GameConst.rand.Next(range) - (range / 2);
                        r.Y += GameConst.rand.Next(range) - (range / 2);
                        sb.Draw(characterTest, r, c);
                    }
                }
                else
                {
                    sb.Draw(characterTest, charRect, c);
                }

                if (World.Player is Mystic && item != null && World.Player != this)
                {
                    sb.Draw(Item.Texture, new Rectangle(DrawRect.X, DrawRect.Y - DrawRect.Width - 10, DrawRect.Width, DrawRect.Width),
                        new Color(100, 100, 100, 100));
                }
                CurrentGun.Draw(sb);
            }
            if (World.inMenu == false)
            {
                int mousex = Mouse.GetState().X;
                int mousey = Mouse.GetState().Y;

                //sb.Draw(TextureManager.Map["circle"], new Rectangle(mousex - 3, mousey - 3, 6, 6), Color.Red);
                Vector2 mouse = new Vector2(oldMouse.X, oldMouse.Y);
                mouse = World.Camera.ToWorldLocation(mouse);
                float dist = Vector2.Distance(mouse, pos);

                int maxDist = 100;

                if (dist > maxDist)
                {
                    if (alive)
                    {
                        int x = (int)(Math.Cos(angle - Math.PI / 2) * maxDist);
                        int y = (int)(Math.Sin(angle - Math.PI / 2) * maxDist);
                        Vector2 translate = new Vector2(x, y);
                        translate = World.Camera.ToLocalLocation(translate);
                        Mouse.SetPosition(x + GameConst.SCREEN_WIDTH / 2, y + GameConst.SCREEN_WIDTH / 2);
                    }
                }
            }
        }
    }
}
