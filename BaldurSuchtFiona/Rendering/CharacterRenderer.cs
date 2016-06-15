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
        private int _deathAnimationTime = 250;
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

        public CharacterRenderer(Character character, Camera camera, Texture2D texture, Texture2D attackTexture)
            : base (character, camera, texture,attackTexture, new Point(32, 32), 25, new Point(16, 26), 1.25f)
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
            if (character == null) return;
           // nächste Animation ermitteln
            var renderAttack = false;
            Animation nextAnimation = Animation.Idle;
            if (character.Velocity.Length() > 0f)
            {
                nextAnimation = Animation.Walk;

                var directionXLength = character.Velocity.X >= 0 ? character.Velocity.X : character.Velocity.X * -1;
                var directionYLength = character.Velocity.Y >= 0 ? character.Velocity.Y : character.Velocity.Y * -1;
                // -> Spieler bewegt sich -> Ausrichtung ermitteln
                if (directionXLength > directionYLength)
                {
                    if (character.Velocity.X >= 0)
                    {
                        direction = Direction.East;
                    }
                    else
                    {
                        direction = Direction.West;
                    }
                }
                else
                {
                    if (character.Velocity.Y >= 0)
                    {
                        direction = Direction.South;
                    }
                    else
                    {
                        direction = Direction.North;
                    }
                }
            }

            if (character is IAttackable && (character as IAttackable).CurrentHitpoints <= 0)
            {
                nextAnimation = Animation.Die;
                direction = Direction.North;
            }

            if (character is IAttacker && nextAnimation != Animation.Die)
            {
                var attacker = character as IAttacker;
                if (attacker.IsAttacking)
                     nextAnimation = Animation.Hit;
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
                        animationRow = 1;
                        break;
                    case Animation.Die:
                        frameCount = 4;
                        animationRow = 1;
                        animationRow = 4;
                        break;
                    case Animation.Idle:
                        frameCount = 4;
                        animationRow = 1;
                        break;
                    case Animation.Hit:
                        frameCount = 4;
                        animationRow = 1;
                        break;
                }
            }

            // Animationszeile ermitteln
            int row = animationRow + (int)direction;

            // Frame ermitteln
            int frame = 0; // Idle
            int attackFrame = 0; 
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
                    if (character is IAttacker)
                    {
                        if (character.Velocity.Length() > 0f)
                        {
                            float animationSpeed = (float)0.20;                  
                            AnimationTime += (int)(gameTime.ElapsedGameTime.TotalMilliseconds * animationSpeed);
                            frame = (AnimationTime / FrameTime) % frameCount;
                        }
                        var attacker = (character as IAttacker);
                        double animationPosition = 1d - (attacker.Recovery.TotalMilliseconds / attacker.TotalRecovery.TotalMilliseconds);
                        attackFrame = (int)(frameCount * animationPosition);
                        renderAttack = true;
                    }
                    break;
                case Animation.Die:
                    row = animationRow;          
                    AnimationTime += (int)(gameTime.ElapsedGameTime.TotalMilliseconds * (float)0.20);
                    frame = AnimationTime < _deathAnimationTime ? (int)((AnimationTime / (_deathAnimationTime*1.0)) * frameCount) : frameCount - 1;
                    if (character is Baldur && AnimationTime >= _deathAnimationTime )
                    {
                        (character as Baldur).IsDead = true;
                    }
                    break;
            }

            // Position des CharacterMittelpunktes in ViewKoordinaten
            int posX = (int)((Objekt.Position.X) * Camera.Scale) - offset.X;
            int posY = (int)((Objekt.Position.Y) * Camera.Scale) - offset.Y;
            int radius = (int)(Objekt.Radius * Camera.Scale);

            // Debug Frame
            //spriteBatch.Draw(pix, new Rectangle(posX, posY, 2, 2), Color.Red);
            //spriteBatch.Draw(pix, new Rectangle(posX - radius, posY - radius, radius + radius, radius + radius), Color.Red);
           
            var sizeScale = (character is Boss) ? 1.5f : 1;

            Vector2 scale = new Vector2(Camera.Scale / FrameSize.X, Camera.Scale / FrameSize.Y) * FrameScale;

            Rectangle sourceRectangle = new Rectangle(
                                            frame * FrameSize.X,
                                            row * FrameSize.Y,
                                            FrameSize.X,
                                            FrameSize.Y
                                        );

            Rectangle destinationRectangle = new Rectangle(
              posX - (int)(ObjektOffset.X * scale.X) + 2, posY - (int)(ObjektOffset.Y * scale.Y),
                (int)(FrameSize.X * scale.X * sizeScale), (int)(FrameSize.Y * scale.Y * sizeScale)
            );
            if (renderAttack && direction == Direction.North)
            {
                posY -= 1;
                Rectangle attackSourceRectangle = new Rectangle(
                    attackFrame * FrameSize.X,
                    row * FrameSize.Y,
                    FrameSize.X,
                    FrameSize.Y
                );
                Rectangle attackDestinationRectangle = new Rectangle(
                    posX - (int)(ObjektOffset.X * scale.X) + 2, posY - (int)(ObjektOffset.Y * scale.Y),
                    (int)(FrameSize.X * scale.X * sizeScale), (int)(FrameSize.Y * scale.Y * sizeScale)
                );
                spriteBatch.Draw(AttackTexture, attackDestinationRectangle, attackSourceRectangle, Color.White);
                posY += 2;
            }

            if (renderAttack && direction != Direction.North)
                
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

                var posX2 = posX;
                var posY2 = posY;

                switch (direction)
                {
                    case Direction.East:
                        posX2 += 1;
                        break;
                    case Direction.South:
                        posY2 += 1;
                        break;
                    case Direction.West:
                        posX2 -= 1;
                        break;
                }
                Rectangle attackSourceRectangle = new Rectangle(
                    attackFrame * FrameSize.X,
                    row * FrameSize.Y,
                    FrameSize.X,
                    FrameSize.Y
                );
                Rectangle attackDestinationRectangle = new Rectangle(
                    posX2 - (int)(ObjektOffset.X * scale.X) + 2, posY2 - (int)(ObjektOffset.Y * scale.Y),
                    (int)(FrameSize.X * scale.X * sizeScale), (int)(FrameSize.Y * scale.Y * sizeScale)
                );
                if((direction == Direction.West && attackFrame <= 1) || (direction == Direction.East && attackFrame >= 2)){
                    spriteBatch.Draw(AttackTexture, attackDestinationRectangle, attackSourceRectangle, Color.White);
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
                }else{
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
                    spriteBatch.Draw(AttackTexture, attackDestinationRectangle, attackSourceRectangle, Color.White);
                }
            }else
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
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