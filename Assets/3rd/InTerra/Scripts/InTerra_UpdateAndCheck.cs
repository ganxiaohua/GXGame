using UnityEngine;
using System;

namespace InTerra
{
	[Serializable] public class DictionaryMaterialTerrain : SerializableDictionary<Material, Terrain> { }
	public class InTerra_UpdateAndCheck : MonoBehaviour
	{
		[SerializeField, HideInInspector] public bool FirstInit;
		[SerializeField, HideInInspector] public DictionaryMaterialTerrain MaterialTerrain = new DictionaryMaterialTerrain();
		void Update()
		{
			if (!InTerra_Setting.DisableAllAutoUpdates) InTerra_Data.CheckAndUpdate();
		}
	}
}
