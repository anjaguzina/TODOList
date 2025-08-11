using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODOList.Serializer;


namespace TODOList.DAO;

public class DAO<T> where T : class, IAccess<T>, ISerializable, new()
{
    private readonly List<T> _objects;
    private readonly Storage<T> _storage;

    public DAO()
    {
        string fileName = typeof(T).Name + ".csv";
        _storage = new Storage<T>(fileName);
        _objects = _storage.Load();
    }

    private int GenerateId()
    {
        if (_objects.Count == 0)
            return 0;

        return _objects[^1].Id + 1;
    }

    public T AddObject(T obj)
    {
        obj.Id = GenerateId();
        _objects.Add(obj);
        _storage.Save(_objects);
        return obj;
    }

    public T? UpdateObject(T obj)
    {
        T? oldObj = GetObjectById(obj.Id);
        if (oldObj is null) 
            return null;

        oldObj.Copy(obj);

        _storage.Save(_objects);
        return oldObj;
    }

    public T? RemoveObject(int id) 
    {
        T? obj = GetObjectById(id);
        if (obj == null)
            return null;

        _objects.Remove(obj);
        _storage.Save(_objects);
        return obj;
    }

    public T? GetObjectById(int id)
    {
        return _objects.Find(o => o.Id == id);
    }

    public List<T> GetAllObjects()
    {
        return _objects;
    }

    public void SaveToStorage()
    {
        _storage.Save(_objects);
    }
}
