﻿using System;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using BaldurSuchtFiona.Models;
using System.Collections.Generic;
using System.Linq;
using BaldurSuchtFiona.Interfaces;
using BaldurSuchtFiona.Screens;

namespace BaldurSuchtFiona.Components
{
	public class SimulationComponent : GameComponent
	{
		private Game1 game;

        private float gap = 0.00001f;
		public Vector2 Position {
			get;
			private set;
		}

		public Vector2 velocity {
			get;
			private set;
		}
			

		public SimulationComponent (Game1 game) : base(game)
		{
			this.game = game;
            this.game.StartGame();
		}

		public override void Update (GameTime gameTime)
        {
            game.GameTime += gameTime.ElapsedGameTime;

            if (this.game.Input.Heal && this.game.Baldur.CurrentHitpoints > 0 && this.game.Baldur.Potions > 0)
            {
                this.game.Baldur.Potions--;
                this.game.Baldur.CurrentHitpoints = this.game.Baldur.MaxHitpoints;
            }

            if (this.game.Baldur.IsDead)
            {
                this.game.Respawn();
            }

            List<Action> transfers = new List<Action>();
            if (game.Input.Handled)
                return;

            var teleport = false;

            var area = game.World.Area;
            foreach (var character in area.Objects.OfType<Character>())
            {
                if (!game.AllowTeleport)
                {
                    var teleporterDistance = game.World.Area.GetTeleportPosition() - game.Baldur.Position;
                    if (teleporterDistance.Length() > 2)
                    {
                        game.AllowTeleport = true;
                    }
                        
                }

                if (character is IAttackable && (character as IAttackable).CurrentHitpoints <= 0)
                    continue;

                if (character.Ai != null)
                    character.Ai.Update(area, gameTime);

                character.move += character.Velocity;

                IAttacker attacker = null;
                if (character is IAttacker)
                {
                    attacker = (IAttacker)character;
                    attacker.AttackableItems.Clear();

                    attacker.Recovery -= gameTime.ElapsedGameTime;
                    if (attacker.Recovery < TimeSpan.Zero)
                        attacker.Recovery = TimeSpan.Zero;
                }

                foreach (var item in area.Objects)
                {
                    if (item == character)
                        continue;

                    Vector2 distance = (item.Position + item.move) - (character.Position + character.move);

                    if (attacker != null &&
                        item is IAttackable &&
                        distance.Length() - attacker.AttackRange - item.Radius < 0f &&
                        item.GetType() != attacker.GetType())
                    {
                        attacker.AttackableItems.Add(item as IAttackable);
                    }

                    float overlap = item.Radius + character.Radius - distance.Length();
                    if (overlap > 0f)
                    {
                        Vector2 resolution = distance * (overlap / distance.Length());
                        if (!item.IsFixed && character.IsFixed)
                        {
                            item.move += resolution;
                        }
                        else if (!item.IsFixed && !character.IsFixed || item.IsFixed && character.IsFixed)
                        {
                            character.move -= resolution;
                            if (character is Enemy)
                            {
                                if ((character as Enemy).Ai != null)
                                    (character as Enemy).Ai.StopWalking();
                            }
                            else
                            {
                                float totalMass = item.Mass + character.Mass;
                                character.move -= resolution * (item.Mass / totalMass);
                                item.move += resolution * (character.Mass / totalMass);
                            }
                        }

                        if (item is ICollectable && character is ICollector)
                        {
                            transfers.Add(() =>
                                {
                                    area.Objects.Remove(item);
                                    (character as ICollector).Inventory.Add(item as Item);
                                    (item as ICollectable).OnCollect(game.World);
                                    item.Position = Vector2.Zero;
                                });

                            if (item is Weapon && character is Baldur)   //WIESO FUNKTIONIERT DAS NICHT?!?! //todo verbessern
                            {
                                transfers.Add(() =>
                                    {
                                        (character as Baldur).ChangeAttackTexture();
                                    });
                            }
                        }

                        if (character is Enemy && item is ICollectable)
                        {
                            if ((character as Enemy).Ai != null)
                                (character as Enemy).CheckCollectableInteraction(item);
                        }
                    }


                }
            }

            foreach (var item in area.Objects)
            {
                var hadCollision = false;
                bool collision = false;
                int loops = 0;
                if (item.Update != null)
                    item.Update(game, area, item, gameTime);
                do
                {
                    Vector2 position = item.Position + item.move;
                    int minCellX = (int)(position.X - item.Radius);
                    int maxCellX = (int)(position.X + item.Radius);
                    int minCellY = (int)(position.Y - item.Radius);
                    int maxCellY = (int)(position.Y + item.Radius);

                    collision = false;
                    float minImpact = 2f;
                    int minAxis = 0;


                    for (int x = minCellX; x <= maxCellX; x++)
                    {
                        for (int y = minCellY; y <= maxCellY; y++)
                        {
                            
                            if(item is Baldur){
                                if(area.IsInteractable(x, y)){
                                    if(area.IsPotionStation(x, y)){
                                        int flowers1 = 0;
                                        int flowers2 = 0;
                                        int flowers3 = 0;
                                        foreach (var itemIntern in game.Baldur.Inventory) {
                                            if (!(itemIntern is Flower))
                                                continue;
                                            if ((itemIntern as Flower).Value == 1) { flowers1 += 1; }
                                            if ((itemIntern as Flower).Value == 2) { flowers2 += 1; }
                                            if ((itemIntern as Flower).Value == 3) { flowers3 += 1; }
                                        }
                                        if (flowers1 > 4 || flowers2 > 1 || flowers3 > 1) {
                                            game.Screen.ShowScreen (new PotionScreen (game.Screen));
                                        } else {
                                            game.Screen.ShowScreen (new NoPotionScreen (game.Screen));
                                        }
                                    }
                                    if(area.IsWorkbench(x, y)){
                                        //todo: PotionScreens
                                        throw new NotImplementedException("Workbench Screen muss hier eingebaut werden");
                                    }
                                    if(area.IsBed(x, y)){
                                        //todo: PotionScreens
                                        throw new NotImplementedException("Bed Screen muss hier eingebaut werden");
                                    }
                                }

                                if (area.IsTeleporter(x, y) && game.AllowTeleport)
                                    teleport = true;
                            }
                            if (!area.IsCellBlocked(x, y))
                                continue;

                            if (position.X - item.Radius > x + 1 ||
                                position.X + item.Radius < x ||
                                position.Y - item.Radius > y + 1 ||
                                position.Y + item.Radius < y)
                                continue;

                            collision = true;

                            float diffX = float.MaxValue;
                            if (item.move.X > 0)
                                diffX = position.X + item.Radius - x + gap;
                            if (item.move.X < 0)
                                diffX = position.X - item.Radius - (x + 1) - gap;
                            float impactX = 1f - (diffX / item.move.X);

                            float diffY = float.MaxValue;
                            if (item.move.Y > 0)
                                diffY = position.Y + item.Radius - y + gap;
                            if (item.move.Y < 0)
                                diffY = position.Y - item.Radius - (y + 1) - gap;
                            float impactY = 1f - (diffY / item.move.Y);

                            int axis = 0;
                            float impact = 0;
                            if (impactX > impactY)
                            {
                                axis = 1;
                                impact = impactX;
                            }
                            else if(impactX < impactY)
                            {
                                axis = 2;
                                impact = impactY;
                            }

                            if (impact < minImpact)
                            {
                                minImpact = impact;
                                minAxis = axis;
                            }
                        }
                    }

                    if (collision)
                    {
                        hadCollision = true;
                        if (minAxis == 1)
                            item.move *= new Vector2(minImpact, 1f);

                        if (minAxis == 2)
                            item.move *= new Vector2(1f, minImpact);
                    }
                    loops++;
                }
                while(collision && loops < 2);

                if (item is Enemy && hadCollision)
                {
                    if ((item as Enemy).Ai != null)
                        (item as Enemy).Ai.StopWalking();
                }

                item.Position += item.move;
                item.move = Vector2.Zero;


                if (item is IAttacker )
                {
                    IAttacker attacker = item as IAttacker;
                    if (item is IAttackable)
                    {
                        if (!((item as IAttackable).CurrentHitpoints <= 0))
                        {
                            if (attacker.IsAttacking && attacker.Recovery <= TimeSpan.Zero)
                            {
                                if (attacker is Baldur)
                                {
                                    var baldur = attacker as Baldur;
                                    if (baldur.ContinueAttack)
                                    {
                                        foreach (var attackable in attacker.AttackableItems)
                                        {
                                            attackable.CurrentHitpoints -= attacker.AttackValue;
                                            if(item is Character)
                                                attackable.OnHit(game, item as Character,transfers);
                                        }

                                        attacker.Recovery = attacker.TotalRecovery;                                        
                                    }
                                    else
                                    {
                                        baldur.IsAttacking = false;
                                    }
                                }
                                else
                                {
                                    foreach (var attackable in attacker.AttackableItems)
                                    {
                                        attackable.CurrentHitpoints -= attacker.AttackValue;
                                        if(item is Character)
                                            attackable.OnHit(game, item as Character,transfers);
                                    }

                                    attacker.Recovery = attacker.TotalRecovery;
                                }
                            }
                        }
                    }
                }
            }



            foreach (var transfer in transfers)
                transfer();

            if (teleport)
            {
                game.Screen.ShowScreen (new KeycardScreen (game.Screen));
//                switch(game.World.Area.Name){
//                    case "base":
//                        switch (game.Baldur.KeycardCounter)
//                        {
//                            case 1:
//                                game.LoadLevel(1);
//                                break;
//                            case 2:
//                                game.LoadLevel(2);
//                                break;
//                            default:
//                                break;
//                        }
//                        break;
//                    case "level1":
//                        game.LoadLevel(0);
//                        break;
//                    case "level2":
//                        game.LoadLevel(0);
//                        break;
//                    default:
//                        break;
//              }
            base.Update(gameTime);
		    }

		
	    }
    }
}