using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TODOList.Serializer;
public class Storage<T> where T : ISerializable, new()
{
    private readonly string _fileName = @"../../../../TODOList/Resources/Data/{0}";


    private readonly Serializer<T> _serializer = new();

    public Storage(string fileName)
    {
        _fileName = string.Format(_fileName, fileName);
    }
    public List<T> Load()
    {
        if (!File.Exists(_fileName))
        {
            FileStream fs = File.Create(_fileName);
            fs.Close();
        }

        List<T> objects = _serializer.FromCSV(_fileName);
        return objects;
    }

    public void Save(List<T> objects)
    {
        _serializer.ToCSV(_fileName, objects);
    }

}