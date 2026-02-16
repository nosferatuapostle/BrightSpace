using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles.Data;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using System;

namespace BrightSpace;

public class BomberExplosion : Projectile
{
    public BomberExplosion(ProjectileContext context) : base(context, 0.5f)
    {
        speed = 0f;
        radius = 12f;
        position = context.targetPosition;
    }

    protected override void HitAction(Unit target)
    {
        target.TakeDamage(context.owner, context.owner.GetBaseDamageValue() * 0.2f);

        var effect = ExplosionEffect();
        effect.Trigger();
        World.AddParticleEffect(effect);
    }

    protected override ParticleEffect ExplosionEffect()
    {
        var particleEffect = new ParticleEffect("BomberExplosion")
        {
            Position = position,
            AutoTrigger = false
        };

        var smokeEmitter = new ParticleEmitter(200)
        {
            Name = "Smoke",
            LifeSpan = 1.2f,
            TextureRegion = new Texture2DRegion(Data.Pixel),
            Profile = Profile.Circle(20f, CircleRadiation.Out),
            Parameters = new ParticleReleaseParameters
            {
                Quantity = new ParticleInt32Parameter(5, 10),
                Speed = new ParticleFloatParameter(20f, 40f),
                Color = new ParticleColorParameter(new Vector3(0.2f, 0.2f, 0.2f)),
                Scale = new ParticleVector2Parameter(new Vector2(5f, 10f))
            }
        };

        smokeEmitter.Modifiers.Add(new AgeModifier
        {
            Interpolators =
            {
                new OpacityInterpolator { StartValue = 0.8f, EndValue = 0f },
                new ScaleInterpolator { StartValue = new Vector2(5f), EndValue = new Vector2(12f) }
            }
        });

        smokeEmitter.Modifiers.Add(new LinearGravityModifier
        {
            Direction = -Vector2.UnitY,
            Strength = 5f
        });

        particleEffect.Emitters.Add(smokeEmitter);

        return particleEffect;
    }
}