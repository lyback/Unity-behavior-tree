using BTreeFrame;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace BTree.Editor
{
    public class BTreeEditorSerialization
    {
        public static string m_ConfigPath = "Assets/Editor/BTreeEditor/Config/";

        public static void WriteBinary(BTreeEditorConfig _bTree, string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(m_ConfigPath + _name + ".btree", System.IO.FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }

        public static BTreeEditorConfig ReadBinary(string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(m_ConfigPath + _name + ".btree", System.IO.FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            BTreeEditorConfig bTree = binaryFormatter.Deserialize(fs) as BTreeEditorConfig;
            fs.Close();
            return bTree;
        }

        public static void WriteXML(BTreeEditorConfig _bTree, string _name)
        {
            XmlSerializer writer = new XmlSerializer(typeof(BTreeEditorConfig));
            System.IO.StreamWriter file = new System.IO.StreamWriter(m_ConfigPath + _name + ".xml");
            writer.Serialize(file, _bTree);
            file.Close();
        }
        public static BTreeEditorConfig ReadXML(string _name)
        {
            XmlSerializer reader = new XmlSerializer(typeof(BTreeEditorConfig));
            System.IO.StreamReader file = new System.IO.StreamReader(m_ConfigPath + _name + ".xml");
            BTreeEditorConfig btree = reader.Deserialize(file) as BTreeEditorConfig;
            file.Close();
            return btree;
        }
    }
}
