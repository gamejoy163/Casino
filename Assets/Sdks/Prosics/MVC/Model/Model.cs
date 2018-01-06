/// <copyright file="Model.cs">Copyright (c) 2015 All Rights Reserved</copyright>
/// <author>Joris van Leeuwen</author>
/// <date>01/27/2014</date>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Prosics.MVC
{
	/// <copyright file="Model.cs">Copyright (c) 2015 All Rights Reserved</copyright>
	/// <author>Joris van Leeuwen</author>
	/// <date>01/27/2014</date>
	/// <summary>A Model is a representation of data, which can be saved and loaded into/from different data types. It will delete itself automatically when there is no ModelRef(s) left referencing it. Extend the Model into a custom class to add data fields that can be saved/loaded. Use Models in combination with Controllers for max use.</summary>
	[Serializable]
	public abstract class Model : ModelReferencer
	{

		/// <summary>
		/// Called when the NotifyChange is called on a Model. Only works in Editor.
		/// </summary>
		/// <param name="affecterType">The type that called the NotifyChange on the Model.</param>
		/// <param name="modelType">The type of Model on which the NotifyChange is called.</param>
    public delegate void OnModelAffectDelegate (Type affecterType, Type modelType);

		/// <summary>Called when a model change is notified. Only works when in the Unity editor.</summary>
		public static OnModelAffectDelegate OnModelChangeNotified;

		/// <summary>Called when a model change is notified. Only works when in the Unity editor.</summary>
		public static OnModelAffectDelegate OnModelChangeHandled;

		/// <summary>Called when a model is deleted. Only works when in the Unity editor.</summary>
		public static OnModelAffectDelegate OnModelDeleted;

		/// <summary>Called when a model deletion is handled. Only works when in the Unity editor.</summary>
		public static OnModelAffectDelegate OnModelDeleteHandled;

		/// <summary>
		/// The id of this model, which can be used to find it later on.
		/// </summary>
		public string Id
		{
			get
			{
				return id;
			}
			private set
			{
				Unregister ();
				id = value;
				Register ();
			}
		}

		private static Dictionary<string, Model> sortedInstances = new Dictionary<string, Model> ();
		private static Dictionary<Type, List<Model>> typeSortedInstances = new Dictionary<Type, List<Model>> ();
		private static List<Model> instances = new List<Model> ();

		private static bool isSerializing;

		private string id;
		private bool isRegistered;
		private bool referencesCollected;
		private int refCount;
		private List<Delegate> onChangeHandlers;
		private List<Delegate> onDeleteHandlers;


  





		/// <summary>
		/// Finds and returns the model with the given id.
		/// </summary>
		/// <param name="id">The id used to find the model.</param>
		/// <returns>The model found with the given id.</returns>
		public static Model Find (string id)
		{
			if (!sortedInstances.ContainsKey (id))
			{
				UnityEngine.Debug.LogError ("Could not find model with id '" + id + "'");
				return null;
			}
			return sortedInstances [id];
		}

		/// <summary>
		/// Finds and returns the model of given type with the given id.
		/// </summary>
		/// <typeparam name="T">The type used to find the model.</typeparam>
		/// <param name="id">The id used to find the model.</param>
		/// <returns>The model found with the given type and id.</returns>
		public static Model Find<T> (string id) where T : Model
		{
			Model model = Find (id);
			if (model.GetType () != typeof(T))
			{
				UnityEngine.Debug.LogError ("Could not find model with id '" + id + "' and type '" + typeof(T) + "'");
				return null;
			}
			return model;
		}

		/// <summary>
		/// Finds and returns an instance of the given model type.
		/// </summary>
		/// <typeparam name="T">The type used to find the instance.</typeparam>
		/// <returns>The model found with the given type.</returns>
		public static T First<T> () where T : Model
		{
			Type type = typeof(T);
			if (!typeSortedInstances.ContainsKey (type))
			{
				return null;
			}
			if (typeSortedInstances [type].Count == 0)
			{
				return null;
			}
			return typeSortedInstances [type] [0] as T;
		}

		/// <summary>
		/// Returns all models.
		/// </summary>
		/// <returns>All models.</returns>
		public static List<Model> GetAll ()
		{
			return instances;
		}

		/// <summary>
		/// Finds and returns all models of the given type.
		/// </summary>
		/// <typeparam name="T">The type used to find the models.</typeparam>
		/// <returns>All models of given type.</returns>
		public static List<T> GetAll<T> () where T : Model
		{
			List<T> models = new List<T> ();
			Type type = typeof(T);
			if (!typeSortedInstances.ContainsKey (type))
			{
				return models;
			}
			foreach (Model model in typeSortedInstances[type])
			{
				models.Add ((T)model);
			}
			return models;
		}
		/*
		 * 禁止直接操作model ，model的摧毁必须由管理它的controller来进行
		/// <summary>
		/// Deletes all models.
		/// </summary>
		public static void DeleteAll ()
		{
			while (instances.Count > 0)
			{
				instances [0].Delete ();
			}
		}

		/// <summary>
		/// Deletes all models of given type.
		/// </summary>
		/// <typeparam name="T">The type used to find and delete the models.</typeparam>
		public static void DeleteAll<T> () where T : Model
		{
			List<T> models = GetAll<T> ();
			while (models.Count > 0)
			{
				if (sortedInstances.ContainsKey (models [0].Id))
				{
					models [0].Delete ();
				}
				models.RemoveAt (0);
			}
		}
		*/
		public Model ()
		{
			Id = Guid.NewGuid ().ToString ();
			refCount = 0;
			onChangeHandlers = new List<Delegate> ();
			onDeleteHandlers = new List<Delegate> ();
		}

		/// <summary>
		/// Adds a listener that triggers the given callback when the NotifyChange is called on this Model.
		/// </summary>
		/// <param name="callback">The callback that will be triggered when NotifyChange is called.</param>
		public void AddChangeListener (Action callback)
		{
			if (callback == null)
			{
				UnityEngine.Debug.LogError ("Failed to add ChangeListener on Model but the given callback is null!");
				return;
			}
			onChangeHandlers.Add (callback);
		}

		/// <summary>
		/// Adds a listener that triggers the given callback when the NotifyChange is called on this Model.
		/// </summary>
		/// <param name="callback">The callback that will be triggered when NotifyChange is called.</param>
		public void AddChangeListener (Action<Model> callback)
		{
			if (callback == null)
			{
				UnityEngine.Debug.LogError ("Failed to add ChangeListener on Model but the given callback is null!");
				return;
			}
			onChangeHandlers.Add (callback);
		}

		/// <summary>
		/// Removes a listener that would trigger the given callback when the NotifyChange is called on this Model.
		/// </summary>
		/// <param name="callback">The callback that is triggered when the NotifyChange is called.</param>
		public void RemoveChangeListener (Action callback)
		{
			onChangeHandlers.Remove (callback);
		}

		/// <summary>
		/// Removes a listener that triggers the given callback when the NotifyChange is called on this Model
		/// </summary>
		/// <param name="callback">The callback that is triggered when the NotifyChange is called.</param>
		public void RemoveChangeListener (Action<Model> callback)
		{
			onChangeHandlers.Remove (callback);
		}

		/// <summary>
		/// Adds a listener that triggers the given callback when this Model is deleted.
		/// </summary>
		/// <param name="callback">The callback that will be triggered when NotifyChange is called.</param>
		public void AddDeleteListener (Action callback)
		{
			if (callback == null)
			{
				UnityEngine.Debug.LogError ("Failed to add DeleteListener on Model but the given callback is null!");
				return;
			}
			onDeleteHandlers.Add (callback);
		}

		/// <summary>
		/// Adds a listener that triggers the given callback when this Model is deleted.
		/// </summary>
		/// <param name="callback">The callback that will be triggered when NotifyChange is called.</param>
		public void AddDeleteListener (Action<Model> callback)
		{
			if (callback == null)
			{
				UnityEngine.Debug.LogError ("Failed to add DeleteListener on Model but the given callback is null!");
				return;
			}
			onDeleteHandlers.Add (callback);
		}

		/// <summary>
		/// Removes a listener that triggers the given callback when this Model is deleted.
		/// </summary>
		/// <param name="callback">The callback that triggers when NotifyChange is called.</param>
		public void RemoveDeleteListener (Action callback)
		{
			onDeleteHandlers.Remove (callback);
		}

		/// <summary>
		/// Removes a listener that triggers the given callback when this Model is deleted.
		/// </summary>
		/// <param name="callback">The callback that triggers when NotifyChange is called.</param>
		public void RemoveDeleteListener (Action<Model> callback)
		{
			onDeleteHandlers.Remove (callback);
		}

		/// <summary>
		/// Sends out callbacks to this Model's change listeners.
		/// </summary>
		public void NotifyChange ()
		{
			if (Application.isEditor && OnModelChangeNotified != null)
			{
				StackTrace stackTrace = new StackTrace ();
				Type notifierType = stackTrace.GetFrame (1).GetMethod ().DeclaringType;
				OnModelChangeNotified (notifierType, GetType ());
			}

			List<Delegate> callbacks = new List<Delegate> (onChangeHandlers);
			while (callbacks.Count > 0)
			{
				Delegate callback = callbacks [0];
				if (Application.isEditor && OnModelChangeHandled != null)
				{
					OnModelChangeHandled (callback.Target.GetType (), GetType ());
				}
				CallbackModelDelegate (callback);
				callbacks.Remove (callback);
			}
		}

		/// <summary>
		/// Deletes this Model, removing it from ModelRefs lists and destroying its linked Controllers.
		/// </summary>
		public override void Delete ()
		{
			if (!sortedInstances.ContainsKey (id))
			{
				return;
			}

			if (Application.isEditor && OnModelDeleted != null)
			{
				StackTrace stackTrace = new StackTrace ();
				Type deleterType = stackTrace.GetFrame (1).GetMethod ().DeclaringType;
				OnModelDeleted (deleterType, GetType ());
			}

			while (onDeleteHandlers.Count > 0)
			{
				Delegate callback = onDeleteHandlers [0];
				if (Application.isEditor && OnModelDeleteHandled != null)
				{
					OnModelDeleteHandled (callback.Target.GetType (), GetType ());
				}
				CallbackModelDelegate (callback);
				onDeleteHandlers.Remove (callback);
			}

			Unregister ();

			List<ModelReferencer> modelReferencers = GetModelReferencersInFields ();
			foreach (ModelReferencer referencer in modelReferencers)
			{
				if (referencer == null)
				{
					continue;
				}
				referencer.Delete ();
			}
		}

		internal override List<Model> GetReferences ()
		{
			List<Model> references = new List<Model> ();
			List<ModelReferencer> referencers = GetModelReferencersInFields ();
			foreach (ModelReferencer referencer in referencers)
			{
				references.AddList<Model> (referencer.GetReferences ());
			}
			references.Distinct<Model> ();
			return references;
		}

		internal override void CollectReferences ()
		{
			if (referencesCollected)
			{
				return;
			}
			referencesCollected = true;
			List<ModelReferencer> referencers = GetModelReferencersInFields ();
			foreach (ModelReferencer referencer in referencers)
			{
				referencer.CollectReferences ();
			}
		}

		internal void IncreaseRefCount ()
		{
			refCount++;
		}

		internal void DecreaseRefCount ()
		{
			refCount--;
			if (refCount <= 0)
			{
				Delete ();
			}
		}

		private List<ModelReferencer> GetModelReferencersInFields ()
		{
			FieldInfo[] fields = GetType ().GetFields ();
			List<ModelReferencer> modelReferencers = new List<ModelReferencer> ();
			foreach (FieldInfo field in fields)
			{
				if (field.GetValue (this) is ModelReferencer)
				{
					modelReferencers.Add (field.GetValue (this) as ModelReferencer);
				}
			}
			return modelReferencers;
		}

		private void Register ()
		{
			if (isSerializing)
			{
				return;
			}
			if (isRegistered)
			{
				return;
			}
			isRegistered = true;
			sortedInstances.Add (id, this);
        
			if (!typeSortedInstances.ContainsKey (GetType ()))
			{
				typeSortedInstances.Add (GetType (), new List<Model> ());
			}
			typeSortedInstances [GetType ()].Add (this);

			instances.Add (this);
		}

		private void Unregister ()
		{
			if (!isRegistered)
			{
				return;
			}
			isRegistered = false;
			if (sortedInstances.ContainsValue (this))
			{
				foreach (KeyValuePair<string, Model> pair in sortedInstances)
				{
					if (pair.Value == this)
					{
						sortedInstances.Remove (pair.Key);
						break;
					}
				}
			}
			if (typeSortedInstances.ContainsKey (GetType ()))
			{
				typeSortedInstances [GetType ()].Remove (this);
			}
			instances.Remove (this);
			if (!string.IsNullOrEmpty (id))
			{
				sortedInstances.Remove (id);
			}
			instances.Remove (this);
		}

		private void CallbackModelDelegate (Delegate callback)
		{
			if (callback is Action<Model>)
			{
				Action<Model> action = callback as Action<Model>;
				action (this);
			}
			else
			{
				Action action = callback as Action;
				action ();
			}
		}

	}
}