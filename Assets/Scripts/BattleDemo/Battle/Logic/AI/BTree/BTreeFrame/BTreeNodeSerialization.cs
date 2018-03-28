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
namespace BTreeFrame
{
    public class BTreeNodeSerialization
    {
        public static string m_ConfigPath = "Assets/Data/config/";

        public static void WriteBinary(TreeConfig _bTree, string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(m_ConfigPath + _name + ".btree", System.IO.FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, _bTree);
            fs.Close();
        }

        public static TreeConfig ReadBinary(string _name)
        {
            System.IO.FileStream fs = new System.IO.FileStream(m_ConfigPath + _name + ".btree", System.IO.FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            TreeConfig bTree = binaryFormatter.Deserialize(fs) as TreeConfig;
            fs.Close();
            return bTree;
        }

        public static void WriteXML(TreeConfig _bTree, string _name)
        {
            XmlSerializer writer = new XmlSerializer(typeof(TreeConfig));
            System.IO.StreamWriter file = new System.IO.StreamWriter(m_ConfigPath + _name + ".xml");
            writer.Serialize(file, _bTree);
            file.Close();
        }
        public static TreeConfig ReadXML(string _name)
        {
            XmlSerializer reader = new XmlSerializer(typeof(TreeConfig));
            System.IO.StreamReader file = new System.IO.StreamReader(m_ConfigPath + _name + ".xml");
            TreeConfig btree = reader.Deserialize(file) as TreeConfig;
            file.Close();
            return btree;
        }
    }
}
