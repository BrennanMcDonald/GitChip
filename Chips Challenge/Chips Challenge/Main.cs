using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TileEngine;

/*
 *  Floor   =   0       =   48
 *  Keys    =   1,2,3,4 =   49,50,51,52
 *  Walls   =   5       =   53
 *  Doors   =   6,7,8,9 =   54,55,56,57
 *  End     =   /       =   47
 *  Chip    =   *       =   42
 */

namespace Chips_Challenge
{
    public class ChipsChallengeMain : Microsoft.Xna.Framework.Game
    {
        const int floor = 48,
            wall = 53,
            redKey = 49,
            redDoor = 54,
            blueKey = 50,
            blueDoor = 55,
            greenKey = 51,
            greenDoor = 56,
            goldKey = 52,
            goldDoor = 57,
            portal = 47,
            chip = 42;
            
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont Font1;
        private KeyboardState prevKeys;

        private Inventory inv;

        public Texture2D wallTexture,
            floorTexture,
            redKeyTexture,
            chipTexture,
            redDoorTexture,
            blueKeyTexture,
            blueDoorTexture,
            greenDoorTexture,
            greenKeyTexture,
            backgroundTexture;

        public Vector2 boardOffset = new Vector2(50, 50);
        public Point player;
        public Queue<TileMap> levelList = new Queue<TileMap>();
        public TileMap map;
        public bool firstRun = true;

        public ChipsChallengeMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 
                9 * 32 + (2 * (int)boardOffset.Y);
            graphics.PreferredBackBufferWidth = 
                9 * 32 + 200 + (2 * (int)boardOffset.X);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            wallTexture = Content.Load<Texture2D>("wall");
            floorTexture = Content.Load<Texture2D>("floor");

            redKeyTexture = Content.Load<Texture2D>("redKey");
            blueKeyTexture = Content.Load<Texture2D>("blueKey");
            greenKeyTexture = Content.Load<Texture2D>("greenKey");

            redDoorTexture = Content.Load<Texture2D>("redDoor");
            blueDoorTexture = Content.Load<Texture2D>("blueDoor");
            greenDoorTexture = Content.Load<Texture2D>("greenDoor");

            chipTexture = Content.Load<Texture2D>("chip");
            backgroundTexture = Content.Load<Texture2D>("bg");

            Font1 = Content.Load<SpriteFont>("Font1");
            inv = new Inventory();

