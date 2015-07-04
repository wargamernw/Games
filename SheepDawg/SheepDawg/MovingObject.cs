namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;

	public class MovingObject
	{
		public Point Location { get; set; }

		public Point Destination { get; set; }

		// Pixel Points per second
		public double Velocity { get; private set; }

		// Pixel Points per second
		public double MaxVelocity { get; set; }

		// Pixel Points per second/second
		public double Accelleration { get; set; }

		// Radians per second
		public double TurnRate { get; set; }

		// Radians
		public double Facing { get; set; }

		public MovingObject(Point loc, double maxVel, double accelleration, double turnRate, double facing)
		{
			this.Location = loc;
			this.MaxVelocity = maxVel;
			this.Velocity = 0;
			this.Accelleration = accelleration;
			this.TurnRate = turnRate;
			this.Facing = facing;
		}

		public void OnTick(double elapsed)
		{
			if (this.Location != this.Destination)
			{
				var deltaX = this.Destination.X - this.Location.X;
				var deltaY = this.Destination.Y - this.Location.Y;

				var dist = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

				// Are we there yet?
				if (dist < this.Velocity * elapsed)
				{
					this.Location = this.Destination;
					this.Velocity = 0;
					return;
				}

				// How long to reach 0;
				var impulses = this.Velocity / this.Accelleration;

				var drift = this.Velocity * impulses;

				// Calculate Velocity
				if (dist < drift)
				{
					this.Velocity -= this.Accelleration * elapsed;

					if (this.Velocity < this.Accelleration * elapsed)
					{
						this.Velocity = this.Accelleration * elapsed;
					}
				}
				else if (this.Velocity < this.MaxVelocity 
					&& (dist > drift * 2
						|| this.Velocity == 0.0))
				{
					this.Velocity = Math.Min(this.MaxVelocity, this.Velocity + this.Accelleration * elapsed);
				}

				var move = new Vector(deltaX, deltaY);
				move.Normalize();
				
				// Calculate facing
				var targetFacing = Math.Atan2(move.Y, move.X);

				var deltaFacing = targetFacing - this.Facing;

				if (deltaFacing > Math.PI)
				{
					deltaFacing -= Math.PI * 2.0;
				}
				else if (deltaFacing < -Math.PI)
				{
					deltaFacing += Math.PI * 2.0;
				}

				if (Math.Abs(this.Facing - targetFacing) < this.TurnRate * elapsed)
				{
					this.Facing = targetFacing;
				}
				else if (deltaFacing > 0)
				{
					this.Facing += this.TurnRate * elapsed;
				}
				else if (deltaFacing < 0)
				{
					this.Facing -= this.TurnRate * elapsed;
				}

				Vector targetMove = new Vector();

				targetMove.X = Math.Cos(this.Facing);
				targetMove.Y = Math.Sin(this.Facing);
				targetMove.Normalize();
				targetMove *= this.Velocity * elapsed;
				this.Location += targetMove;
			}
		}
	}
}
