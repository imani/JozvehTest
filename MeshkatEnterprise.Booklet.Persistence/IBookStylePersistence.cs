using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookStylePersistence
    {
        List<BookSectionStyle> GetBlockStyles(long startParagraphId, long endParagraphId, long volumeId);
        List<BookSectionStyle> GetAllStyles();
    }
}