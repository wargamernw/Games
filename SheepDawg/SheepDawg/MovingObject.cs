namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Xml;

	public class MovingObject
	{
		private MovingObjectController controller;

		public MovingObjectData Data { get; set; }

		public MovingObjectDataTemporal Pos { get; set; }

		public MovingObject(MovingObjectController controller)
		{
			this.controller = controller;
			this.Data = new MovingObjectData();
			this.Pos = new MovingObjectDataTemporal();
		}

		public void Load(string dataFile)
		{
			try
			{
				using (var fs = new FileStream(dataFile, FileMode.OpenOrCreate))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(MovingObjectData));
					this.Data = (MovingObjectData)ser.ReadObject(fs);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}

		public void LoadTemporal(string dataFile)
		{
			try
			{
				using (var fs = new FileStream(dataFile, FileMode.OpenOrCreate))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(MovingObjectDataTemporal));
					this.Pos = (MovingObjectDataTemporal)ser.ReadObject(fs);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}

		public void Save(string dataFile)
		{
			try
			{
				using (var fs = new FileStream(dataFile, FileMode.OpenOrCreate))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(MovingObjectData));
					ser.WriteObject(fs, this.Data);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}

		public void SaveTemporal(string dataFile)
		{
			try
			{
				using (var fs = new FileStream(dataFile, FileMode.OpenOrCreate))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(MovingObjectDataTemporal));
					ser.WriteObject(fs, this.Pos);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}

		public void OnTick(double elapsed)
		{
			var impulse = this.CheckCollision();

			var delta = this.Pos.Destination - this.Pos.Location;

			if (delta.LengthSquared > Math.Pow(this.Data.Radius * 1.5, 2.0)
				|| impulse.LengthSquared > 0)
			{
				var dist = Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);

				// Are we there yet and not being pushed?
				if (dist < this.Pos.Velocity * elapsed
					&& impulse.LengthSquared == 0)
				{
					this.Pos.Location = this.Pos.Destination;
					this.Pos.Velocity = 0;
					return;
				}

				this.CheckVelocity(dist, elapsed);

				this.CheckFacing(elapsed);
				
				Vector targetMove = new Vector();
				targetMove.X = Math.Cos(this.Pos.Facing);
				targetMove.Y = Math.Sin(this.Pos.Facing);
				targetMove.Normalize();

				targetMove += impulse;

				targetMove *= this.Pos.Velocity * elapsed;
				Debug.Assert(!double.IsNaN(targetMove.X) && !double.IsNaN(targetMove.Y));
				this.Pos.Location += targetMove;
			}
		}

		private void CheckVelocity(double dist, double elapsed)
		{
			// How long to reach 0;
			var impulses = this.Pos.Velocity / this.Data.Accelleration;

			var drift = this.Pos.Velocity * impulses;

			// Slow down
			if (dist < drift)
			{
				this.Pos.Velocity -= this.Data.Accelleration * elapsed;

				if (this.Pos.Velocity < this.Data.Accelleration * elapsed)
				{
					this.Pos.Velocity = this.Data.Accelleration * elapsed;
				}
			}
			// Speed up
			else if (this.Pos.Velocity < this.Data.MaxVelocity
				&& (dist > drift * 2
					|| this.Pos.Velocity == 0.0))
			{
				this.Pos.Velocity = Math.Min(this.Data.MaxVelocity, this.Pos.Velocity + this.Data.Accelleration * elapsed);
			}
		}

		private void CheckFacing(double elapsed)
		{
			var delta = this.Pos.Destination - this.Pos.Location;
			var move = new Vector(delta.X, delta.Y);
			move.Normalize();

			// Calculate facing
			var targetFacing = Math.Atan2(move.Y, move.X);

			var deltaFacing = targetFacing - this.Pos.Facing;

			if (deltaFacing > Math.PI)
			{
				deltaFacing -= Math.PI * 2.0;
			}
			else if (deltaFacing < -Math.PI)
			{
				deltaFacing += Math.PI * 2.0;
			}

			if (Math.Abs(this.Pos.Facing - targetFacing) < this.Data.TurnRate * elapsed)
			{
				this.Pos.Facing = targetFacing;
			}
			else if (deltaFacing > 0)
			{
				this.Pos.Facing += this.Data.TurnRate * elapsed;
			}
			else if (deltaFacing < 0)
			{
				this.Pos.Facing -= this.Data.TurnRate * elapsed;
			}
		}

		private Vector CheckCollision()
		{
			var push = new Vector();

			foreach (var obj in this.controller.Objects)
			{
				var delta = new Vector(this.Pos.Location.X - obj.Pos.Location.X, this.Pos.Location.Y - obj.Pos.Location.Y);

				if (delta.LengthSquared > 0)
				{
					if (delta.LengthSquared < Math.Pow(obj.Data.Radius + this.Data.Radius, 2.0))
					{
						delta.Normalize();
						push += delta * 1.2;
					}
					else if (delta.LengthSquared < Math.Pow(obj.Data.Radius * 2.0 + this.Data.Radius * 2.0, 2.0))
					{
						delta.Normalize();
						push += delta / 2.0;
					}
				}
			}

			return push;
		}
	}
}
