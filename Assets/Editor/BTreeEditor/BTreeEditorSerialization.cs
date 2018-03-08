using BTreeFrame;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BTree.Editor
{
    public class BTreeEditorSerialization
    {
        public static string m_ConfigPath = "Assets/Editor/BTreeEditor/Config/";
        public static string m_Suffix = ".btreeEditor";
        public static void WriteBinary(BTreeEditorConfig _bTree, string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(m_ConfigPath + _name + m_Suffix, System.IO.FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }

        public static BTreeEditorConfig ReadBinary(string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(m_ConfigPath + _name + m_Suffix, System.IO.FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            BTreeEditorConfig bTree = binaryFormatter.Deserialize(fs) as BTreeEditorConfig;
            fs.Close();
            return bTree;
        }

        public static void WriteXML(BTreeEditorConfig _bTree, string _name)
        {
            WirteXMLAtPath(_bTree, m_ConfigPath + _name + ".xml");
        }
        public static void WirteXMLAtPath(BTreeEditorConfig _bTree, string _path)
        {
            XmlSerializer writer = new XmlSerializer(typeof(BTreeEditorConfig));
            System.IO.StreamWriter file = new System.IO.StreamWriter(_path);
            writer.Serialize(file, _bTree);
            file.Close();
        }
        public static BTreeEditorConfig ReadXML(string _name)
        {
            return ReadXMLAtPath(m_ConfigPath + _name + ".xml");
        }
        public static BTreeEditorConfig ReadXMLAtPath(string _path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(BTreeEditorConfig));
            System.IO.StreamReader file = new System.IO.StreamReader(_path);
            BTreeEditorConfig btree = reader.Deserialize(file) as BTreeEditorConfig;
            file.Close();
            return btree;
        }
    }
}
