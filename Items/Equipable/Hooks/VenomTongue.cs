using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Equipable.Hooks
{
    class VenomTongue : ModItem
    {
        public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Lunges you at extremely high speeds instead of staying grappled" +
				"\nHit a tile that you're touching to launch yourself away from it");
        }

        public override void SetDefaults()
		{
			item.noUseGraphic = true;
			item.damage = 0;
			item.knockBack = 7f;
			item.useStyle = 5;
			item.shootSpeed = 30f;
			item.width = 20;
			item.height = 26;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 5;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.shoot = ModContent.ProjectileType<VenomTongueProj>();
		}
    }

	class VenomTongueProj : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/TerrorRain/VenomTongueTip";

        public override void SetDefaults()
        {
			projectile.netImportant = true;
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 7;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 10000;
		}

		//public override bool? CanUseGrapple(Player player)
		//{
		//	int hooksOut = 0;
		//	for (int l = 0; l < 1000; l++)
		//	{
		//		if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
		//		{
		//			hooksOut++;
		//		}
		//	}
		//	if (hooksOut > 0)
		//	{
		//		return false;
		//	}
		//	return true;
		//}

        public override void PostAI()
        {
            base.PostAI();
			if (projectile.velocity == Vector2.Zero)
			{
				projectile.active = false;
			}
		}

        public override bool? SingleGrappleHook(Player player)
        {
            return true;
		}
		public override float GrappleRange()
		{
			return 500f;
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 1;
		}

		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 30f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 20f;
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 18;
			height = 18;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void GrappleTargetPoint(Player player, ref float grappleX, ref float grappleY)
		{
			Vector2 dirToPlayer = projectile.DirectionTo(player.Center);
			float hangDist = 50f;
			grappleX += dirToPlayer.X * hangDist;
			grappleY += dirToPlayer.Y * hangDist;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 originPoint = Main.player[projectile.owner].Center;
			Vector2 center = projectile.Center;
			Vector2 distToProj = originPoint - projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();
			Texture2D texture = ModContent.GetTexture("TerrorbornMod/NPCs/TerrorRain/VenomTongueSegment");

			while (distance > texture.Height && !float.IsNaN(distance))
			{
				distToProj.Normalize();
				distToProj *= texture.Height;
				center += distToProj;
				distToProj = originPoint - center;
				distance = distToProj.Length();


				//Draw chain
				spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
					new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
					new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
			}

			Texture2D texture2 = Main.projectileTexture[projectile.type];
			Vector2 position = projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
			return false;
		}
	}
}
