using UnityEngine;

namespace Pathfinding.Examples {
	[ExecuteInEditMode]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/documentationbutton.html")]
	public class DocumentationButton : MonoBehaviour {
		public string page;

		const string UrlBase = "https://arongranberg.com/astar/docs/";

#if UNITY_EDITOR
		void OnGUI () {
			if (GUI.Button(new Rect(Screen.width - 250, Screen.height - 60, 240, 50), "Example Scene Documentation")) {
				Application.OpenURL(UrlBase + page + ".html");
			}
		}
#endif
	}
}
