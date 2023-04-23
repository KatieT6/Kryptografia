using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DES.Data
{
    public class FileEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class FileRepository : IRepository<FileEntity>
    {
        private readonly string _directoryPath;

        public FileRepository(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Add(FileEntity entity)
        {
            string filePath = GetFilePath(entity.Name);
            if (File.Exists(filePath))
            {
                throw new InvalidOperationException($"File {entity.Name} already exists.");
            }
            File.WriteAllText(filePath, entity.Content);
        }

        public void Update(FileEntity entity)
        {
            string filePath = GetFilePath(entity.Name);
            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException($"File {entity.Name} does not exist.");
            }
            File.WriteAllText(filePath, entity.Content);
        }

        public void Delete(FileEntity entity)
        {
            string filePath = GetFilePath(entity.Name);
            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException($"File {entity.Name} does not exist.");
            }
            File.Delete(filePath);
        }

        public FileEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileEntity> GetAll()
        {
            return Directory.GetFiles(_directoryPath)
                .Select(filePath => new FileEntity
                {
                    Name = Path.GetFileName(filePath),
                    Content = File.ReadAllText(filePath)
                });
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(_directoryPath, fileName);
        }
    }
}
