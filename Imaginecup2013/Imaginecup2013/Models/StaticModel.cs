﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Imaginecup2013
{
    /// <summary>
    /// Component that draws a model.
    /// </summary>
    public class StaticModel : DrawableGameComponent
    {
        Model model;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;
        Effect effect;
        public Texture tex;

        /// <summary>
        /// Creates a new StaticModel.
        /// </summary>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public StaticModel(Model model, Matrix transform, Game game) : base(game)
        {
            this.model = model;
            this.Transform = transform;
            effect = (game as Leoni).simpleEffect;

            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the StaticModel to work with more complicated shapes.
            /*foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
            }*/
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect = effect;
            }
        }

        public StaticModel(Model model, Matrix transform, Game game, Effect effect)
            : base(game)
        {
            this.model = model;
            this.Transform = transform;
            this.effect = effect;
            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the StaticModel to work with more complicated shapes.
            /*foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
            }*/
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect = effect;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["Main"];
                    effect.Parameters["World"].SetValue( boneTransforms[mesh.ParentBone.Index] * Transform );
                    effect.Parameters["View"].SetValue( (Game as Leoni).Camera.ViewMatrix );
                    effect.Parameters["Projection"].SetValue((Game as Leoni).Camera.ProjectionMatrix);
                    effect.Parameters["tex"].SetValue(tex);

                    /*effect.World = boneTransforms[mesh.ParentBone.Index] * Transform;
                    effect.View = (Game as Leoni).Camera.ViewMatrix;
                    effect.Projection = (Game as Leoni).Camera.ProjectionMatrix;*/
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
