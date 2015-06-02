/// http://answers.unity3d.com/questions/461958/generate-a-gradient-texture-from-editor-script.html
/// http://answers.unity3d.com/questions/436295/how-to-have-a-gradient-editor-in-an-editor-script.html
using UnityEngine; 
using UnityEditor;

using System.Collections; 
using System.IO;

public class GradientToTexture : EditorWindow {

	int width;
	public Gradient gradient;
	[MenuItem ("Window/GradientToTexture")]
	// Use this for initialization
	static void Init(){
		GradientToTexture window = (GradientToTexture)EditorWindow.GetWindow(typeof(GradientToTexture));
		window.maxSize = new Vector2(300,200);
	}
	void OnGUI(){
	if(gradient == null)
	{
		gradient = new Gradient();
	}
		EditorGUI.BeginChangeCheck();
		SerializedObject serializedGradient = new SerializedObject(this);
		SerializedProperty colorGradient = serializedGradient.FindProperty("gradient");
		EditorGUILayout.PropertyField(colorGradient, true, null);
		//if(EditorGUI.EndChangeCheck()) {
			serializedGradient.ApplyModifiedProperties();
		//}
		width = Mathf.Clamp(EditorGUILayout.IntField("Width",width),1,4096);
		if(gradient != null)
		{
			Texture2D tex = new Texture2D(width,1);
			for(int i = 0; i < width; i++)
			{
				tex.SetPixel(i,0,gradient.Evaluate((float)i/(float)width));
			}
			if(GUILayout.Button("Gen")){
				string path = EditorUtility.SaveFilePanel("Save texture as PNG","","foo.png","png");
				if(path.Length != 0)
				{
					GenTexture(tex, path);				
				}
			}
		}
		
		
	}
	void GenTexture(Texture2D tex, string path)
	{
		var pngData = tex.EncodeToPNG();
		if (pngData != null)
			File.WriteAllBytes(path, pngData);
	}

}
