﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities;

namespace Imaginecup2013
{
    /// <summary>
    /// Component that draws a model following the position and orientation of a BEPUphysics entity.
    /// </summary>
    public class EntityModel : DrawableGameComponent
    {
        /// <summary>
        /// Entity that this model follows.
        /// </summary>
        Entity entity;
        Model model;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;
        Effect effect;


        /// <summary>
        /// Creates a new EntityModel.
        /// </summary>
        /// <param name="entity">Entity to attach the graphical representation to.</param>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public EntityModel(Entity entity, Model model, Matrix transform, Game game) : base(game)
        {
            this.entity = entity;
            this.model = model;
            this.Transform = transform;
            effect = (game as Leoni).mainEffect;

            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the EntityModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            
            /*foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
            }*/
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect = effect;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //Notice that the entity's worldTransform property is being accessed here.
            //This property is returns a rigid transformation representing the orientation
            //and translation of the entity combined.
            //There are a variety of properties available in the entity, try looking around
            //in the list to familiarize yourself with it.
            Matrix worldMatrix = Transform * entity.WorldTransform;


            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    /*
                    effect.World = boneTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = (Game as Leoni).Camera.ViewMatrix;
                    effect.Projection = (Game as Leoni).Camera.ProjectionMatrix;*/

                    effect.CurrentTechnique = effect.Techniques["Simple"];
                    effect.Parameters["World"].SetValue(boneTransforms[mesh.ParentBone.Index] * worldMatrix);
                    effect.Parameters["View"].SetValue((Game as Leoni).Camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue((Game as Leoni).Camera.ProjectionMatrix);
                    
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
