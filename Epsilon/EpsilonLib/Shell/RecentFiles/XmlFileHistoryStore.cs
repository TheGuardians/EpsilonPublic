using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Shared
{
    [Export(typeof(IFileHistoryStore))]
    public class XmlFileHistoryStore : IFileHistoryStore
    {
        private readonly string _filePath;

        public XmlFileHistoryStore(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<FileHistoryRecord>> FetchRecords()
        {
            if (!File.Exists(_filePath))
                return Enumerable.Empty<FileHistoryRecord>();

            return await Task.Run(() =>
            {
                var records = new List<FileHistoryRecord>();

                using (var reader = XmlReader.Create(File.OpenText(_filePath)))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == "FileHistoryRecord")
                        {
                            var editorProviderId = Guid.Parse(reader.GetAttribute("EditorProviderId"));
                            var filePath = reader.GetAttribute("FilePath");
                            var lastOpened = DateTime.Parse(reader.GetAttribute("LastOpened"));
                            var record = new FileHistoryRecord(editorProviderId, filePath, lastOpened);
                            records.Add(record);
                        }
                    }
                }

                return records;
            });
        }

        public Task StoreRecords(IEnumerable<FileHistoryRecord> records)
        {
            return Task.Run(() =>
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true
                };

                using (var writer = XmlWriter.Create(File.CreateText("filehistory.xml"), settings))
                {

                    writer.WriteStartDocument();

                    writer.WriteStartElement("FileHistory");
                    foreach (var record in records)
                    {
                        writer.WriteStartElement("FileHistoryRecord");
                        writer.WriteAttributeString("EditorProviderId", record.EditorProviderId.ToString());
                        writer.WriteAttributeString("FilePath", record.FilePath);
                        writer.WriteAttributeString("LastOpened", record.LastOpened.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            });
        }
    }
}
