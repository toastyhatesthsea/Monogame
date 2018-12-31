using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprites
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D rings, skull, pig;
        Point ringFrameSize, ringCurrentFrame, ringSheetSize, skullFrameSize, skullCurrentFrame, skullSheetSize;
        Point pigFrameSize, pigCurrentFrame, pigSheetSize;
        Vector2 ringsPosition, skullPosition, pigposition;
        MouseState prevMouseState;
        float ringsSpeed = 6;
        float skullSpeed = 7;
        float pigSpeed = 4;
        const int collisionRectOffset = 10;

        private int timeSinceLastFrame, milliSecondsPerFrame;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ringsPosition = new Vector2(0, 0);
            skullPosition = new Vector2(100, 100);
            pigposition = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            timeSinceLastFrame = 0;
            milliSecondsPerFrame = 500;

            //TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 16);
          
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected bool Collide()
        {
            Rectangle ringsRect = new Rectangle((int)ringsPosition.X + collisionRectOffset,
                (int)ringsPosition.Y + collisionRectOffset, ringFrameSize.X - collisionRectOffset * 2, ringFrameSize.Y - collisionRectOffset * 2);
            Rectangle skullRect = new Rectangle((int)skullPosition.X + collisionRectOffset,
                (int)skullPosition.Y + collisionRectOffset, skullFrameSize.X - collisionRectOffset * 2, skullFrameSize.Y - collisionRectOffset * 2);
            return ringsRect.Intersects(skullRect);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            skull = Content.Load<Texture2D>("Images/skullball");
            skullCurrentFrame = new Point(0, 0);
            skullFrameSize = new Point(skull.Width / 6, skull.Height / 8);
            skullSheetSize = new Point(6, 8);

            rings = Content.Load<Texture2D>("Images/threerings");
            ringCurrentFrame = new Point(0, 0);
            ringFrameSize = new Point(rings.Width / 6, rings.Height / 8);
            ringSheetSize = new Point(6, 8);

            pig = Content.Load<Texture2D>("Images/guineapigs");
            pigCurrentFrame = new Point(0, 0);
            pigFrameSize = new Point(pig.Width / 12, pig.Height / 8);
            pigSheetSize = new Point(3, 0);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            ++ringCurrentFrame.X;
            if (ringCurrentFrame.X >= ringSheetSize.X)
            {
                ringCurrentFrame.X = 0;
                ++ringCurrentFrame.Y;
                if (ringCurrentFrame.Y >= ringSheetSize.Y)
                    ringCurrentFrame.Y = 0;
            }

            ++skullCurrentFrame.X;
            if (skullCurrentFrame.X >= skullSheetSize.X)
            {
                skullCurrentFrame.X = 0;
                ++skullCurrentFrame.Y;
                if (skullCurrentFrame.Y >= skullSheetSize.Y)
                {
                    skullCurrentFrame.Y = 0;
                }
            }

            timeSinceLastFrame = timeSinceLastFrame + gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > milliSecondsPerFrame)
            {
                timeSinceLastFrame = timeSinceLastFrame - milliSecondsPerFrame;
                pigCurrentFrame.X++;
                if (pigCurrentFrame.X >= pigSheetSize.X)
                {
                    pigCurrentFrame.X = 0;
                    pigCurrentFrame.Y++;
                    if (pigCurrentFrame.Y >= pigSheetSize.Y)
                    {
                        pigCurrentFrame.Y = 0;
                    }
                }
            }

            skullPosition.X = skullPosition.X + skullSpeed;
            if (skullPosition.X > Window.ClientBounds.Width)
            {
                skullSpeed = skullSpeed * -1;
            }
            else if (skullPosition.X < 0)
            {
                skullSpeed = skullSpeed * -1;
            }

            ringsPosition.X = ringsPosition.X + ringsSpeed;
            if(ringsPosition.X > Window.ClientBounds.Width)
            {
                ringsSpeed = ringsSpeed * -1;
            }
            else if(ringsPosition.X < 0)
            {
                ringsSpeed = ringsSpeed * -1;
            }

            KeyboardState keyboardState = Keyboard.GetState();

            if (pigposition.X + pigFrameSize.X / 2 < Window.ClientBounds.Width)
            {
                if (keyboardState.IsKeyDown(Keys.Right))
                    pigposition.X += pigSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
                pigposition.X -= pigSpeed;

            if (keyboardState.IsKeyDown(Keys.Up))
                pigposition.Y -= pigSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                pigposition.Y += pigSpeed;

            /*MouseState mouseState = Mouse.GetState();
            if (mouseState.X != prevMouseState.X ||
                mouseState.Y != prevMouseState.Y)
                ringsPosition = new Vector2(mouseState.X, mouseState.Y);
            prevMouseState = mouseState;*/

            if (Collide())
            {
                Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            spriteBatch.Draw(rings, ringsPosition,
                new Rectangle(ringCurrentFrame.X * ringFrameSize.X,
                    ringCurrentFrame.Y * ringFrameSize.Y,
                    ringFrameSize.X,
                    ringFrameSize.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
            //spriteBatch.End();
            spriteBatch.Draw(skull, skullPosition,
                new Rectangle(skullCurrentFrame.X * skullFrameSize.X,
                    skullCurrentFrame.Y * skullFrameSize.Y,
                    skullFrameSize.X,
                    skullFrameSize.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0);
            spriteBatch.Draw(pig, pigposition,
                 new Rectangle(pigCurrentFrame.X * pigFrameSize.X,
                     pigCurrentFrame.Y * pigFrameSize.Y,
                     pigFrameSize.X,
                     pigFrameSize.Y),
                 Color.White, 0, Vector2.Zero,
                 3, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}