using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public interface IEntity
    {
        int GetID();
        void SetID(int id);
        string GetCode();
        void SetCode(string code);
        string GetInsertSQL();
        string GetDeleteSQL();
        string GetUpdateSQL();
        string GetByIDSQL(int ID);
        string GetByCodeSQL(string code);
        string GetAllSQL();
        string GetMaximumIDSQL();
        string GetByCodeLikeSQL(string text);
        string GetByNameLikeSQL(string text);
        IEntity Get(OdbcDataReader aReader);
        IList GetAll(OdbcDataReader aReader);
    }
}
