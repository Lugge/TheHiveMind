using System;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace AntHill
{
	public class XMLImport
	{
		public  XMLImport ()
		{
		}
		
		public static Dictionary<string, Dictionary<string, string>> importXML2D(string xmlName) {
			Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>> ();
			string path = Application.dataPath;
			string xml_path = path + "/" + xmlName;
			
			if (!File.Exists(xml_path))
			{			
				Debug.Log("File Not Found");
				throw new ArgumentException(); 
			}
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(xml_path);
			XmlNodeList rowNodes = xDoc.GetElementsByTagName ("Row");
			
			//TranslateDictionary<string, TranslateDictionary<string, string>> dict = new TranslateDictionary<string, TranslateDictionary<string, string>>();
			for (int i = 0;  i < rowNodes.Count; i++)
			{				 
				XmlNodeList cellNodes = rowNodes[i].ChildNodes;
				if(!dict.ContainsKey(cellNodes[0].InnerText)) dict.Add(cellNodes[0].InnerText, new Dictionary<string, string>());
				if(cellNodes[1] != null){

					if(!dict[cellNodes[0].InnerText].ContainsKey(cellNodes[1].InnerText)) dict[cellNodes[0].InnerText].Add(cellNodes[1].InnerText, "");
					dict[cellNodes[0].InnerText][cellNodes[1].InnerText] = cellNodes[2].InnerText;
				}
			}

			return dict;
		}

		public static Dictionary<string, string> importXML1D(string xmlName) {
			Dictionary<string, string> dict = new Dictionary<string, string> ();
			string path = Application.dataPath;
			string xml_path = path + "/" + xmlName;
			
			if (!File.Exists(xml_path))
			{			
				Debug.Log("File Not Found");
				throw new ArgumentException(); 
			}
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(xml_path);
			Debug.Log (xDoc.GetElementsByTagName("Row").Count);
			XmlNodeList rowNodes = xDoc.GetElementsByTagName ("Row");
			
			//TranslateDictionary<string, TranslateDictionary<string, string>> dict = new TranslateDictionary<string, TranslateDictionary<string, string>>();
			for (int i = 0;  i < rowNodes.Count; i++)
			{				 
				XmlNodeList cellNodes = rowNodes[i].ChildNodes;
				if(!dict.ContainsKey(cellNodes[0].InnerText)) dict.Add(cellNodes[0].InnerText, "");
				dict[cellNodes[0].InnerText] = cellNodes[1].InnerText;
			}
			
			return dict;
		}

		public static Dictionary<string, double> importXML1DDouble(string xmlName) {
			Dictionary<string, double> dict = new Dictionary<string, double> ();
			string path = Application.dataPath;
			string xml_path = path + "/" + xmlName;
			
			if (!File.Exists(xml_path))
			{			
				Debug.Log("File Not Found: " + xml_path);
				throw new ArgumentException(); 
			}
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(xml_path);
			Debug.Log (xDoc.GetElementsByTagName("Row").Count);
			XmlNodeList rowNodes = xDoc.GetElementsByTagName ("Row");
			
			//TranslateDictionary<string, TranslateDictionary<string, string>> dict = new TranslateDictionary<string, TranslateDictionary<string, string>>();
			for (int i = 0;  i < rowNodes.Count; i++)
			{				 
				XmlNodeList cellNodes = rowNodes[i].ChildNodes;
				if(!dict.ContainsKey(cellNodes[0].InnerText)) dict.Add(cellNodes[0].InnerText, 0);
				dict[cellNodes[0].InnerText] = Convert.ToDouble(cellNodes[1].InnerText);
			}
			
			return dict;
		}
	}
}

