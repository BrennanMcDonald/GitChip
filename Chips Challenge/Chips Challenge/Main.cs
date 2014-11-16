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
        // Constants to deal with tile values
        // TODO: Create a Map Tile struct that has a texture, numerical value and ASCII value
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
        private SpriteBatch _spriteBatch;
        private SpriteFont _menuFont;
        private KeyboardState _prevKeys;
        private Player _player = new Player();

        public Texture2D wallTexture,
            floorTexture,
            redKeyTexture,
            playerTexture,
            redDoorTexture,
            blueKeyTexture,
            blueDoorTexture,
            greenDoorTexture,
            greenKeyTexture,
            backgroundTexture,
            chipTexture,
            portalTexture,
            playerPortalTexture;

        public Vector2 boardOffset = new Vector2(50, 50);
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            wallTexture = Content.Load<Texture2D>("wall");
            floorTexture = Content.Load<Texture2D>("floor");
            portalTexture = Content.Load<Texture2D>("portal");
            playerPortalTexture = Content.Load<Texture2D>("portalPlayer");

            redKeyTexture = Content.Load<Texture2D>("redKey");
            blueKeyTexture = Content.Load<Texture2D>("blueKey");
            greenKeyTexture = Content.Load<Texture2D>("greenKey");

            redDoorTexture = Content.Load<Texture2D>("redDoor");
            blueDoorTexture = Content.Load<Texture2D>("blueDoor");
            greenDoorTexture = Content.Load<Texture2D>("greenDoor");

            playerTexture = Content.Load<Texture2D>("player");
            backgroundTexture = Content.Load<Texture2D>("bg");
            chipTexture = Content.Load<Texture2D>("chip");

            _menuFont = Content.Load<SpriteFont>("Font1");
            _player.inventory = new Inventory();
            // Add leves to the Level Queue
            levelList.Enqueue(new TileMap("level.txt"));
            levelList.Enqueue(new TileMap("level2.txt"));
            levelList.Enqueue(new TileMap("level3.txt"));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // If the game is on its first run dequeue the first map and set the start position
            // Reason this isnt grouped with the rest is because 
            // I was having troubles with a method of only dequeueing one map
            // I could set the first start character a portal and just
            // have it work with every portal after
            if (firstRun)
            {
                map = levelList.Dequeue();
                firstRun = false;
                _player.Position = map.startPos;
            };
            // TODO: Clean up key handling
            KeyboardState ks = Keyboard.GetState();
            if (ks != _prevKeys)
            {
                if (ks.IsKeyDown(Keys.Right))
                {
                    if ((int)_player.Position.X + 1 < map.MapWidth)
                    {
                        switch (map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X + 1].TileID)
                        {
                            case redDoor:
                                if (_player.inventory.redKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X + 1].TileID = floor;
                                    _player.inventory.redKeys--;
                                }
                                break;
                            case greenDoor:
                                if (_player.inventory.greenKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X + 1].TileID = floor;
                                    _player.inventory.greenKeys--;
                                }
                                break;
                            case blueDoor:
                                if (_player.inventory.blueKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X + 1].TileID = floor;
                                    _player.inventory.blueKeys--;
                                }
                                break;
                            case goldDoor:
                                if (_player.inventory.goldKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X + 1].TileID = floor;
                                    _player.inventory.goldKeys--;
                                }
                                break;
                        }
                        if (map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X + 1].TileID < wall)
                            _player.Position.X += 1;
                    }
                }
                else if (ks.IsKeyDown(Keys.Left))
                {
                    if (_player.Position.X > 0)
                    {
                        switch (map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X - 1].TileID)
                        {
                            case redDoor:
                                if (_player.inventory.redKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X - 1].TileID = floor;
                                    _player.inventory.redKeys--;
                                }
                                break;
                            case greenDoor:
                                if (_player.inventory.greenKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X - 1].TileID = floor;
                                    _player.inventory.greenKeys--;
                                }
                                break;
                            case blueDoor:
                                if (_player.inventory.blueKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X - 1].TileID = floor;
                                    _player.inventory.blueKeys--;
                                }
                                break;
                            case goldDoor:
                                if (_player.inventory.goldKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X - 1].TileID = floor;
                                    _player.inventory.goldKeys--;
                                }
                                break;
                        }
                        if (map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X - 1].TileID < wall)
                            _player.Position.X -= 1;
                    }
                }
                else if (ks.IsKeyDown(Keys.Down))
                {
                    if (_player.Position.Y + 1 < map.MapHeight)
                    {
                        switch (map.Rows[(int)_player.Position.Y + 1].Columns[(int)_player.Position.X].TileID)
                        {
                            case redDoor:
                                if (_player.inventory.redKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y + 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.redKeys--;
                                }
                                break;
                            case greenDoor:
                                if (_player.inventory.greenKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y + 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.greenKeys--;
                                }
                                break;
                            case blueDoor:
                                if (_player.inventory.blueKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y + 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.blueKeys--;
                                }
                                break;
                            case goldDoor:
                                if (_player.inventory.goldKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y + 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.goldKeys--;
                                }
                                break;
                        }
                        if (map.Rows[(int)_player.Position.Y + 1].Columns[(int)_player.Position.X].TileID < wall)
                            _player.Position.Y += 1;
                    }
                }
                else if (ks.IsKeyDown(Keys.Up))
                {
                    if (_player.Position.Y > 0)
                    {
                        switch (map.Rows[(int)_player.Position.Y - 1].Columns[(int)_player.Position.X].TileID)
                        {
                            case redDoor:
                                if (_player.inventory.redKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y - 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.redKeys--;
                                }
                                break;
                            case greenDoor:
                                if (_player.inventory.greenKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y - 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.greenKeys--;
                                }
                                break;
                            case blueDoor:
                                if (_player.inventory.blueKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y - 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.blueKeys--;
                                }
                                break;
                            case goldDoor:
                                if (_player.inventory.goldKeys > 0)
                                {
                                    map.Rows[(int)_player.Position.Y - 1].Columns[(int)_player.Position.X].TileID = floor;
                                    _player.inventory.goldKeys--;
                                }
                                break;


                        }
                        if (map.Rows[(int)_player.Position.Y - 1].Columns[(int)_player.Position.X].TileID < wall)
                            _player.Position.Y -= 1;
                    }
                }
            }
            // Add keys to inventory and handle map control.
            switch (map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X].TileID)
            {
                case redKey:
                    _player.inventory.redKeys++;
                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X].TileID = floor;
                    break;
                case blueKey:
                    _player.inventory.blueKeys++;
                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X].TileID = floor;
                    break;
                case greenKey:
                    _player.inventory.greenKeys++;
                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X].TileID = floor;
                    break;
                case portal:
                    if (levelList.Count > 0 && _player.inventory.chips >= map.chipCount)
                    {
                        map = levelList.Dequeue();
                        _player.Position = map.startPos;
                        _player.inventory = new Inventory();
                    }
                    else if (levelList.Count <= 0)
                        this.Exit();
                    break;
                case chip:
                    _player.inventory.chips++;
                    map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X].TileID = floor;
                    break;

            }
            // Watch keystates so the user cant hold down keys
            _prevKeys = ks;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            _spriteBatch.Begin();
            // Draw Background
            _spriteBatch.Draw(floorTexture,
                new Rectangle((int)boardOffset.X - 6, (int)boardOffset.Y - 6, map.MapWidth * 32 + 18, map.MapHeight * 32 + 18),
                Color.White);
            // Draw board in a 2 deep Height,Width for loop
            for (int y = 0; y < map.MapHeight; y++)
            {
                for (int x = 0; x < map.MapWidth; x++)
                {
                    if (map.Rows[y].Columns[x].TileID == wall)
                        _spriteBatch.Draw(wallTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Wall
                    else if (map.Rows[y].Columns[x].TileID == floor)
                        _spriteBatch.Draw(floorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Floor
                    else if (map.Rows[y].Columns[x].TileID == redKey)
                        _spriteBatch.Draw(redKeyTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Red key
                    else if (map.Rows[y].Columns[x].TileID == blueKey)
                        _spriteBatch.Draw(blueKeyTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Blue key
                    else if (map.Rows[y].Columns[x].TileID == greenKey)
                        _spriteBatch.Draw(greenKeyTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Green key
                    else if (map.Rows[y].Columns[x].TileID == redDoor)
                        _spriteBatch.Draw(redDoorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Red Door
                    else if (map.Rows[y].Columns[x].TileID == blueDoor)
                        _spriteBatch.Draw(blueDoorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Blue Door
                    else if (map.Rows[y].Columns[x].TileID == greenDoor)
                        _spriteBatch.Draw(greenDoorTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Green Door
                    else if (map.Rows[y].Columns[x].TileID == chip)
                        _spriteBatch.Draw(chipTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Chip
                    else if (map.Rows[y].Columns[x].TileID == portal)
                        _spriteBatch.Draw(portalTexture,
                            new Rectangle(x * 32 + (int)boardOffset.X, y * 32 + (int)boardOffset.Y, 32, 32),
                            Color.White); // Portal
                }
            }
            // Draw text for score
            _spriteBatch.DrawString(_menuFont,
                string.Format("Chips: {0}", _player.inventory.chips),
                new Vector2((2 * (int)boardOffset.Y) + map.MapWidth * 32, (int)boardOffset.X),
                Color.Black);
            // Draw Player
            _spriteBatch.Draw(map.Rows[(int)_player.Position.Y].Columns[(int)_player.Position.X].TileID != portal ? playerTexture : playerPortalTexture,
                new Rectangle((int)_player.Position.X * 32 + (int)boardOffset.X, (int)_player.Position.Y * 32 + (int)boardOffset.Y, 32, 32),
                Color.White);
            // Draw red key
            _spriteBatch.Draw((_player.inventory.redKeys > 0) ? redKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32, (int)boardOffset.X + 50, 32, 32),
                Color.White);
            _spriteBatch.Draw((_player.inventory.redKeys > 1) ? redKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32, (int)boardOffset.X + 82, 32, 32),
                Color.White);
            // Draw blue key
            _spriteBatch.Draw((_player.inventory.blueKeys > 0) ? blueKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + 32, (int)boardOffset.X + 50, 32, 32),
                Color.White);
            _spriteBatch.Draw((_player.inventory.blueKeys > 1) ? blueKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + 32, (int)boardOffset.X + 82, 32, 32),
                Color.White);
            // Draw green key
            _spriteBatch.Draw((_player.inventory.greenKeys > 0) ? greenKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + (2 * 32), (int)boardOffset.X + 50, 32, 32),
                Color.White);
            _spriteBatch.Draw((_player.inventory.greenKeys > 1) ? greenKeyTexture : floorTexture,
                new Rectangle((2 * (int)boardOffset.Y) + map.MapWidth * 32 + (2 * 32), (int)boardOffset.X + 82, 32, 32),
                Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
