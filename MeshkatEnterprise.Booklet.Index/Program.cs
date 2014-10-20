using System;
using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Booklet.Search;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Index
{
    class Program
    {
        static void Main(string[] args)
        {
            ContextUtil.Init();

            ComponentContext context = ComponentContextBinder.Lookup();
            var searchPersistence = context.GetComponent<ISearchPersistence>();

            List<BookParagraph> paragraphs = searchPersistence.GetAllIndexDocuments();
            
            var indexPath = @"Z:\Search\Index\";
            var dataPath = @"Z:\Search\Data\";
            var indexer = new Indexer(indexPath, dataPath);

            long volumeId = -1;
            int counter = 0;
            foreach (BookParagraph paragraph in paragraphs)
            {
                if (paragraph.VolumeInfo.VolumeId != volumeId)
                {
                    volumeId = paragraph.VolumeInfo.VolumeId;
                    Console.WriteLine(String.Format("Indexing Volume Id: {0}", volumeId));
                }
                else
                {
                    if (counter++/1000 > 0)
                    {
                        Console.Write("+ ");
                        counter = 0;
                    }
                }
                indexer.Index(paragraph);
                
            }
        }
    }
}
