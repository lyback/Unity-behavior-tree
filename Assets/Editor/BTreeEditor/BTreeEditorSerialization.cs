//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

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
