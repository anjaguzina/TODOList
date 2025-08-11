using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList.DAO;

public interface IAccess<T>
{
    int Id { get; set; }

    void Copy(T obj);
}
