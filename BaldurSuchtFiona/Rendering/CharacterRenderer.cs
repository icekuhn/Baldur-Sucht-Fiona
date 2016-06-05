using System;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Rendering
{
    internal class CharacterRenderer : ObjektRenderer
    {
        private Character character;
        private Animation animation;
        private Direction direction;
        private int frameCount;
        private int animationRow;
        private Texture2D pix;




        public CharacterRenderer(Character character, Camera camera, Texture2D texture)
            : base (character, camera, texture, new Point(32, 32), 25, new Point(16, 26), 1.25f)
        {
            this.character = character;
            animation = Animation.Idle;
            direction = Direction.South;
            frameCount = 4;
            animationRow = 1;

            pix = new Texture2D(texture.GraphicsDevice, 1, 1);
            pix.SetData(new [] { Color.White });
        }

        public override void Draw(SpriteBatch spriteBatch, Point offset, GameTime gameTime)
        {
           // nächste Animation ermitteln
            Animation nextAnimation = Animation.Idle;
            if (character.Velocity.Length() > 0f)
            {
                nextAnimation = Animation.Walk;

                // -> Spieler bewegt sich -> Ausrichtung ermitteln
                if (character.Velocity.X > character.Velocity.Y)
                {
                    // Rechts oben
                    if (-character.Velocity.X > character.Velocity.Y)
                    {
                        // Links oben -> Oben
                        direction = Direction.North;
                    }
                    else
                    {
                        // Rechts unten -> Rechts
                        direction = Direction.East;
                    }
                }
                else
                {
                    // Links unten
                    if (-character.Velocity.X > character.Velocity.Y)
                    {
                        // Links oben -> Links
                        direction = Direction.West;
                    }
                    else
                    {
                        // Rechts unten -> Rechts
                        direction = Direction.South;
                    }
                }
                if (character is Farmer)
                {
                    if (character.Velocity.X > character.Velocity.Y)
                    {
                        // Rechts oben
                        if (-character.Velocity.X > character.Velocity.Y)
                        {
                            // Links oben -> Oben
                            direction = Direction.South;
                        }
                        else
                        {
                            // Rechts unten -> Rechts
                            direction = Direction.West;
                        }
                    }
                    else
                    {
                        // Links unten
                        if (-character.Velocity.X > character.Velocity.Y)
                        {
                            // Links oben -> Links
                            direction = Direction.East;
                        }
                        else
                        {
                            // Rechts unten -> Rechts
                            direction = Direction.North;
                        }
                    }
                };

                // TODO: Angriff Animation


                // Sterbe Animation
                if (character is IAttackable && (character as IAttackable).CurrentHitpoints <= 0)
                {
                    nextAnimation = Animation.Die;
                    direction = Direction.North;
                }
            }

            // Animation setzen
            if (animation != nextAnimation)
            {
                animation = nextAnimation;
                AnimationTime = 0;

                switch (animation)
                {
                    case Animation.Walk:
                        frameCount = 4;
                        if (character is Farmer)
                            frameCount = 3;
                        animationRow = 1;
                        break;

                    case Animation.Die:
                        frameCount = 4;
                        animationRow = 0;
                        break;

                    case Animation.Idle:
                        frameCount = 4;
                        animationRow = 1;
                        break;

                    case Animation.Hit:
                        frameCount = 4;
                        animationRow = 0;
                        break;
                }
            }

            // Animationszeile ermitteln
            int row = animationRow + (int)direction;

            if (animation == Animation.Die)
            {
                row = animationRow;
            }

            // Frame ermitteln
            int frame = 0; // Idle
            switch (animation)
            {
                case Animation.Walk:
                    // Animationsgeschwindigkeit <-> Laufgeschwindigkeit
                    //float speed = character.Velocity.Length() / character.MaxSpeed;
                    float speed = (float)0.20;
                    AnimationTime += (int)(gameTime.ElapsedGameTime.TotalMilliseconds * speed);
                    frame = (AnimationTime / FrameTime) % frameCount;
                    break;

                case Animation.Hit:
                    // TODO Angriff Animationsverlauf
                    break;

                case Animation.Die:
                    // TODO Sterbe Animationsvelauf
                    break;
            }

            // Position des CharacterMittelpunktes in ViewKoordinaten
            int posX = (int)((Objekt.Position.X) * Camera.Scale) - offset.X;
            int posY = (int)((Objekt.Position.Y) * Camera.Scale) - offset.Y;
            int radius = (int)(Objekt.Radius * Camera.Scale);

            // Debug Frame
            //spriteBatch.Draw(pix, new Rectangle(posX, posY, 2, 2), Color.Red);
            //spriteBatch.Draw(pix, new Rectangle(posX - radius, posY - radius, radius + radius, radius + radius), Color.Red);
           
            Vector2 scale = new Vector2(Camera.Scale / FrameSize.X, Camera.Scale / FrameSize.Y) * FrameScale;

            Rectangle sourceRectangle = new Rectangle(
                                            frame * FrameSize.X,
                                            row * FrameSize.Y,
                                            FrameSize.X,
                                            FrameSize.Y
                                        );

            if(character is Farmer)
                sourceRectangle = new Rectangle(
                    row * FrameSize.X,
                    frame * FrameSize.Y,
                    FrameSize.X,
                    FrameSize.Y
                );

            Rectangle destinationRectangle = new Rectangle(
              posX - (int)(ObjektOffset.X * scale.X) + 2, posY - (int)(ObjektOffset.Y * scale.Y),
              (int)(FrameSize.X * scale.X), (int)(FrameSize.Y * scale.Y)
            );

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

        }
    }

    // Auflistungen Animationen
    internal enum Animation
    {
        Idle,
        Walk,
        Hit,
        Die
    }

    // Auflistung Blickrichtungen
    internal enum Direction
    {
        North = -1,
        West = 0,
        South = 1,
        East = 2,
    }

}


