using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Text;

public class PersistenceHelper
{
	public const string HIGHSCORE_KEY = "HighScore";
    public const string HIGHTIME_KEY = "HighTime";

	private const string INFO_ELEM = "Info";

	private const string FILENAME = "TryToSurvive.info";

	private string _FilePath;
	private Dictionary<string,string> _Values;

	private static PersistenceHelper _Instance;
	public static PersistenceHelper Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = new PersistenceHelper();
			return _Instance;
		}
	}

	private PersistenceHelper()
	{
		_Values = new Dictionary<string, string>();
		_FilePath = Path.Combine(Application.persistentDataPath, FILENAME);

		_ReadValues();
	}

	private void _ReadValues()
	{
		if (File.Exists(_FilePath))
		{
			string xml = File.ReadAllText(_FilePath, Encoding.UTF8);

			byte[] data = Convert.FromBase64String(xml);
			xml = Encoding.UTF8.GetString(data);

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);

			XmlNodeList highScoreNodes = xmlDocument.GetElementsByTagName(HIGHSCORE_KEY);
			if (highScoreNodes != null && highScoreNodes.Count > 0)
				_Values.Add(HIGHSCORE_KEY, highScoreNodes[0].InnerText);

			XmlNodeList highTimeNodes = xmlDocument.GetElementsByTagName(HIGHTIME_KEY);
			if (highTimeNodes != null && highTimeNodes.Count > 0)
				_Values.Add(HIGHTIME_KEY, highTimeNodes[0].InnerText);
		}
	}

	public void Save()
	{
		string xml = _GetXml();

		byte[] data = Encoding.UTF8.GetBytes(xml);
		string base64Data = Convert.ToBase64String(data);
		File.WriteAllText(_FilePath, base64Data);
	}

	private string _GetXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		
		using (XmlWriter writer = XmlWriter.Create(stringBuilder))
		{
			writer.WriteStartElement(INFO_ELEM);
			
			foreach (string key in _Values.Keys)
				writer.WriteElementString(key, _Values[key]);
			
			writer.WriteEndElement();

			return stringBuilder.ToString();
		}
	}

	public void PersistInteger (string key, int value)
	{
		if (!_Values.ContainsKey(key))
			_Values.Add(key, value.ToString());
		else
			_Values[key] = value.ToString();
	}

	public int ReadInteger (string key)
	{
		int value = 0;
		if (_Values.ContainsKey(key))
			int.TryParse(_Values[key], out value);

		return value;
	}

    public void PersistFloat(string key, float value)
    {
		if (!_Values.ContainsKey(key))
			_Values.Add(key, value.ToString());
		else
			_Values[key] = value.ToString();
    }

    public float ReadFloat(string key)
    {
        float value = 0;
        if (_Values.ContainsKey(key))
			float.TryParse(_Values[key], out value);

        return value;
    }
}
