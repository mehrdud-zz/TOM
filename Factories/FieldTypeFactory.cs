using ModelsLayer; 
using System.Collections.Generic;
using System.Data;
using System.Linq; 

namespace Factories
{
    public interface IFieldTypeFactory
    {
        void Initialize();
        FieldType GetFieldType(int fieldTypeId);
        List<FieldType> GetFieldTypes();
        bool CreateFieldType(FieldType fieldType);
        bool UpdateFieldType(FieldType fieldType);
        bool DeleteFieldType(FieldType fieldType);
        void Dispose(bool disposing);
    }

    public class FieldTypeFactory : IFieldTypeFactory
    {
        private readonly ClaimsEntities _db = new ClaimsEntities();

        public void Initialize()
        {

        }

        public FieldType GetFieldType(int fieldTypeId)
        {
            return _db.FieldTypes.Find(fieldTypeId);
        }

        public List<FieldType> GetFieldTypes()
        {
            return _db.FieldTypes.ToList(); 
        }

        public bool CreateFieldType(FieldType fieldType)
        {
            _db.FieldTypes.Add(fieldType);
            _db.SaveChanges();
            return true;
        }

        public bool UpdateFieldType(FieldType fieldType)
        {
            _db.Entry(fieldType).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        public bool DeleteFieldType(FieldType fieldType)
        {
            _db.FieldTypes.Remove(fieldType);
            _db.SaveChanges();
            return true;
        }

        public void Dispose(bool disposing)
        {
            _db.Dispose();
        } 
    }
}