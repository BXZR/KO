using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.Collections.Generic;

public class xmlFileUse : MonoBehaviour {

	public static xmlFileUse fileMaker;
	void Start () 
	{
		fileMaker  = this;
    }

	public static void saveWithXMLStatic(string fileName, string data)
	{
		StreamWriter writerMaker;
		try
		{
			writerMaker = File.CreateText(fileName);
		}
		catch
		{
			writerMaker = File.CreateText (fileName);
		}

		using (StreamWriter writer =  writerMaker)
		{
			writer .Write (data);
			writer .Close ();
		}
		writerMaker.Close ();
	}

	public static string loadWithXMLStatic(string fileName)
	{
		using (StreamReader reader= File.OpenText (fileName))
		{
			string s = 	reader .ReadToEnd();
			reader .Close();
			return s;
		}
	}


	public void save(string filename, object theOBJ , System .Type  type)
	{
		string stringToSave = serializeOBJ (theOBJ,type);
		saveWithXML (filename,stringToSave);
	}


	public object load (string fileName ,System.Type type)
	{
		string stringGet = loadWithXML (fileName);
		object obj = DeserilizeOBJ (stringGet, type);
		return obj;
	}

	public void saveWithXML(string fileName, string data)
	{
		using (StreamWriter writer = File.CreateText (fileName))
		{
						writer .Write (data);
						writer .Close ();
		}
	}

	public string loadWithXML(string fileName)
	{
		using (StreamReader reader= File.OpenText (fileName))
		{
			string s = 	reader .ReadToEnd();
			reader .Close();
			return s;
		}
	}


	public string serializeOBJ(object theOBJ,System .Type  ty)
	{
		string xmlString = "";
		MemoryStream memoryStream = new MemoryStream ();
		XmlSerializer xs = new XmlSerializer (ty);
		XmlTextWriter xmlwriter = new XmlTextWriter (memoryStream, Encoding.UTF8);
		xs.Serialize (xmlwriter, theOBJ);
		memoryStream = (MemoryStream)xmlwriter.BaseStream;
		xmlString = UTF8ByteArrayToString (memoryStream .ToArray());
		return xmlString;
	}

	public object DeserilizeOBJ(string stringGet,System.Type ty)
	{
		XmlSerializer xs = new XmlSerializer (ty);
		MemoryStream memoryStream = new MemoryStream (stringToUTFBytrArray(stringGet));
		XmlTextWriter xmlWriter = new XmlTextWriter (memoryStream ,Encoding .UTF8);
		return xs.Deserialize (memoryStream);
	}

	string UTF8ByteArrayToString(byte[] array)
	{
		UTF8Encoding encoding = new UTF8Encoding ();
		return ( encoding .GetString (array));
	}

	byte[] stringToUTFBytrArray(string s)
	{
		UTF8Encoding encoding = new UTF8Encoding ();
		return ( encoding .GetBytes (s));
	}
}
