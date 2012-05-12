using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MultiDungeon
{
	public class Camera
	{
		protected float zoom; // Camera Zoom
		public Matrix transform; // Matrix Transform
		public Vector2 pos; // Camera Position
		protected float rotation; // Camera Rotation
		GraphicsDeviceManager graphics;

		public Camera(GraphicsDeviceManager grph)
		{
			zoom = 1.0f;
			rotation = 0.0f;
			pos = Vector2.Zero;
			graphics = grph;
		}
		// Sets and gets zoom
		public float Zoom
		{
			get { return zoom; }
			set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
		}

		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}

		// Auxiliary function to move the camera
		public void Move(Vector2 amount)
		{
			pos += amount;
		}
		// Get set position
		public Vector2 Pos
		{
			get { return pos; }
			set { pos = value; }
		}
		/*
		public Matrix getTransformation()
		{
			transform =       
			  Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
										 Matrix.CreateRotationZ(Rotation) *
										 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
										 Matrix.CreateTranslation(new Vector3(graphics.GraphicsDevice.Viewport.Width * 0.5f, 
											 graphics.GraphicsDevice.Viewport.Height * 0.5f, 0));
			return transform;
		}
		*/
		public Matrix getTransformation()
		{
			transform =
				Matrix.Identity *
				Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
				Matrix.CreateRotationZ(this.rotation) *
				Matrix.CreateScale(new Vector3(this.zoom, this.zoom, 1)) *
				Matrix.CreateTranslation(new Vector3(graphics.GraphicsDevice.Viewport.Width * 0.5f,
					graphics.GraphicsDevice.Viewport.Height * 0.5f, 0));
			return transform;
		}
		public Matrix getTransformation(int parallax)
		{
			transform =
				Matrix.Identity *
				Matrix.CreateTranslation(new Vector3(-pos.X / parallax / 2, -pos.Y, 0)) *
				Matrix.CreateRotationZ(this.rotation) *
				Matrix.CreateScale(new Vector3(this.zoom, this.zoom, 1)) *
				Matrix.CreateTranslation(new Vector3(graphics.GraphicsDevice.Viewport.Width * 0.5f,
					graphics.GraphicsDevice.Viewport.Height * 0.5f, 0));
			return transform;
		}
		public Vector2 ToLocalLocation(Vector2 position)
		{
			return Vector2.Transform(position, transform);
		}
		public Vector2 ToWorldLocation(Vector2 position)
		{
			return Vector2.Transform(position, Matrix.Invert(transform));
		}
	}
}
