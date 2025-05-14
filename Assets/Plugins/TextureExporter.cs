using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class TextureExporter
{
	public static void ExportTexture(Texture2D texture, string name) {
		byte[] data = texture.EncodeToPNG();
		string fullFilename = Application.streamingAssetsPath + "/" + name + ".png";
		File.WriteAllBytes(fullFilename, data);
		Debug.Log("Texture successfully saved to " + fullFilename);
	}
}
