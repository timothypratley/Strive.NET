using System;

using Strive.Multiverse;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Resources;
using Strive.Math3D;

namespace Strive.UI.WorldView
{
	/// <summary>
	/// Note: position and heading of the physical object
	/// are not maintained. This info is kept with the model
	/// so only use those values.
	/// </summary>
	public class PhysicalObjectInstance {

		// Consists of the data object and the view object
		// so we can get to each independently.
		public PhysicalObject physicalObject;
		public IModel model;

		// These variables are to keep track of when the
		// server needs to be sent an update message,
		// atm they only apply to the currently occupied avatar.
		bool hasMoved = false;
		bool stateChanged = false;
		bool velocityChanged = false;
		DateTime lastUpdateSent;
		Vector3D lastVelocitySent = new Vector3D();
		Vector3D currentVelocity = new Vector3D();
		
		// Terrain model loading occurs in TerrainCollection,
		// everything else gets it model loaded upon creation.
		public PhysicalObjectInstance( PhysicalObject po, ResourceManager rm ) {
			physicalObject = po;
			if ( !(po is Terrain) ) {
				if ( po is Mobile ) {
					model = rm.GetActor( po.ObjectInstanceID, po.ResourceID, po.Height );
				} else {
					model = rm.GetModel( po.ObjectInstanceID, po.ResourceID, po.Height );
				}
				model.Label = po.TemplateObjectName;
			}
		}

		// move the model and see if we need to send an update to the server.
		// this only applies to the currently possessed avatar atm.
		public void Move( Vector3D oldPosition, Vector3D velocity, Vector3D newRotation ) {
			// has the change in velocity been above the threshhold?
			currentVelocity.Set( velocity );
			if ( (currentVelocity - lastVelocitySent).GetMagnitudeSquared() > 1 ) {
				velocityChanged = true;
			}

			// TODO: do we really care about state? the server takes care of this,
			// all we need concern ourselves with is velocity change.
			// eg: We do need to know if we have stopped.

			// check to see if we have entered running/walking
			float magnitude = currentVelocity.GetMagnitudeSquared();
			if ( magnitude > 1 ) {
				if ( physicalObject is Mobile && ((Mobile)physicalObject).MobileState != EnumMobileState.Running ) {
					((Mobile)physicalObject).MobileState = EnumMobileState.Running;
					stateChanged = true;
				}
			} else if ( magnitude > 0.3 ) {
				if ( physicalObject is Mobile && ((Mobile)physicalObject).MobileState != EnumMobileState.Walking ) {
					((Mobile)physicalObject).MobileState = EnumMobileState.Walking;
					stateChanged = true;
				}
			}

			// have we stopped?
			if ( currentVelocity == Vector3D.Origin ) {
				if ( lastVelocitySent != Vector3D.Origin ) {
					if ( physicalObject is Mobile && ((Mobile)physicalObject).MobileState != EnumMobileState.Standing ) {
						((Mobile)physicalObject).MobileState = EnumMobileState.Standing;
						stateChanged = true;
					}
				}
			} else {
				model.Position = model.Position+currentVelocity;
				hasMoved = true;
			}

			// have we rotated?
			if ( model.Rotation != newRotation ) {
				model.Rotation = newRotation;
				hasMoved = true;
			}
		}

		public bool NeedsUpdate( DateTime now ) {
			return (
				hasMoved && now - lastUpdateSent > TimeSpan.FromSeconds( 0.5 )
				|| velocityChanged
				|| stateChanged
			);
		}

		// we have sent an update message
		public void SentUpdate( DateTime now ) {
			velocityChanged = false;
			stateChanged = false;
			hasMoved = false;
			lastUpdateSent = now;
		}
	}
}
