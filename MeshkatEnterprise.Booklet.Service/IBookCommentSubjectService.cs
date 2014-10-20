using System.Collections.Generic;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Infrastructure.General;

namespace MeshkatEnterprise.Booklet.Service
{
    [Contract]
    public interface IBookCommentSubjectService
    {
        /// <summary>
        ///     گره‌های درخت موضوعی مربوط به حاشیه مورد نظر را برمی‌گرداند.
        /// </summary>
        /// <param name="commentSubjectId"></param>
        /// <param name="isRoot">آیا گره مورد نظر اولین گره درخت است یا خیر؟</param>
        /// <returns>گره مورد نظر موضوع به همراه فرزندانش</returns>
        TServiceResult<List<TreeNode>> GetCommentSubjects(int commentSubjectId, bool isRoot);
    }
}