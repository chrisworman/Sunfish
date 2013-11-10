using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sunfish.Views
{
	public class ParticleSystem : View
	{


		private class Particle : Sprite
		{

			public bool IsAlive;

			public delegate void OnDieDelegate (Particle deadParticle);

			public OnDieDelegate OnDie;

			private Effects.Disappear DisappearEffect;

			private Effects.Scale ScaleEffect;

			public static void Recycle(Particle deadParticle, Texture2D particleTexture, Vector2 position, Constants.ViewLayer layer, Vector2 velocity, Vector2 acceleration, double lifeSpan, bool shouldShrink)
			{

				deadParticle.Texture = particleTexture;
				deadParticle.Position = position;
				deadParticle.Layer = layer;
				deadParticle.Velocity = velocity;
				deadParticle.Acceleration = acceleration;

				deadParticle.ClearEffects ();

				deadParticle.DisappearEffect.Reset (lifeSpan);
				deadParticle.StartEffect (deadParticle.DisappearEffect);

				if (shouldShrink) {
					deadParticle.ScaleEffect.Reset (1f, 0f, lifeSpan);
					deadParticle.StartEffect (deadParticle.ScaleEffect);
				}

				deadParticle.Visible = true;

			}

			public Particle (Texture2D particleTexture, Vector2 position, Constants.ViewLayer layer, Vector2 velocity, Vector2 acceleration, double lifeSpan, bool shouldShrink) :
				base (particleTexture, position, layer)
			{

				//CenterOrigin ();
				Velocity = velocity;
				Acceleration = acceleration;
				//StartEffect (new Effects.Motion (velocity, acceleration, lifeSpan));

				DisappearEffect = new Effects.Disappear (lifeSpan);
				DisappearEffect.OnComplete = HandleDisappear;
				StartEffect (DisappearEffect);

				ScaleEffect = new Effects.Scale (1f, 0f, lifeSpan);
				if (shouldShrink) {
					StartEffect (ScaleEffect);
				}

			}

			private void HandleDisappear(Effects.Effect disappearEffect)
			{
				if (OnDie != null) {
					OnDie (this);
				}
			}

		}

		public Vector2 ParticleVelocity;

		public bool ShouldRandomizeParticleVelocity;

		public bool ShouldRandomizeParticleVelocityDirectionX;

		public bool ShouldRandomizeParticleVelocityDirectionY;

		public double ParticleLifeSpanMilliseconds;

		public bool ShouldRandomizeParticleLifeSpan;

		public Vector2 ParticleAcceleration;

		public double ParticleBirthDelayMilliseconds;

		public bool ShouldRandomizeParticleBirthRate;

		public bool ShouldShrink;

		//public bool ShouldTrail;

		//public delegate void OnStoppedDelegate (ParticleSystem particleSystemThatIsComplete);

		//public OnStoppedDelegate OnStopped;

		//private bool StopRequested;

		private List<Particle> Particles;

		private Stack<Particle> DeadParticles;

		private Texture2D[] ParticleTextures;

		//private ScreenLayers Trails;

		private double NextParticleBirthTimeMilliseconds;

		private static Random RandomGenerator = new Random ();

		public ParticleSystem (Vector2 position, Constants.ViewLayer layer, params Texture2D[] particleTextures) :
			base (position, layer)
		{
			ParticleTextures = particleTextures;
			InitializeAndSetDefaults ();
		}

		private void InitializeAndSetDefaults()
		{
			Particles = new List<Particle> ();
			DeadParticles = new Stack<Particle> ();
			//Trails = new ScreenLayers ();
			ParticleBirthDelayMilliseconds = 50d;
			ParticleVelocity = new Vector2 (2.5f, 2.5f);
			ShouldRandomizeParticleVelocity = true;
			ShouldRandomizeParticleVelocityDirectionX = true;
			ShouldRandomizeParticleVelocityDirectionY = true;
			ParticleLifeSpanMilliseconds = 3000d;
			ShouldRandomizeParticleLifeSpan = true;
			ParticleAcceleration = new Vector2 (0, 0.05f);
			NextParticleBirthTimeMilliseconds = -1.0d;
			ShouldRandomizeParticleBirthRate = true;
			//StopRequested = false;
			ShouldShrink = true;
			//ShouldTrail = false;
		}

		public override void Update (GameTime gameTime)
		{

			base.Update (gameTime);

			//Particles.Update (gameTime);
			//Particles.RemoveNonVisibleViews ();

			//Trails.Update (gameTime);
			//Trails.RemoveNonVisibleViews ();

			//if (StopRequested) {
			//	if (Particles.Count == 0 && OnStopped != null) {
			//		OnStopped (this);
			//	}
			//} else {
				SpawnParticleIfNecessary (gameTime);
//				if (ShouldTrail) {
//					UpdateTrails ();
//				}
			//}

		}

		public override void Draw (GameTime gameTime, GraphicsDeviceManager graphics)
		{
//			//Trails.Draw (gameTime, graphics);
//			Particles.Draw (gameTime, graphics);
		}

//		public void Stop()
//		{
//			StopRequested = true;
//		}

		private void SpawnParticleIfNecessary(GameTime gameTime)
		{

			// Initialize NextParticleBirthTimeMilliseconds if necessary
			if (NextParticleBirthTimeMilliseconds == -1.0d) {
				NextParticleBirthTimeMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
			}

			// Time to spawn a new particle?
			double currentTimeMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
			if (currentTimeMilliseconds >= NextParticleBirthTimeMilliseconds) {
				CreateNextParticle ();
				NextParticleBirthTimeMilliseconds = GetNextBirthTime (currentTimeMilliseconds);
			}

		}

		private void CreateNextParticle()
		{

			Texture2D particleTexture = GetNextParticleTexture ();
			Vector2 velocity = GetNextParticleVelocity ();
			double lifeSpan = GetNextParticleLifeSpan ();

			Particle nextParticle = RecycleDeadParticle (particleTexture, velocity, ParticleAcceleration, lifeSpan);
			if (nextParticle == null) {
				nextParticle = new Particle (particleTexture, Position, Layer, velocity, ParticleAcceleration, lifeSpan, ShouldShrink);
				nextParticle.OnDie = HandleParticleOnDie;
				Particles.Add (nextParticle);
				SunfishGame.ActiveScreen.AddChildView (nextParticle);
			}
	
		}

		private Particle RecycleDeadParticle(Texture2D particleTexture, Vector2 velocity, Vector2 acceleration, double lifeSpan)
		{
			if (DeadParticles.Count > 0) {
				Particle recycledParticle = DeadParticles.Pop ();
				Particle.Recycle (recycledParticle, particleTexture, Position, Layer, velocity, ParticleAcceleration, lifeSpan, ShouldShrink);
				return recycledParticle;
			} else {
				return null;
			}
		}

		private void HandleParticleOnDie(Particle deadParticle)
		{
			DeadParticles.Push (deadParticle);
		}

		private double GetNextBirthTime(double currentTimeMilliseconds)
		{
			if (ShouldRandomizeParticleBirthRate) {
				return currentTimeMilliseconds + ParticleBirthDelayMilliseconds * RandomGenerator.NextDouble ();
			} else {
				return currentTimeMilliseconds + ParticleBirthDelayMilliseconds;
			}
		}

		private double GetNextParticleLifeSpan()
		{
			if (ShouldRandomizeParticleLifeSpan) {
				return ParticleLifeSpanMilliseconds * RandomGenerator.NextDouble ();
			} else {
				return ParticleLifeSpanMilliseconds;
			}
		}

		private Texture2D GetNextParticleTexture()
		{
			if (ParticleTextures.Length == 1) {
				return ParticleTextures [0];
			} else {
				return ParticleTextures [RandomGenerator.Next (0, ParticleTextures.Length)];
			}
		}

		private Vector2 GetNextParticleVelocity()
		{
			if (ShouldRandomizeParticleVelocity) {
				float randomX = ParticleVelocity.X * (float)RandomGenerator.NextDouble ();
				float randomY = ParticleVelocity.Y * (float)RandomGenerator.NextDouble ();
				if (ShouldRandomizeParticleVelocityDirectionX &&  RandomGenerator.NextDouble () > 0.5d) {
					randomX = -randomX;
				}
				if (ShouldRandomizeParticleVelocityDirectionY && RandomGenerator.NextDouble () > 0.5d) {
					randomY = -randomY;
				}
				return new Vector2 (randomX, randomY);
			} else {
				return ParticleVelocity;
			}
		}

		private void UpdateTrails() {
//			for (int layer = 0; layer < (int) Constants.ViewLayer.Modal+1; layer++) {
//				List<View> viewLayer = Particles.GetLayer ((Constants.ViewLayer)layer);
//				foreach (View view in viewLayer) {
//					Sprite particle = (Sprite) view;
//					Sprite trail = new Sprite (particle.Texture, particle.Position, particle.Layer);
//					trail.Scale = particle.Scale;
//					trail.Origin = particle.Origin;
//					trail.RotationRadians = particle.RotationRadians;
//					trail.OverlayColor = particle.OverlayColor;
//					trail.StartEffect (new Effects.Disappear (200d));
//					Trails.Add (trail);
//				}
//			}
		}

	}
}