            levelList.Enqueue(new TileMap("level.txt"));
            levelList.Enqueue(new TileMap("level2.txt"));
            levelList.Enqueue(new TileMap("level3.txt"));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (firstRun)
            {
                map = levelList.Dequeue();
                firstRun = false;
                player = map.startPos;
            }
            if (map.Rows[(int)player.X].Columns[(int)player.Y].TileID == portal)
            {
                map = levelList.Dequeue();
                player = map.startPos;
            }
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                KeyboardState ks = Keyboard.GetState();
                if (ks != prevKeys)
                {
                    if (ks.IsKeyDown(Keys.Right))
                    {
                        if ((int)player.X + 1 < map.MapWidth)
                        {
                            switch (map.Rows[(int)player.Y].Columns[(int)player.X + 1].TileID)
                            {
                                case redDoor:
                                    if (inv.redKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X + 1].TileID = floor;
                                        inv.redKeys--;
                                    }
                                    break;
                                case greenDoor:
                                    if (inv.greenKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X + 1].TileID = floor;
                                        inv.greenKeys--;
                                    }
                                    break;
                                case blueDoor:
                                    if (inv.blueKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X + 1].TileID = floor;
                                        inv.blueKeys--;
                                    }
                                    break;
                                case goldDoor:
                                    if (inv.goldKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X + 1].TileID = floor;
                                        inv.goldKeys--;
                                    }
                                    break;
                            }
                            if (map.Rows[(int)player.Y].Columns[(int)player.X + 1].TileID < wall)
                                player.X += 1;
                        }
                    }
                    else if (ks.IsKeyDown(Keys.Left))
                    {
                        if (player.X > 0)
                        {
                            switch (map.Rows[(int)player.Y].Columns[(int)player.X - 1].TileID)
                            {
                                case redDoor:
                                    if (inv.redKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X - 1].TileID = floor;
                                        inv.redKeys--;
                                    }
                                    break;
                                case greenDoor:
                                    if (inv.greenKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X - 1].TileID = floor;
                                        inv.greenKeys--;
                                    }
                                    break;
                                case blueDoor:
                                    if (inv.blueKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X - 1].TileID = floor;
                                        inv.blueKeys--;
                                    }
                                    break;
                                case goldDoor:
                                    if (inv.goldKeys > 0)
                                    {
                                        map.Rows[(int)player.Y].Columns[(int)player.X - 1].TileID = floor;
                                        inv.goldKeys--;
                                    }
                                    break;
                            }
                            if (map.Rows[(int)player.Y].Columns[(int)player.X - 1].TileID < wall)
                                player.X -= 1;
                        }
                    }
                    else if (ks.IsKeyDown(Keys.Down))
                    {
                        if (player.Y + 1 < map.MapHeight)
                        {
                            switch (map.Rows[(int)player.Y + 1].Columns[(int)player.X].TileID)
                            {
                                case redDoor:
                                    if (inv.redKeys > 0)
                                    {
                                        map.Rows[(int)player.Y + 1].Columns[(int)player.X].TileID = floor;
                                        inv.redKeys--;
                                    }
                                    break;
                                case greenDoor:
                                    if (inv.greenKeys > 0)
                                    {
                                        map.Rows[(int)player.Y + 1].Columns[(int)player.X].TileID = floor;
                                        inv.greenKeys--;
                                    }
                                    break;
                                case blueDoor:
                                    if (inv.blueKeys > 0)
                                    {
                                        map.Rows[(int)player.Y + 1].Columns[(int)player.X].TileID = floor;
                                        inv.blueKeys--;
                                    }
                                    break;
                                case goldDoor:
                                    if (inv.goldKeys > 0)
                                    {
                                        map.Rows[(int)player.Y + 1].Columns[(int)player.X].TileID = floor;
                                        inv.goldKeys--;
                                    }
                                    break;
                            }
                            if (map.Rows[(int)player.Y + 1].Columns[(int)player.X].TileID < wall)
                                player.Y += 1;
                        }
                    }
                    else if (ks.IsKeyDown(Keys.Up))
                    {
                        if (player.Y > 0)
                        {
                            switch (map.Rows[(int)player.Y - 1].Columns[(int)player.X].TileID)
                            {
                                case redDoor:
                                    if (inv.redKeys > 0)
                                    {
                                        map.Rows[(int)player.Y - 1].Columns[(int)player.X].TileID = floor;
                                        inv.redKeys--;
                                    }
                                    break;
                                case greenDoor:
                                    if (inv.greenKeys > 0)
                                    {
                                        map.Rows[(int)player.Y - 1].Columns[(int)player.X].TileID = floor;
                                        inv.greenKeys--;
                                    }
                                    break;
                                case blueDoor:
                                    if (inv.blueKeys > 0)
                                    {
                                        map.Rows[(int)player.Y - 1].Columns[(int)player.X].TileID = floor;
                                        inv.blueKeys--;
                                    }
                                    break;
                                case goldDoor:
                                    if (inv.goldKeys > 0)
                                    {
                                        map.Rows[(int)player.Y - 1].Columns[(int)player.X].TileID = floor;
                                        inv.goldKeys--;
                                    }
                                    break;

                            }
                            if (map.Rows[(int)player.Y - 1].Columns[(int)player.X].TileID < wall)
                                player.Y -= 1;
                        }
                    }
                }

                if (map.Rows[(int)player.Y].Columns[(int)player.X].TileID == redKey)
                {
                    inv.redKeys++;
                    map.Rows[(int)player.Y].Columns[(int)player.X].TileID = floor;
                }
                if (map.Rows[(int)player.Y].Columns[(int)player.X].TileID == blueKey)
                {
                    inv.blueKeys++;
                    map.Rows[(int)player.Y].Columns[(int)player.X].TileID = floor;
                }
                if (map.Rows[(int)player.Y].Columns[(int)player.X].TileID == greenKey)
                {
                    inv.greenKeys++;
                    map.Rows[(int)player.Y].Columns[(int)player.X].TileID = floor;
                }
                if (map.Rows[(int)player.Y].Columns[(int)player.X].TileID == portal)
                    map = levelList.Dequeue();
                prevKeys = ks;
                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            // Draw Background
            spriteBatch.Draw(floorTexture,
                new Rectangle((int)boardOffset.X - 6, (int)boardOffset.Y - 6, map.MapWidth * 32 + 18, map.MapHeight * 32 + 18),
                Color.White);
            // Draw board in a 2 deep Height,Width for loop
            for(int y = 0; y < map.MapHeight; y++)
            {
                for(int x = 0; x < map.MapWidth; x++)
                {
                    if (map.Rows[y].Columns[x].TileID == wall)
                        spriteBatch.Draw(wallTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32), 
                            Color.White); // Wall
                    else if (map.Rows[y].Columns[x].TileID == floor || map.Rows[y].Columns[x].TileID == portal)
                        spriteBatch.Draw(floorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32), 
                            Color.White); // Floor
                    else if (map.Rows[y].Columns[x].TileID == redKey)
                        spriteBatch.Draw(redKeyTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32), 
                            Color.White); // Red key
                    else if (map.Rows[y].Columns[x].TileID == blueKey)
                        spriteBatch.Draw(blueKeyTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Blue key
                    else if (map.Rows[y].Columns[x].TileID == greenKey)
                        spriteBatch.Draw(greenKeyTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Green key
                    else if (map.Rows[y].Columns[x].TileID == redDoor)
                        spriteBatch.Draw(redDoorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Red Door
                    else if (map.Rows[y].Columns[x].TileID == blueDoor)
                        spriteBatch.Draw(blueDoorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Blue Door
                    else if (map.Rows[y].Columns[x].TileID == greenDoor)
                        spriteBatch.Draw(greenDoorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Green Door
                }
            }
            // Draw text for score
            spriteBatch.DrawString(Font1,
                "PlaceHolder",
                new Vector2((2 * (int)boardOffset.Y) + map.MapWidth * 32, (int)boardOffset.X), 
                Color.Black);
            // Draw chip
            spriteBatch.Draw(chipTexture,
                new Rectangle((int)player.X * 32 + (int)boardOffset.X, (int)player.Y * 32 + (int)boardOffset.Y, 32, 32), 
                Color.White);
            // Draw red key
            spriteBatch.Draw((inv.redKeys > 0) ? redKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32, (int)boardOffset.X + 50, 32, 32),
                Color.White);
            spriteBatch.Draw((inv.redKeys > 1) ? redKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32, (int)boardOffset.X + 82, 32, 32),
                Color.White);
            // Draw blue key
            spriteBatch.Draw((inv.blueKeys > 0) ? blueKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + 32, (int)boardOffset.X + 50, 32, 32),
                Color.White);
            spriteBatch.Draw((inv.blueKeys > 1) ? blueKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + 32, (int)boardOffset.X + 82, 32, 32),
                Color.White);
            // Draw green key
            spriteBatch.Draw((inv.greenKeys > 0) ? greenKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + (2 * 32), (int)boardOffset.X + 50, 32, 32),
                Color.White);
            spriteBatch.Draw((inv.greenKeys > 1) ? greenKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + (2*32), (int)boardOffset.X + 82, 32, 32),
                Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
