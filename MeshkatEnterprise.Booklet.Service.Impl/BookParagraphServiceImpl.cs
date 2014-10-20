using System.Linq;
using Compositional.Composer;
using MeshkatEnterprise.Booklet.Entity;
using MeshkatEnterprise.Booklet.Persistence;
using MeshkatEnterprise.Infrastructure.General;
using MeshkatEnterprise.Infrastructure.General.ServiceResponse;
using MeshkatEnterprise.Security.API;

namespace MeshkatEnterprise.Booklet.Service.Impl
{
    [Component]
    public class BookParagraphServiceImpl : IBookParagraphService
    {
        private readonly IBookCommentPersistence _bookCommentPersistence;
        private readonly IBookHighlightPersistence _bookHighlightPersistence;
        private readonly IBookParagraphPersistence _bookParagraphPersistence;
        private readonly IBookStylePersistence _bookStylePersistence;
        private readonly BookParagraphCache _bookParagraphCache;
        private readonly IPersonPersistence _personPersistence;
        private bool cacheUseFlag;

        public BookParagraphServiceImpl(IBookParagraphPersistence bookParagraphPersistence,
            IBookStylePersistence bookStylePersistence,
            IBookHighlightPersistence bookHighlightPersistence, IBookCommentPersistence bookCommentPersistence, IPersonPersistence personPersistence)
        {
            cacheUseFlag = false;
            if (cacheUseFlag)
            {
                _bookParagraphCache = new BookParagraphCache();
                _bookParagraphCache.AllParagraphs = _bookParagraphPersistence.GetAllParagraphs();
                _bookParagraphCache.AllStyles = _bookStylePersistence.GetAllStyles();
            }
            _bookParagraphPersistence = bookParagraphPersistence;
            _bookStylePersistence = bookStylePersistence;
            _bookHighlightPersistence = bookHighlightPersistence;
            _bookCommentPersistence = bookCommentPersistence;
            _personPersistence = personPersistence;
        }

        public TServiceResult<BookParagraphsBlock> GetParagraphsBlock(long startParagraphId, long endParagraphId, long volumeId)
        {
            long? personId = _personPersistence.GetPersonId(SecurityTokenBinder.Lookup().UserName);
            var result = new BookParagraphsBlock();
            if (startParagraphId < 0 || endParagraphId < 0 || startParagraphId > endParagraphId)
            {
                return new TServiceResult<BookParagraphsBlock>(null,
                    new SimpleServiceResponse("Invalid Start and end paragraph"));
            }
            if (_bookParagraphCache == null)
            {
                result.Paragraphs = _bookParagraphPersistence.GetBlockParagraphs(startParagraphId, endParagraphId, volumeId);
                result.Styles = _bookStylePersistence.GetBlockStyles(startParagraphId, endParagraphId, volumeId);

                result.Highlights = _bookHighlightPersistence.GetBlockHighlights(startParagraphId, endParagraphId, volumeId, personId);


                result.Comments = _bookCommentPersistence.GetBlockComments(startParagraphId, endParagraphId, volumeId, personId);
                //Comment dummy
                 //Section commentSection = new Section();
                 //commentSection.ParagraphId = startParagraphId;
                 //commentSection.StartOffset = 15;
                 //commentSection.EndOffset = 25;
                 //BookComment comment = new BookComment();
                 //comment.Sections = new List<Section>();
                 //comment.Sections.Add(commentSection);
                 //comment.Text = "یک توضیح آزمایشی";
                 //comment.Type = new BookCommentType();
                 //comment.Type.BookCommentTypeTitle = "آزمایشی";
                 //BookComment comment2 = new BookComment();
                 //comment2.Sections = new List<Section>();
                 //comment2.Sections.Add(commentSection);
                 //comment2.Text = "توضیح 1";
                 //comment2.Type = new BookCommentType();
                 //comment2.Type.BookCommentTypeTitle = "نوع توضیح";
                 //result.Comments = new List<BookComment>();
                 //result.Comments.Add(comment);
                 //result.Comments.Add(comment2);
            }
            else
            {
                result.Paragraphs =
                    _bookParagraphCache.AllParagraphs.Where(a => a.Key >= startParagraphId && a.Key <= endParagraphId)
                        .Select(c => c.Value)
                        .ToList();
                result.Styles =
                    _bookParagraphCache.AllStyles.Where(
                        s => s.Section.ParagraphId >= startParagraphId && s.Section.ParagraphId <= endParagraphId)
                        .ToList();
            }

            return new TServiceResult<BookParagraphsBlock>(result);
        }

        public TServiceResult<BookParagraphsBlock> GetParagraphsByPageNumber(long volumeId, int pageNumber,
            int fetchSizeBefore, int fetchSizeAfter)
        {
            if (cacheUseFlag == true && _bookParagraphCache == null)
            {
                return new TServiceResult<BookParagraphsBlock>(null, new  SimpleServiceResponse("Cache is empty"), false);
            }
            else if (cacheUseFlag == false)
            {
                long paragraphId = _bookParagraphPersistence.GetParagraphId(volumeId, pageNumber);
                return GetParagraphsBlock(paragraphId - fetchSizeBefore, paragraphId + fetchSizeAfter, volumeId);
            }
            else
            {
                long paragraphId =
                _bookParagraphCache.AllParagraphs.FirstOrDefault(
                    a => a.Value.VolumeInfo.VolumeId == volumeId && a.Value.ParagraphPageNumber == pageNumber).Key;
                return GetParagraphsBlock(paragraphId - fetchSizeBefore, paragraphId + fetchSizeAfter, volumeId);
            }
            
        }
    }
}