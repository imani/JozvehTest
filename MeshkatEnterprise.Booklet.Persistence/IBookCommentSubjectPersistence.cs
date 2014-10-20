using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;

namespace MeshkatEnterprise.Booklet.Persistence
{
    [Contract]
    public interface IBookCommentSubjectPersistence
    {
        List<TreeNode> GetCommentSubjects(int commentSubjectId);
        bool HasChild(int commentSubjectId);
    }
}