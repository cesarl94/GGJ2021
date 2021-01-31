using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

using System.IO;

public class TiledLayer {
	public string name;
	public int width;
	public int height;
	public int[][] tiles;

	public TiledLayer(XmlNode layerNode) {
		XmlAttribute nameAt = XmlUtils.FindAttribute(layerNode, "name");
		name = nameAt.InnerText;
		XmlAttribute widthAt = XmlUtils.FindAttribute(layerNode, "width");
		XmlAttribute heightAt = XmlUtils.FindAttribute(layerNode, "height");
		width = int.Parse(widthAt.InnerText);
		height = int.Parse(heightAt.InnerText);

		XmlNode data = XmlUtils.FindChild(layerNode, "data");
		XmlNode numbers = data.FirstChild;

		string[] rows = numbers.InnerText.Split('\n');
		string[][] columns = new string[rows.Length][];
		for (int i = 0; i < rows.Length; i++) {
			columns[i] = rows[i].Split(',');
			for (int j = 0; j < columns[i].Length; j++) {
				string onlyNumbers = "";
				foreach (char c in columns[i][j]) {
					if (c >= '0' && c <= '9') onlyNumbers += c;
				}

				columns[i][j] = onlyNumbers;
			}
		}

		List<List<int>> ints = new List<List<int>>();

		for (int i = 0; i < columns.Length; i++) {
			List<int> row = null;
			for (int j = 0; j < columns[i].Length; j++) {
				if (columns[i][j] != "") {
					int value = int.Parse(columns[i][j]);
					if (row == null) {
						row = new List<int>();
					}
					row.Add(value);
				}
			}
			if (row != null) {
				ints.Add(row);
			}
		}

		tiles = new int[width][];
		for (int i = 0; i < width; i++) {
			tiles[i] = new int[height];

			for (int j = 0; j < height; j++) {

				tiles[i][j] = ints[j][i];
			}
		}

	}
}

public class TiledMap {
	public TiledLayer layer;
	public int width;
	public int height;


	public TiledMap(string fileText, string layerName) {
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(fileText);

		XmlNode map = XmlUtils.FindChild(doc, "map");
		XmlAttribute widthAt = XmlUtils.FindAttribute(map, "width");
		XmlAttribute heightAt = XmlUtils.FindAttribute(map, "height");
		width = int.Parse(widthAt.InnerText);
		height = int.Parse(heightAt.InnerText);

		List<XmlNode> layersNodes = XmlUtils.FindChilds(map, "layer");
		foreach (XmlNode layerNode in layersNodes) {
			if (XmlUtils.FindAttribute(layerNode, "name").InnerText == layerName) {
				layer = new TiledLayer(layerNode);
				return;
			}
		}
	}
}

static class XmlUtils {
	public static XmlNode FindChild(XmlNode parent, string name) {
		for (int i = 0; i < parent.ChildNodes.Count; i++) {
			if (parent.ChildNodes[i].Name == name) return parent.ChildNodes[i];
		}

		return null;
	}

	public static XmlAttribute FindAttribute(XmlNode node, string nameAttribute) {
		if (node.Attributes == null) return null;
		for (int i = 0; i < node.Attributes.Count; i++) {
			if (node.Attributes[i].Name == nameAttribute) return node.Attributes[i];
		}
		return null;
	}

	public static List<XmlNode> FindChilds(XmlNode parent, string name) {
		List<XmlNode> rv = new List<XmlNode>();

		for (int i = 0; i < parent.ChildNodes.Count; i++) {
			if (parent.ChildNodes[i].Name == name)
				rv.Add(parent.ChildNodes[i]);
		}

		return rv;
	}

	public static List<string> FindLevelsByType(string fileText, string gameMode) {
		List<string> rv = new List<string>();
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(fileText);

		XmlNode map = XmlUtils.FindChild(doc, "map");

		List<XmlNode> groupNodes = XmlUtils.FindChilds(map, "group");
		foreach (XmlNode groupNode in groupNodes) {
			XmlAttribute groupNodeNameAt = XmlUtils.FindAttribute(groupNode, "name");
			string groupNodeName = groupNodeNameAt.InnerText;

			if (groupNodeName == gameMode) {
				List<XmlNode> levelNodes = XmlUtils.FindChilds(groupNode, "group");
				foreach (XmlNode levelNode in levelNodes) {
					XmlAttribute levelNodeNameAt = XmlUtils.FindAttribute(levelNode, "name");
					string levelNodeName = levelNodeNameAt.InnerText;
					rv.Add(levelNodeName);
				}
				return rv;
			}
		}

		// foreach (XmlNode levelNode in groupNodes)
		// {
		// 	XmlAttribute levelNodeNameAt = XmlUtils.FindAttribute(levelNode, "name");
		// 	string levelNodeName = levelNodeNameAt.InnerText;
		// 	rv.Add(levelNodeName);
		// }

		// return rv;
		return null;
	}
}
