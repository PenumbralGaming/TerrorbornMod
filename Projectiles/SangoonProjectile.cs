using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Projectiles
{
    class SangoonProjectile : ModProjectile // Created by Seraph
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sangoon");
            Main.projFrames[projectile.type] = 5;
        }
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 30;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.timeLeft = 900;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            
        }

        public float AI_State
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public float AI_Timer
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }


        public float TargetIndex
        {
            get => projectile.localAI[1];
            set => projectile.localAI[1] = value;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(TargetIndex);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            TargetIndex = reader.ReadSingle();
        }

        public static readonly int FindingTarget = 0;
        public static readonly int Attacking = 1;
        public static readonly int AttackCooldown = 2;

        public static readonly float MaxEngagementDistance = 1000;
        public static readonly float MaxAttackDistance = 150;
        public void GradualTurnMovement(Vector2 TargetPosition, float MaxSpeed, float TurnResist)
        {
            float speed = MaxSpeed;
            Vector2 move = TargetPosition -projectile.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TurnResist; //the larger this is, the slower the npc will turn
            move = (projectile.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            projectile.velocity = move;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            NPC target;

            

            AI_Timer++;

            if (TargetIndex >= 0)
            {
                target = Main.npc[(int)TargetIndex];
            }
            else
            {
                target = null;
            }

            if (AI_State == FindingTarget)
            {
                float distance;
                NPC possibleTarget;
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    possibleTarget = Main.npc[k];
                    distance = (possibleTarget.Center - player.Center).Length();
                    if (distance < MaxEngagementDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && possibleTarget.chaseable && !possibleTarget.friendly && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        TargetIndex = k;
                        AI_State = Attacking;


                        //maxDistance = (target.Center - projectile.Center).Length();
                    }

                }

                if(Vector2.Distance(projectile.Center, player.Center)> 80f)
                {
                    GradualTurnMovement(player.Center, 5 + (Vector2.Distance(projectile.Center, player.Center) / 75f), 30 / ((Vector2.Distance(projectile.Center, player.Center) / 100f) + 1f));
                }
            }

            if (AI_State == Attacking)
            {
                if (target == null || !target.active || target.dontTakeDamage || !target.chaseable|| Vector2.Distance(target.Center, player.Center)>MaxEngagementDistance)
                {
                    AI_State = FindingTarget;
                }
                float distance = Vector2.Distance(target.Center, projectile.Center);
                if (distance > MaxAttackDistance)
                {
                    GradualTurnMovement(target.Center, 15, 10);
                }
                else
                {
                    Vector2 dash01 = target.Center - projectile.Center;
                    dash01.Normalize();
                    dash01 *= 24f;
                    projectile.velocity = dash01;
                    AI_State = AttackCooldown;
                    AI_Timer = 0;
                }

            }

            if(AI_State == AttackCooldown)
            {
                if (target == null || !target.active || target.dontTakeDamage || !target.chaseable || Vector2.Distance(target.Center, player.Center) > MaxEngagementDistance)
                {
                    AI_State = FindingTarget;
                }
                if (AI_Timer >= 15)
                {
                    AI_State = Attacking;
                }
            }

            #region animation

            projectile.frameCounter++;

            if (projectile.frameCounter > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 4)
                {
                    projectile.frame = 0;
                }
            }

            #endregion


            projectile.direction = (projectile.velocity.X <= 0).ToDirectionInt();
            projectile.spriteDirection = projectile.direction;
        }

    }
}
